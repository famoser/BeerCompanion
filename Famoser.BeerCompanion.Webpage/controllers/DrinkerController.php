<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 09.01.2016
 * Time: 18:01
 */

namespace famoser\beercompanion\webpage\controllers;


use famoser\beercompanion\webpage\core\interfaces\iController;
use famoser\beercompanion\webpage\models\Drinker;

class DrinkerController implements iController
{
    function execute($param, $post)
    {
        if (count($param) > 0 && ($param[0] == "update")) {
            $obj = json_decode($post);
            $user = GetByGuid("Drinker", $post->Guid);
            if ($user instanceof Drinker) {
                $user->Guid = $obj->Guid;
                $user->Name = $obj->Name;
                return Update("Drinkers", $user);
            } else {
                return Insert("Drinkers", $user);
            }
        }
        return LINK_INVALID;
    }
}