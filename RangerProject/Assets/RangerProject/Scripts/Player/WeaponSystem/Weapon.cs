using UnityEngine;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponData WeaponDataToUse;
        [SerializeField] private MeshFilter WeaponDynamicMesh;

        public float GetWeaponShotDelay() => WeaponDataToUse.GetShotDelay();

        public void InitWeapon(WeaponData WeaponData, MeshFilter WeaponMeshFilter)
        {
            WeaponDataToUse = WeaponData;
            WeaponDynamicMesh = WeaponMeshFilter;
            WeaponDynamicMesh.sharedMesh = WeaponData.GetWeaponMesh();
        }

        public void Shoot()
        {
            Debug.Log("Shot Current Weapon" + WeaponDataToUse.GetWeaponName());
        }
    }
}
