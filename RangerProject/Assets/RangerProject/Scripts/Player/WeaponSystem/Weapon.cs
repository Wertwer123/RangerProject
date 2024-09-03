using System;
using System.Collections;
using RangerProject.Scripts.Gameplay;
using UnityEngine;
using UnityEngine.Serialization;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData WeaponData;
        [SerializeField] protected Light MuzzleFlash;

        public event Action<int> OnWeaponFired; 
        
        protected PlayerController PlayerController;
        protected int CurrentAmmo = 0;

        //To save a weapon later on we need to know which weapons we have by the weapon id and then we need to also save how many shots were left in the gun itself
        public WeaponData GetWeaponData() => WeaponData;
        
        public float GetWeaponShotDelay() => WeaponData.GetShotDelay();

        private void Start()
        {
            InitWeapon();
        }

        public void InitWeapon()
        {
            CurrentAmmo = WeaponData.GetMaxAmmo();
            PlayerController = GetComponentInParent<PlayerController>();
        }

        protected void InvokeOnWeaponFired()
        {
            OnWeaponFired?.Invoke(CurrentAmmo);
        }

        protected void PlayMuzzleFlash()
        {
            StartCoroutine(AnimMuzzleFlash());
        }
        
        private IEnumerator AnimMuzzleFlash()
        {
            float FlashTime = WeaponData.GetShotDelay() * 0.5f;
            float T = 0.0f;
            
            while (FlashTime > T)
            {
                T += Time.deltaTime;
                MuzzleFlash.intensity = Mathf.Lerp(0, WeaponData.GetMuzzleFlashIntensity(), T / FlashTime);
                yield return null;
            }

            MuzzleFlash.intensity = 0;
        }
        public abstract void Shoot(CameraController CameraController);
    }
}
