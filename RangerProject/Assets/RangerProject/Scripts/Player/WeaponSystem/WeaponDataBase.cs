using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    [CreateAssetMenu(menuName = "Weapons/NewWeaponDatabase", fileName = "NewWeaponDatabase")]
    public class WeaponDataBase : ScriptableObject
    {
        [SerializeField] private List<Weapon> AllAvailableWeapons;

        public Weapon GetWeaponById(GUID WeaponIDToFind)
        {
            foreach (var Weapon in AllAvailableWeapons)
            {
                GUID WeaponId = Weapon.GetWeaponData().GetWeaponId();

                if (WeaponIDToFind.Equals(WeaponId))
                {
                    return Weapon;
                }
            }

            return null;
        }
    }
}
