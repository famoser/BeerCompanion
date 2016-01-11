<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 09.01.2016
 * Time: 18:01
 */

namespace famoser\beercompanion\webpage\controllers;


use famoser\beercompanion\webpage\core\interfaces\iController;
use famoser\beercompanion\webpage\models\Drinker;

class DrinkerController implements iController
{
    function execute($param, $post)
    {
        if (count($param) > 0 && ($param[0] == "act")) {
            $obj = json_decode($post);
            $user = GetByGuid("Drinker", $post->Guid);
            if ($obj->Action == "exists") {
                return $user != null;
            }
            else if ($obj->Action == "update") {
                if ($user instanceof Drinker) {
                    $user->Name = $obj->UserInformations->Name;
                    $user->Color = $obj->UserInformations->Color;
                    return Update("Drinkers", $user);
                } else {
                    return Insert("Drinkers", $user);
                }
            } else if ($obj->Action == "remove") {
                if ($user instanceof Drinker) {
                    //remove all DrinkerCycleRelations
                    $relations = GetAllByCondition("DrinkerCyclesDrinkerRelations",array("DrinkerId" => $user->Id));
                    if (DeleteAll($relations))
                        return Delete("Drinkers",$user);
                }
            }
        }
        return LINK_INVALID;
    }
}