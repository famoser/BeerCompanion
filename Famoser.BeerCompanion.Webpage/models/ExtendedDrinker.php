<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 09.01.2016
 * Time: 17:01
 */

namespace famoser\beercompanion\webpage\models;


class ExtendedDrinker extends Drinker
{
    public function __construct(Drinker $drinker)
    {
        $this->Id = $drinker->Id;
        $this->Guid = $drinker->Guid;
        $this->Name = $drinker->Name;
        $this->LastBeer = $drinker->LastBeer;
        $this->TotalBeers = $drinker->TotalBeers;
    }

    public $DrinkerCylces;
}