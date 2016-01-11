<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 11.01.2016
 * Time: 12:42
 */

namespace famoser\beercompanion\webpage\models\entities;


use famoser\beercompanion\webpage\models\Beer;

class BeerEntity extends Beer
{
    public function __construct(Beer $ds)
    {
        //$this->Id = $ds->Id; Id censored
        $this->Guid = $ds->Guid;
        $this->DrinkTime = $ds->DrinkTime;
    }
}