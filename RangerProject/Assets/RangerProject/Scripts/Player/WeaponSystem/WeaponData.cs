using RangerProject.Scripts.Gameplay;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    [CreateAssetMenu(menuName = "Weapons/NewWeaponData", fileName = "NewWeapon")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField, Min(1)] private int Dmg = 10;
        [SerializeField, Min(0)] private int ShotsPerSecond = 3;
        [SerializeField] private int MaxAmmo = 20;
        [SerializeField] private float WeaponRange = 20;
        [SerializeField, Min(1)] private float MuzzleFlashIntensity = 400;
        [SerializeField] private string WeaponName = "DefaultWeaponName";
        [SerializeField] private GUID WeaponId = new GUID();
        [SerializeField] private LayerMask HitableLayer;
        [SerializeField] private EWeaponType WeaponType;
        [SerializeField] private CameraSettings WeaponSettings;

        public int GetWeaponDmg() => Dmg;
        public int GetMaxAmmo() => MaxAmmo;
        public float GetWeaponRange() => WeaponRange;
        public float GetShotDelay() => 1.0f / ShotsPerSecond;
        public float GetMuzzleFlashIntensity() => MuzzleFlashIntensity;
        public string GetWeaponName() => WeaponName;
        public GUID GetWeaponId() => WeaponId;
        public LayerMask GetHitableLayer() => HitableLayer;
        public EWeaponType GetWeaponType() => WeaponType;
        public CameraSettings GetWeaponShakeSettings() => WeaponSettings;
    }
}
