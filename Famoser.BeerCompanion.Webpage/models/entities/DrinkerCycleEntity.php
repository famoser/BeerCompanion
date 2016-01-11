<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 11.01.2016
 * Time: 12:41
 */

namespace famoser\beercompanion\webpage\models\entities;


use famoser\beercompanion\webpage\models\DrinkerCycle;

class DrinkerCycleEntity extends DrinkerCycle
{
    public function __construct(DrinkerCycle $ds)
    {
        //$this->Id = $ds->Id; Id censored
        $this->Name = $ds->Name;
        $this->Guid = $ds->Guid;
    }

    public $IsAuthenticated;
}