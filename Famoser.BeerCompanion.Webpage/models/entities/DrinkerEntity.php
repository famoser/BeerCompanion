<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 11.01.2016
 * Time: 12:36
 */

namespace famoser\beercompanion\webpage\models\entities;


use famoser\beercompanion\webpage\models\Drinker;

class DrinkerEntity extends Drinker
{
    public function __construct(Drinker $drinker)
    {
        //$this->Id = $drinker->Id; Id censored
        //$this->Guid = $drinker->Guid; Guid censored
        $this->Name = $drinker->Name;
        $this->Color = $drinker->Color;
        $this->LastBeer = $drinker->LastBeer;
        $this->TotalBeers = $drinker->TotalBeers;
    }

    public $AuthDrinkerCycles = array();
    public $NonAuthDrinkerCycles = array();
}