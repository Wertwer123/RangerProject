using System.Collections.Generic;
using RangerProject.Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    [CreateAssetMenu(menuName = "Weapons/NewWeaponDatabase", fileName = "NewWeaponDatabase")]
    public class WeaponDataBase : ScriptableObject
    {
        [SerializeField] private List<Weapon> AllAvailableWeapons;

        public void ReassignWeaponIDs()
        {
            foreach (Weapon Weapon in AllAvailableWeapons)
            {
                Weapon.GetWeaponData().SetWeaponId();
            }
        }
        public Weapon GetWeaponById(GUID WeaponIDToFind)
        {
            foreach (var Weapon in AllAvailableWeapons)
            {
                GUID WeaponId = Weapon.GetWeaponData().GetWeaponId();

                if (WeaponIDToFind == WeaponId)
                {
                    return Weapon;
                }
            }

            return null;
        }
    }
}
