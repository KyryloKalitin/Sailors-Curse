using UnityEngine;

public static class SOSetLoader
{
    private static string _gameEventSetPath = "Data/GameEventSOSet";
    private static string _weaponSOSetPath = "Data/WeaponSOSet";
    private static string _rareItemSOSetPath = "Data/RareItemSOSet";
    private static string _shipInventoryLevelSOSetPath = "Data/ShipInventoryLevelSOSet";
    private static string _losingReasonSOSetPath = "Data/LosingReasonSOSet";

    public static GameEventSOSet LoadGameEventSOSet()
    {
        return Resources.Load<GameEventSOSet>(_gameEventSetPath);
    }

    public static WeaponSOSet LoadWeaponSOSet()
    {
        return Resources.Load<WeaponSOSet>(_weaponSOSetPath);
    }

    public static RareItemSOSet LoadRareItemSOSet()
    {
        return Resources.Load<RareItemSOSet>(_rareItemSOSetPath);
    }

    public static ShipInventoryLevelSOSet LoadShipInventoryLevelSOSet()
    {
        return Resources.Load<ShipInventoryLevelSOSet>(_shipInventoryLevelSOSetPath);
    }

    public static LosingReasonSOSet LoadLosingReasonSOSet()
    {
        return Resources.Load<LosingReasonSOSet>(_losingReasonSOSetPath);
    }
}



