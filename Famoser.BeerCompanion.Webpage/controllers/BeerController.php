<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 09.01.2016
 * Time: 15:16
 */

namespace famoser\beercompanion\webpage\controllers;


use famoser\beercompanion\webpage\core\interfaces\iController;
use function famoser\beercompanion\webpage\core\responsehelper\ReturnBoolean;
use function famoser\beercompanion\webpage\core\responsehelper\ReturnError;
use function famoser\beercompanion\webpage\core\responsehelper\ReturnNotFound;
use function famoser\beercompanion\webpage\core\validationhelper\ConvertToDatabaseDateTime;
use function famoser\beercompanion\webpage\core\validationhelper\ValidateGuid;
use famoser\beercompanion\webpage\models\Beer;
use famoser\beercompanion\webpage\models\communication\BeerCollection;
use famoser\beercompanion\webpage\models\communication\BeerResponse;
use famoser\beercompanion\webpage\models\Drinker;
use famoser\beercompanion\webpage\models\entities\BeerEntity;
use PDO;

class BeerController implements iController
{
    function execute($param, $post)
    {
        if (count($param) > 0) {
            if ($param[0] == "act") {
                $obj = json_decode($post["json"]);
                $drinker = GetByGuid("Drinker", $obj->Guid);
                if ($drinker instanceof Drinker) {
                    if ($obj->Action == "remove") {
                        $existingBeers = array();
                        foreach ($obj->Beers as $beer) {
                            $existingBeer = GetSingleByCondition("Beers", array("Guid" => $beer->Guid));
                            if ($existingBeer != null)
                                $existingBeers[] = $existingBeer;
                        }
                        return ReturnBoolean(DeleteAll($existingBeers) && $this->refreshDrinkerProperties($drinker));
                    } else if ($obj->Action == "add") {
                        $newBeers = array();
                        foreach ($obj->Beers as $beer) {
                            $existingBeer = GetSingleByCondition("Beers", array("Guid" => $beer->Guid));
                            if ($existingBeer == null) {
                                $newbeer = new Beer();
                                $newbeer->Guid = $beer->Guid;
                                $newbeer->DrinkTime = ConvertToDatabaseDateTime($beer->DrinkTime);
                                $newbeer->DrinkerId = $drinker->Id;
                                $newBeers[] = $newbeer;
                            }
                        }
                        return ReturnBoolean(InsertAll($newBeers) && $this->refreshDrinkerProperties($drinker));
                    }else if ($obj->Action == "sync") {
                        $beercount = $this->countExistingBeers($drinker);
                        if ($beercount !== $obj->ExpectedCount)
                            return ReturnBoolean(false);


                        $beers = GetAllByCondition("Beers", array("DrinkerId" => $drinker->Id), "DrinkTime DESC", " LIMIT " . count($obj->Beers));
                        for ($i = 0; $i < count($beers); $i++)
                        {
                            if ($beers[$i]->Guid != $obj->Beers[$i]->Guid)
                                return ReturnBoolean(false);
                        }

                        return ReturnBoolean(true);
                    } else {
                        return ReturnError(LINK_INVALID);
                    }
                }
                return ReturnNotFound($param[0], "Drinker");
            } else if (ValidateGuid($param[0])) {
                $drinker = GetByGuid("Drinker", $param[0]);
                if ($drinker != null && $drinker instanceof Drinker) {
                    /*
                    $limit = 0;
                    if (count($param) > 1 && is_numeric($param[1])) {
                        $beerCount = $this->countExistingBeers($drinker);
                        if ($beerCount > $param[1]) {
                            $limit = $beerCount - $param[0];
                        }
                    }
                    $limitstring = " LIMIT " . $limit;
                    if ($limit == 0)
                        $limitstring = "";
                    */

                    $beers = GetAllByCondition("Beers", array("DrinkerId" => $drinker->Id), " DrinkTime DESC ");
                    $resp = new BeerResponse();
                    foreach ($beers as $beer) {
                        $resp->Beers[] = new BeerEntity($beer);
                    }
                    return json_encode($resp);
                } else {
                    return ReturnNotFound($param[0], "Drinker");
                }
            }
        }
        return ReturnError(LINK_INVALID);
    }

    private function refreshDrinkerProperties(Drinker $drinker)
    {
        $drinker->TotalBeers = $this->countExistingBeers($drinker);

        $db = GetDatabaseConnection();
        $pdo = $db->prepare("SELECT DrinkTime FROM Beers WHERE DrinkerId=:Id ORDER BY DrinkTime DESC LIMIT 1");
        $pdo->bindParam(":Id", $drinker->Id);
        $pdo->execute();
        $drinker->LastBeer = $pdo->fetch(PDO::FETCH_NUM)[0];

        return Update("Drinkers", $drinker);
    }

    private function countExistingBeers(Drinker $drinker)
    {
        $db = GetDatabaseConnection();
        $pdo = $db->prepare("SELECT COUNT(*) FROM Beers WHERE DrinkerId=:Id");
        $pdo->bindParam(":Id", $drinker->Id);
        $pdo->execute();

        return $pdo->fetch(PDO::FETCH_NUM)[0];
    }
}