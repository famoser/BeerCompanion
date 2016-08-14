<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 09.01.2016
 * Time: 18:01
 */

namespace famoser\beercompanion\webpage\controllers;


use famoser\beercompanion\webpage\core\interfaces\IController;
use function famoser\beercompanion\webpage\core\responsehelper\ReturnBoolean;
use function famoser\beercompanion\webpage\core\responsehelper\ReturnCrudError;
use function famoser\beercompanion\webpage\core\responsehelper\ReturnError;
use function famoser\beercompanion\webpage\core\responsehelper\ReturnNotFound;
use function famoser\beercompanion\webpage\core\validationhelper\ValidateGuid;
use famoser\beercompanion\webpage\models\communication\DrinkerResponse;
use famoser\beercompanion\webpage\models\Drinker;
use famoser\beercompanion\webpage\models\DrinkerCycle;
use famoser\beercompanion\webpage\models\DrinkerCyclesDrinkersRelation;
use famoser\beercompanion\webpage\models\entities\DrinkerEntity;

class DrinkerController implements IController
{
    function execute($param, $post)
    {
        if (count($param) > 0) {
            if ($param[0] == "act") {
                $obj = json_decode($post["json"]);
                $user = GetByGuid("Drinker", $obj->Guid);
                if ($obj->Action == "exists") {
                    return ReturnBoolean($user != null);
                } else if ($obj->Action == "update") {
                    if ($user instanceof Drinker) {
                        $user->Name = $obj->UserInformations->Name;
                        $user->Color = $obj->UserInformations->Color;
                        return ReturnBoolean(Update(DRINKER_TABLE, $user));
                    } else {
                        $user = new Drinker();
                        $user->Name = $obj->UserInformations->Name;
                        $user->Color = $obj->UserInformations->Color;
                        $user->Guid = $obj->Guid;
                        return ReturnBoolean(Insert(DRINKER_TABLE, $user));
                    }
                } else if ($obj->Action == "remove") {
                    if ($user instanceof Drinker) {
                        //remove all DrinkerCycleRelations
                        $relations = GetAllByCondition(DRINKERCYCLESDRINKERSRELATION_TABLE, array("DrinkerId" => $user->Id));
                        if (DeleteAll($relations))
                            return ReturnBoolean(Delete(DRINKER_TABLE, $user));
                        else
                            return ReturnCrudError($relations, "DeleteAll");
                    } else {
                        return ReturnNotFound($obj->Guid, "Drinker");
                    }
                }
            } else if (ValidateGuid($param[0])) {
                $user = GetByGuid("Drinker", $param[0]);
                if ($user instanceof Drinker) {
                    $drinker = new DrinkerEntity($user);
                    $relations = GetAllByCondition(DRINKERCYCLESDRINKERSRELATION_TABLE, array("DrinkerId" => $drinker->Id));
                    foreach ($relations as $relation) {
                        if ($relation instanceof DrinkerCyclesDrinkersRelation) {
                            $cycle = GetById(DRINKER_TABLE, $relation->DrinkerCycleId);
                            if ($cycle instanceof DrinkerCycle) {
                                if ($relation->IsAuthenticated) {
                                    $drinker->AuthDrinkerCycles[] = $cycle->Guid;
                                } else {
                                    $drinker->NonAuthDrinkerCycles[] = $cycle->Guid;
                                }
                            }
                        }
                    }
                    $resp = new DrinkerResponse();
                    $resp->Drinker = $drinker;
                    return json_encode($resp);
                } else {
                    return ReturnNotFound($param[0], "Drinker");
                }
            }
        }
        return ReturnError(LINK_INVALID);
    }
}