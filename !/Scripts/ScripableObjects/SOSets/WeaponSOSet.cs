using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOSet/Weapons")]
public class WeaponSOSet : ScriptableObject
{
    public List<WeaponSO> weaponsSOList = new();

    public WeaponSO FindByName(string name)
    {
        foreach (var weapon in weaponsSOList)
        {
            if (weapon.name == name)
                return weapon;
        }

        return null;
    }
}



