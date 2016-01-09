<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 09.01.2016
 * Time: 15:16
 */

namespace famoser\beercompanion\webpage\controllers;


use famoser\beercompanion\webpage\core\interfaces\iController;
use famoser\beercompanion\webpage\models\Beer;
use famoser\beercompanion\webpage\models\communication\BeerCollection;
use famoser\beercompanion\webpage\models\Drinker;
use PDO;

class BeerController implements iController
{
    function execute($param, $post)
    {
        if (count($param) > 0 && ($param[0] == "add" || $param[0] == "delete")) {
            $obj = json_decode($post);
            $drinker = GetByGuid("Drinker", $obj->Guid);
            if ($drinker != null && $drinker instanceof Drinker) {
                if ($param[0] == "delete") {
                    $existingBeers = array();
                    foreach ($obj->Beers as $beer) {
                        $existingBeer = GetSingleByCondition("Beers", array("Guid" => $beer->Guid));
                        if ($existingBeer != null)
                            $existingBeers[] = $existingBeer;
                    }
                    return DeleteAll($existingBeers) && $this->refreshDrinkerProperties($drinker);
                } else {
                    $newBeers = array();
                    foreach ($obj->Beers as $beer) {
                        $newbeer = new Beer();
                        $newbeer->Guid = $beer->Guid;
                        $newbeer->DrinkTime = $beer->DrinkTime;
                        $newbeer->DrinkerId = $drinker->Id;
                        $newBeers[] = $beer;
                    }
                    return InsertAll($newBeers) && $this->refreshDrinkerProperties($drinker);
                }
            }
            return DRINKER_NOT_FOUND;
        } else if (count($param) > 0) {
            $drinker = GetByGuid("Drinker", $param[0]);
            if ($drinker != null && $drinker instanceof Drinker) {
                $beers = GetAllByCondition("Beers", array("DrinkerId" => $drinker->Id));
                return json_encode($beers);
            }
            return DRINKER_NOT_FOUND;
        }
        return LINK_INVALID;
    }

    private function refreshDrinkerProperties(Drinker $drinker)
    {
        $db = GetDatabaseConnection();
        $pdo = $db->query("SELECT COUNT(*) FROM Beers WHERE DrinkerId=:Id");
        $pdo->setAttribute(":Id", $drinker->Id);
        $drinker->TotalBeers = $pdo->fetch(PDO::FETCH_NUM)[0];

        $pdo = $db->query("SELECT DrinkTime FROM Beers WHERE DrinkerId=:Id ORDER BY DrinkTime DESC LIMIT 1");
        $pdo->setAttribute(":Id", $drinker->Id);
        $drinker->LastBeer = $pdo->fetch(PDO::FETCH_NUM)[0];

        return Update("Drinkers", $drinker);
    }
}