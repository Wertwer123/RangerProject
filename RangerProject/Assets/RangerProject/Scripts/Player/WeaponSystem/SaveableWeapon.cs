using UnityEditor;
using UnityEngine;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    [System.Serializable]
    public class SaveableWeapon
    {
        [SerializeField] private int WeaponID;
        [SerializeField] private int CurrentAmmo;

        public int GetWeaponID() => WeaponID;
        public int GetCurrentAmmo() => CurrentAmmo;
        public void SetAmmo(int NewCurrentAmmo) => CurrentAmmo = NewCurrentAmmo;
        
        public SaveableWeapon(int WeaponID, int CurrentAmmo)
        {
            this.WeaponID = WeaponID;
            this.CurrentAmmo = CurrentAmmo;
        }
    }
}
