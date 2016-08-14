<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 11.01.2016
 * Time: 20:17
 */

namespace famoser\beercompanion\webpage\controllers;


use famoser\beercompanion\webpage\core\interfaces\IController;

class ApiController implements IController
{
    function execute($param, $post)
    {
        return "Online";
    }
}