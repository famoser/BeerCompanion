<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 09.01.2016
 * Time: 14:45
 */

namespace famoser\beercompanion\webpage\controllers;


use famoser\beercompanion\webpage\core\interfaces\iController;
use famoser\beercompanion\webpage\models\communication\DrinkerCycleCommunication;
use famoser\beercompanion\webpage\models\CycleResponseCollection;
use famoser\beercompanion\webpage\models\Drinker;
use famoser\beercompanion\webpage\models\DrinkerCycle;
use famoser\beercompanion\webpage\models\DrinkerCycleDrinkerRelation;
use famoser\beercompanion\webpage\models\ExtendedDrinker;
use famoser\beercompanion\webpage\models\ExtendedDrinkerCycle;

class CycleController implements iController
{
    function execute($param, $post)
    {
        function execute($param, $post)
        {
            if (count($param) > 0 && $param[0] == "act") {
                $obj = json_decode($post);
                $group = GetSingleByCondition("DrinkerCycles", array("Name" => $obj->Name));
                if ($group instanceof DrinkerCycle) {
                    if ($obj->Action == "exist") {
                        $groupRela = GetSingleByCondition("DrinkerCyclesDrinkerRelations", array("DrinkerCycleId" => $group->Id));
                        return $groupRela != null;
                    } else if ($obj->Action == "add") {
                        $drinker = GetSingleByCondition("Drinkers", array("Guid" => $obj->Guid));
                        if ($drinker instanceof Drinker) {
                            $groupRela = GetSingleByCondition("DrinkerCyclesDrinkerRelations", array("DrinkerCycleId" => $group->Id));
                            $newRela = new DrinkerCycleDrinkerRelation();
                            $newRela->DrinkerId = $drinker->Id;
                            $newRela->DrinkerCycleId = $group->Id;
                            $newRela->IsAuthenticated = $groupRela == null;
                            return Insert("DrinkerCyclesDrinkerRelations", $newRela);
                        } else {
                            return DRINKER_NOT_FOUND;
                        }
                    } else if ($obj->Action == "remove") {
                        $drinker = GetSingleByCondition("Drinkers", array("Guid" => $obj->Guid));
                        if ($drinker instanceof Drinker) {
                            $groupRela = GetSingleByCondition("DrinkerCyclesDrinkerRelations", array("DrinkerCycleId" => $group->Id, "DrinkerId" => $drinker->Id));
                            if ($groupRela instanceof DrinkerCycleDrinkerRelation) {
                                return DeleteById("DrinkerCyclesDrinkerRelations", $groupRela->Id);
                            } else {
                                return DRINKER_CYCLE_RELATION_NOT_FOUND;
                            }
                        } else {
                            return DRINKER_NOT_FOUND;
                        }
                    } else if ($obj->Action == "authenticate") {
                        $drinker = GetSingleByCondition("Drinkers", array("Guid" => $obj->Guid));
                        if ($drinker instanceof Drinker) {
                            $groupRela = GetSingleByCondition("DrinkerCyclesDrinkerRelations", array("DrinkerCycleId" => $group->Id, "DrinkerId" => $drinker->Id));
                            if ($groupRela instanceof DrinkerCycleDrinkerRelation) {
                                $groupRela->IsAuthenticated = true;
                                return Update("DrinkerCyclesDrinkerRelations", $groupRela);
                            } else {
                                return DRINKER_CYCLE_RELATION_NOT_FOUND;
                            }
                        } else {
                            return DRINKER_NOT_FOUND;
                        }
                    } else if ($obj->Action == "deauthenticate") {
                        $drinker = GetSingleByCondition("Drinkers", array("Guid" => $obj->Guid));
                        if ($drinker instanceof Drinker) {
                            $groupRela = GetSingleByCondition("DrinkerCyclesDrinkerRelations", array("DrinkerCycleId" => $group->Id, "DrinkerId" => $drinker->Id));
                            if ($groupRela instanceof DrinkerCycleDrinkerRelation) {
                                return DeleteById("DrinkerCyclesDrinkerRelations", $groupRela->Id);
                            } else {
                                return DRINKER_CYCLE_RELATION_NOT_FOUND;
                            }
                        } else {
                            return DRINKER_NOT_FOUND;
                        }
                    } else {
                        return LINK_INVALID;
                    }
                }
                if ($obj->Action == "exist") {
                    return false;
                } else if ($obj->Action == "add") {
                    $newGroup = new DrinkerCycle();
                    $newGroup->Name = $obj->Name;
                    if (Insert("DrinkerCycles", $newGroup)) {
                        return $this->execute($param, $post);
                    } else {
                        return DRINKER_CYCLE_ADD_FAILED;
                    }
                }
                return DRINKER_CYCLE_NOT_FOUND;
            } else if (count($param) > 0) {
                //construct model puh
                $drinker = GetByGuid("Drinker", $param[0]);
                if ($drinker != null && $drinker instanceof Drinker) {
                    $relations = GetAllByCondition("DrinkerCyclesDrinkerRelations", array("DrinkerId" => $drinker->Id));

                    $cycles = array();
                    $authCycles = array();
                    foreach ($relations as $relation) {
                        if ($relation instanceof DrinkerCycleDrinkerRelation) {
                            $cycle = GetById("DrinkerCycles", $relation->DrinkerCycleId);
                            if ($cycle instanceof DrinkerCycle) {
                                if ($relation->IsAuthenticated) {
                                    $cycl = new ExtendedDrinkerCycle($cycle);
                                    $cycl->IsAuthenticated = true;
                                    $authCycles[] = $cycl;
                                } else
                                    $cycles[] = new ExtendedDrinkerCycle($cycle);
                            }
                        }
                    }

                    $drinkers = array();
                    foreach ($authCycles as $cycle) {
                        $userRelations = GetAllByCondition("DrinkerCyclesDrinkerRelations", array("DrinkerCycleId" => $cycle->Id));
                        foreach ($userRelations as $userRelation) {
                            if ($userRelation instanceof DrinkerCycleDrinkerRelation) {

                                if (!isset($drinkers[$userRelation->DrinkerId])) {
                                    $user = GetById("Drinkers", $userRelation->DrinkerId);
                                    if ($user instanceof Drinker) {
                                        $drinkers[$userRelation->DrinkerId] = new ExtendedDrinker($user);
                                    }
                                }

                                if ($drinkers[$userRelation->DrinkerId] instanceof ExtendedDrinker) {
                                    $drinkers[$userRelation->DrinkerId]->DrinkerCylces[$drinker->Guid][$userRelation->IsAuthenticated];
                                }
                            }
                        }
                    }

                    $coll = new CycleResponseCollection();
                    $coll->DrinkerCycles = $authCycles;
                    foreach ($cycles as $cyle) {
                        $coll->DrinkerCycles[] = $cyle;
                    }
                    $coll->Drinkers = $drinkers;
                    return json_encode($coll);
                }
                return DRINKER_NOT_FOUND;
            }
            return LINK_INVALID;
        }
    }
}