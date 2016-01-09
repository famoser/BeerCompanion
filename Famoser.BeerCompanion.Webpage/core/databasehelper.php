<?php
/**
 * Created by PhpStorm.
 * User: Florian Moser
 * Date: 09.01.2016
 * Time: 14:46
 */
function GetDatabaseConnection()
{
    $db = new PDO("mysql:host=" . DATABASE_HOST . ";dbname=" . DATABASE_NAME . ";charset=utf8", DATABASE_USER, DATABASE_USER_PASSWORD);
    $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $db->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);
    return $db;
}

function GetById($table, $id)
{
    return GetSingleByCondition($table, array("Id" => $id));
}

function GetByGuid($modelName, $guid)
{
    return GetSingleByCondition($modelName . "s", array("Guid" => $guid));
}

function GetAllOrderedBy($table, $orderBy, $additionalSql = null)
{
    return GetAllByCondition($table, null, $orderBy, $additionalSql);
}

function GetAllByCondition($table, $condition, $orderBy = null, $additionalSql = null)
{
    if ($orderBy != null)
        $orderBy = " ORDER BY " . $orderBy;

    $model = GetModelByTable($table);

    $db = GetDatabaseConnection();
    $stmt = $db->prepare('SELECT * FROM ' . $table . ConstructConditionSQL($condition) . $orderBy . " " . $additionalSql);
    $stmt->execute($condition);

    $result = $stmt->fetchAll(PDO::FETCH_CLASS, $model);
    return $result;
}

function GetSingleByCondition($table, $condition, $orderBy = null)
{
    if ($orderBy != null)
        $orderBy = " ORDER BY " . $orderBy;

    $model = GetModelByTable($table);

    $db = GetDatabaseConnection();
    $stmt = $db->prepare('SELECT * FROM ' . $table . ConstructConditionSQL($condition) . $orderBy . " LIMIT 1");
    $stmt->execute($condition);

    $result = $stmt->fetchAll(PDO::FETCH_CLASS, $model);
    if (isset($result[0])) {
        return $result[0];
    } else
        return false;
}

function Insert($table, $obj)
{
    $db = GetDatabaseConnection();
    $excludedArray = array();
    $params = PrepareGenericArray($obj);
    $stmt = $db->prepare('INSERT INTO ' . $table . ' ' . ConstructMiddleSQL("insert", $params, $excludedArray));
    return $stmt->execute($params);
}

function InsertAll(array $obj)
{
    $res = true;
    foreach ($obj as $item) {
        $table = get_class($obj);
        $res &= Insert($table, $item);
    }
    return $res;
}

function Delete($table, $obj)
{
    $db = GetDatabaseConnection();
    $stmt = $db->prepare('DELETE FROM ' . $table . '  WHERE Id = :Id');
    $db->setAttribute(":Id", $obj->Id);
    return $stmt->execute();
}

function DeleteAll(array $obj)
{
    $res = true;
    foreach ($obj as $item) {
        $table = get_class($obj);
        $res &= Delete($table, $item);
    }
    return $res;
}

function DeleteById($table, $id)
{
    $db = GetDatabaseConnection();
    $stmt = $db->prepare('DELETE FROM ' . $table . ' WHERE Id = :Id');
    $stmt->bindParam(":Id", $id);
    return $stmt->execute();
}

function Update($table, $arr)
{
    $db = GetDatabaseConnection();
    $params = PrepareGenericArray($arr);
    $excludedArray = array();
    $excludedArray[] = "Id";
    $stmt = $db->prepare('UPDATE ' . $table . ' SET ' . ConstructMiddleSQL("update", $params, $excludedArray) . ' WHERE Id = :Id');
    return $stmt->execute($params);
}

function GetModelByTable($table)
{
    return substr($table, 0, -1);
}

function ConstructConditionSQL($params)
{
    if ($params == null || !is_array($params) || count($params) == 0)
        return "";

    $sql = " WHERE ";
    foreach ($params as $key => $val) {
        $sql .= $key . " = :" . $key . " AND ";
    }
    $sql = substr($sql, 0, -4);
    return $sql;
}

function ConstructMiddleSQL($mode, $params, $excluded)
{
    $sql = "";
    if ($mode == "update") {
        foreach ($params as $key => $val) {
            if (!in_array($key, $excluded))
                $sql .= $key . " = :" . $key . ", ";
        }
        $sql = substr($sql, 0, -2);
    } else if ($mode == "insert") {
        $part1 = "(";
        $part2 = "VALUES (";
        foreach ($params as $key => $val) {
            if (!in_array($key, $excluded)) {
                $part1 .= $key . ", ";
                $part2 .= ":" . $key . ", ";
            }
        }
        $part1 = substr($part1, 0, -2);
        $part2 = substr($part2, 0, -2);

        $part1 .= ") ";
        $part2 .= ")";
        $sql = $part1 . $part2;
    }
    return $sql;
}

function PrepareGenericArray($params)
{
    if (is_object($params)) {
        $properties = get_object_vars($params);
        $params = array();
        foreach ($properties as $key => $val) {
            if ($val != null && !is_object($val) && $key != "Id")
                $params[$key] = $val;
        }
    }
    return $params;
}


function ConvertToDateTime($input)
{
    if ($input == null || $input == "")
        return null;
    return date(DATETIME_FORMAT_DATABASE, strtotime($input));
}

function ConvertToPasswordHash($passwd)
{
    $options = [
        'cost' => 12,
    ];
    $hash = password_hash($passwd, PASSWORD_BCRYPT, $options);
    return $hash;
}