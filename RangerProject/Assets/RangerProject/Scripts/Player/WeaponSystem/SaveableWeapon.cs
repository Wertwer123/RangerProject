using UnityEditor;
using UnityEngine;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    [System.Serializable]
    public class SaveableWeapon
    {
        [SerializeField] private GUID WeaponID;
        [SerializeField] private int CurrentAmmo;

        public GUID GetWeaponID() => WeaponID;
        public int GetCurrentAmmo() => CurrentAmmo;
        public void SetAmmo(int NewCurrentAmmo) => CurrentAmmo = NewCurrentAmmo;
        
        public SaveableWeapon(GUID WeaponID, int CurrentAmmo)
        {
            this.WeaponID = WeaponID;
            this.CurrentAmmo = CurrentAmmo;
        }
    }
}
