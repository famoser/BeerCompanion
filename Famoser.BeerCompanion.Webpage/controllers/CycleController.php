<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 09.01.2016
 * Time: 14:45
 */

namespace famoser\beercompanion\webpage\controllers;


use famoser\beercompanion\webpage\core\interfaces\iController;
use function famoser\beercompanion\webpage\core\responsehelper\RelationNotFound;
use function famoser\beercompanion\webpage\core\responsehelper\ReturnBoolean;
use function famoser\beercompanion\webpage\core\responsehelper\ReturnCrudError;
use function famoser\beercompanion\webpage\core\responsehelper\ReturnError;
use function famoser\beercompanion\webpage\core\responsehelper\ReturnNotAuthenticated;
use function famoser\beercompanion\webpage\core\responsehelper\ReturnNotFound;
use function famoser\beercompanion\webpage\core\validationhelper\GenerateGuid;
use function famoser\beercompanion\webpage\core\validationhelper\ValidateGuid;
use famoser\beercompanion\webpage\models\communication\DrinkerCycleResponse;
use famoser\beercompanion\webpage\models\Drinker;
use famoser\beercompanion\webpage\models\DrinkerCycle;
use famoser\beercompanion\webpage\models\DrinkerCyclesDrinkersRelation;
use famoser\beercompanion\webpage\models\entities\DrinkerCycleEntity;
use famoser\beercompanion\webpage\models\entities\DrinkerEntity;

class CycleController implements iController
{
    function execute($param, $post)
    {
        if (count($param) > 0 && $param[0] == "act") {
            $obj = json_decode($post["json"]);
            $group = GetSingleByCondition(DRINKERCYCLE_TABLE, array("Name" => $obj->Name));
            if ($group instanceof DrinkerCycle) {
                if ($obj->Action == "exists") {
                    $groupRela = GetSingleByCondition(DRINKERCYCLESDRINKERSRELATION_TABLE, array("DrinkerCycleId" => $group->Id));
                    return ReturnBoolean($groupRela != null);
                } else if ($obj->Action == "add") {
                    $drinker = GetSingleByCondition(DRINKER_TABLE, array("Guid" => $obj->Guid));
                    if ($drinker instanceof Drinker) {
                        $presGroupRelation = GetSingleByCondition(DRINKERCYCLESDRINKERSRELATION_TABLE, array("DrinkerCycleId" => $group->Id, "DrinkerId" => $drinker->Id));
                        if ($presGroupRelation == null) {
                            $groupRela = GetSingleByCondition(DRINKERCYCLESDRINKERSRELATION_TABLE, array("DrinkerCycleId" => $group->Id));

                            $newRela = new DrinkerCyclesDrinkersRelation();
                            $newRela->DrinkerId = $drinker->Id;
                            $newRela->DrinkerCycleId = $group->Id;
                            $newRela->IsAuthenticated = $groupRela == null;
                            return ReturnBoolean(Insert(DRINKERCYCLESDRINKERSRELATION_TABLE, $newRela));
                        }
                        return ReturnBoolean(true);
                    } else {
                        return ReturnNotFound($obj->Guid, "Drinker");
                    }
                } else if ($obj->Action == "remove") {
                    $drinker = GetSingleByCondition(DRINKER_TABLE, array("Guid" => $obj->Guid));
                    if ($drinker instanceof Drinker) {
                        $groupRela = GetSingleByCondition(DRINKERCYCLESDRINKERSRELATION_TABLE, array("DrinkerCycleId" => $group->Id, "DrinkerId" => $drinker->Id));
                        if ($groupRela instanceof DrinkerCyclesDrinkersRelation) {
                            return ReturnBoolean(DeleteById(DRINKERCYCLESDRINKERSRELATION_TABLE, $groupRela->Id));
                        } else {
                            return RelationNotFound($group->Id, $drinker->Id, DRINKERCYCLESDRINKERSRELATION_TABLE);
                        }
                    } else {
                        return ReturnNotFound($obj->Guid, DRINKER_TABLE);
                    }
                } else if ($obj->Action == "authenticate" || $obj->Action == "deauthenticate") {
                    $newVal = true;
                    if ($obj->Action == "deauthenticate")
                        $newVal = false;
                    $drinker = GetSingleByCondition(DRINKER_TABLE, array("Guid" => $obj->Guid));
                    if ($drinker instanceof Drinker) {
                        $groupRela = GetSingleByCondition(DRINKERCYCLESDRINKERSRELATION_TABLE, array("DrinkerCycleId" => $group->Id, "DrinkerId" => $drinker->Id));
                        if ($groupRela instanceof DrinkerCyclesDrinkersRelation) {
                            //can change others status
                            if ($groupRela->IsAuthenticated) {
                                $otherDrinker = GetSingleByCondition(DRINKER_TABLE, array("Guid" => $obj->AuthGuid));
                                if ($otherDrinker instanceof Drinker) {
                                    $otherGroupRela = GetSingleByCondition(DRINKERCYCLESDRINKERSRELATION_TABLE, array("DrinkerCycleId" => $group->Id, "DrinkerId" => $otherDrinker->Id));
                                    if ($otherGroupRela instanceof DrinkerCyclesDrinkersRelation) {
                                        $otherGroupRela->IsAuthenticated = $newVal;
                                        return ReturnBoolean(Update(DRINKERCYCLESDRINKERSRELATION_TABLE, $otherGroupRela));
                                    } else {
                                        return RelationNotFound($group->Id, $otherDrinker->Id, DRINKERCYCLESDRINKERSRELATION_TABLE);
                                    }
                                } else {
                                    return ReturnNotFound($obj->AuthGuid, "Drinker");
                                }
                            } else {
                                //not authenticated
                                return ReturnBoolean(false);
                            }
                        } else {
                            return RelationNotFound($group->Id, $drinker->Id, DRINKERCYCLESDRINKERSRELATION_TABLE);
                        }
                    } else {
                        return ReturnNotFound($obj->Guid, "Drinker");
                    }
                } else if ($obj->Action == "removeforeign") {
                    $drinker = GetSingleByCondition(DRINKER_TABLE, array("Guid" => $obj->Guid));
                    if ($drinker instanceof Drinker) {
                        $groupRela = GetSingleByCondition(DRINKERCYCLESDRINKERSRELATION_TABLE, array("DrinkerCycleId" => $group->Id, "DrinkerId" => $drinker->Id));
                        if ($groupRela instanceof DrinkerCyclesDrinkersRelation) {
                            //can change others status
                            if ($groupRela->IsAuthenticated) {
                                $otherDrinker = GetSingleByCondition(DRINKER_TABLE, array("Guid" => $obj->AuthGuid));
                                if ($otherDrinker instanceof Drinker) {
                                    $otherGroupRela = GetSingleByCondition(DRINKERCYCLESDRINKERSRELATION_TABLE, array("DrinkerCycleId" => $group->Id, "DrinkerId" => $otherDrinker->Id));
                                    if ($otherGroupRela instanceof DrinkerCyclesDrinkersRelation) {
                                        return ReturnBoolean(Delete(DRINKERCYCLESDRINKERSRELATION_TABLE, $otherGroupRela));
                                    } else {
                                        return RelationNotFound($group->Id, $otherDrinker->Id, DRINKERCYCLESDRINKERSRELATION_TABLE);
                                    }
                                } else {
                                    return ReturnNotFound($obj->AuthGuid, "Drinker");
                                }
                            } else {
                                //not authenticated
                                return ReturnBoolean(false);
                            }
                        } else {
                            return RelationNotFound($group->Id, $drinker->Id, DRINKERCYCLESDRINKERSRELATION_TABLE);
                        }
                    } else {
                        return ReturnNotFound($obj->Guid, "Drinker");
                    }
                } else {
                    return ReturnError(LINK_INVALID);
                }
            }
            if ($obj->Action == "exists") {
                return ReturnBoolean(false);
            } else if ($obj->Action == "add") {
                $newGroup = new DrinkerCycle();
                $newGroup->Name = $obj->Name;
                $newGroup->Guid = GenerateGuid();
                if (Insert(DRINKERCYCLE_TABLE, $newGroup)) {
                    return $this->execute($param, $post);
                } else {
                    return ReturnCrudError($newGroup, "add");
                }
            } else {
                return ReturnNotFound($obj->Guid, DRINKERCYCLE_TABLE);
            }
        } else if (ValidateGuid($param[0])) {
            //construct model puh
            $drinker = GetByGuid("Drinker", $param[0]);
            if ($drinker != null && $drinker instanceof Drinker) {
                $relations = GetAllByCondition(DRINKERCYCLESDRINKERSRELATION_TABLE, array("DrinkerId" => $drinker->Id));

                $cyclesEnt = array();
                $cycles = array();
                $authCycles = array();
                foreach ($relations as $relation) {
                    if ($relation instanceof DrinkerCyclesDrinkersRelation) {
                        $cycle = GetById(DRINKERCYCLE_TABLE, $relation->DrinkerCycleId);
                        if ($cycle instanceof DrinkerCycle) {
                            if ($relation->IsAuthenticated) {
                                $cycl = new DrinkerCycleEntity($cycle);
                                $cycl->IsAuthenticated = true;
                                $authCycles[] = $cycle;
                                $cyclesEnt[] = $cycl;
                            } else {
                                $cycles[] = $cycle;
                                $cycl = new DrinkerCycleEntity($cycle);
                                $cycl->IsAuthenticated = false;
                                $cyclesEnt[] = $cycl;
                            }
                        }
                    }
                }

                $drinkers = array();
                foreach ($authCycles as $cycle) {
                    $userRelations = GetAllByCondition(DRINKERCYCLESDRINKERSRELATION_TABLE, array("DrinkerCycleId" => $cycle->Id));
                    foreach ($userRelations as $userRelation) {
                        if ($userRelation instanceof DrinkerCyclesDrinkersRelation) {
                            //exclude self
                            if ($drinker->Id != $userRelation->DrinkerId) {
                                if (!isset($drinkers[$userRelation->DrinkerId])) {
                                    $user = GetById(DRINKER_TABLE, $userRelation->DrinkerId);
                                    if ($user instanceof Drinker) {
                                        $drinkers[$userRelation->DrinkerId] = new DrinkerEntity($user);
                                    }
                                }

                                if ($drinkers[$userRelation->DrinkerId] instanceof DrinkerEntity) {
                                    if ($userRelation->IsAuthenticated) {
                                        $drinkers[$userRelation->DrinkerId]->AuthDrinkerCycles[] = $cycle->Guid;
                                    } else {
                                        $drinkers[$userRelation->DrinkerId]->NonAuthDrinkerCycles[] = $cycle->Guid;
                                    }
                                }
                            }
                        }
                    }
                }

                $coll = new DrinkerCycleResponse();
                $coll->DrinkerCycles = $cyclesEnt;
                foreach ($drinkers as $drinker) {
                    $coll->Drinkers[] = $drinker;
                }
                return json_encode($coll);
            } else {
                return ReturnNotFound($param[0], "Drinker");
            }
        } else {
            return ReturnError(LINK_INVALID);
        }
    }
}