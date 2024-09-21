using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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
                byte[] HashBuffer = Encoding.UTF8.GetBytes(Weapon.GetWeaponData().GetWeaponName());
                SHA256 WeaponId = SHA256.Create();
               
                byte[] HashBytes = WeaponId.ComputeHash(HashBuffer);
                
                Weapon.GetWeaponData().SetWeaponId(BitConverter.ToInt32(HashBytes));
            }
        }
        public Weapon GetWeaponById(int WeaponIDToFind)
        {
            foreach (var Weapon in AllAvailableWeapons)
            {
                int WeaponId = Weapon.GetWeaponData().GetWeaponId();

                if (WeaponIDToFind == WeaponId)
                {
                    return Weapon;
                }
            }

            return null;
        }
    }
}
