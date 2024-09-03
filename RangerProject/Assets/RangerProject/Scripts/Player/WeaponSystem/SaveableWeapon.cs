using UnityEditor;
using UnityEngine;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    [System.Serializable]
    public struct SaveableWeapon
    {
        [SerializeField] private GUID WeaponID;
        [SerializeField] private int CurrentAmmo;

        public SaveableWeapon(GUID WeaponID, int CurrentAmmo)
        {
            this.WeaponID = WeaponID;
            this.CurrentAmmo = CurrentAmmo;
        }
    }
}
