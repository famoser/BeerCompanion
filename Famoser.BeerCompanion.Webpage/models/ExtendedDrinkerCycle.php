<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 09.01.2016
 * Time: 17:13
 */

namespace famoser\beercompanion\webpage\models;


class ExtendedDrinkerCycle extends DrinkerCycle
{
    public function __construct(DrinkerCycle $ds)
    {
        $this->Name = $ds->Name;
        $this->Id = $ds->Id;
    }

    public $IsAuthenticated;
}