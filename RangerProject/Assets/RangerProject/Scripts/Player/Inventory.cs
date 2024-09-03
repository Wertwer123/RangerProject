using System.Collections.Generic;
using RangerProject.Scripts.Player.WeaponSystem;
using UnityEngine;

namespace RangerProject.Scripts.Player
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<SaveableWeapon> AllCollectedWeapons;

        public void AddWeaponToInventory(WeaponData WeaponData)
        {
            // SaveableWeapon SaveableWeapon = new SaveableWeapon();
        }
    }
}
