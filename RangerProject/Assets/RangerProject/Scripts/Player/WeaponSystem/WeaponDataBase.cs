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

        public Weapon GetWeaponById(SerializableGUID WeaponIDToFind)
        {
            foreach (var Weapon in AllAvailableWeapons)
            {
                SerializableGUID WeaponId = Weapon.GetWeaponData().GetWeaponId();

                if (WeaponIDToFind.Equals(WeaponId))
                {
                    return Weapon;
                }
            }

            return null;
        }
    }
}
