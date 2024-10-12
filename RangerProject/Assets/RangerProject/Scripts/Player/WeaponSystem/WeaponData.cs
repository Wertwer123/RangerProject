using System;
using RangerProject.Scripts.Gameplay;
using RangerProject.Scripts.Utils;
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
        [SerializeField, Min(1)] private int AmmoUsedPerShot = 1;
        [SerializeField] private int MaxAmmo = 20;
        [SerializeField] private float WeaponRange = 20;
        [SerializeField, Min(1)] private float MuzzleFlashIntensity = 400;
        [SerializeField] private string WeaponName = "DefaultWeaponName";
        [SerializeField] public EAmmoType WeaponAmmoTypeToUse = EAmmoType.Rifle7_76mm;
        [SerializeField] private ParamterizedAudiofile ShotSound;
        [SerializeField] Vector2 WeaponSpray = Vector2.zero;
        [SerializeField] private LayerMask HitableLayer;
        [SerializeField] private int WeaponId = -1;
        [SerializeField] private EWeaponType WeaponType;
        [SerializeField] private CameraSettings WeaponSettings;
        
        public int GetWeaponDmg() => Dmg;
        public int GetMaxAmmo() => MaxAmmo;
        public float GetWeaponRange() => WeaponRange;
        public float GetShotDelay() => 1.0f / ShotsPerSecond;
        public int GetAmmoUsedPerShot() => AmmoUsedPerShot;
        public float GetMuzzleFlashIntensity() => MuzzleFlashIntensity;
        public string GetWeaponName() => WeaponName;
        public EAmmoType GetWeaponAmmoType() => WeaponAmmoTypeToUse;
        public ParamterizedAudiofile GetWeaponShotSound() => ShotSound;
        public Vector2 GetWeaponSpray() => WeaponSpray;
        public int GetWeaponId() => WeaponId;

        public void SetWeaponId(int Id)
        {
            if (WeaponId == -1)
            {
                Debug.Log("Assigned a id");
                WeaponId = Id;
                
                #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
                #endif
            }
            else
            {
                Debug.Log(WeaponId);
            }
        }
        public LayerMask GetHitableLayer() => HitableLayer;
        public EWeaponType GetWeaponType() => WeaponType;
        public CameraSettings GetWeaponShakeSettings() => WeaponSettings;
    }
}
