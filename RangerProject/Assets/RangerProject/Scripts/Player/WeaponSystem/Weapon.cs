using System;
using System.Collections;
using RangerProject.Scripts.Gameplay;
using RangerProject.Scripts.Manager;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData WeaponData;
        [SerializeField] protected Light MuzzleFlash;
        [SerializeField] protected LineRenderer BulletTrace;
        [SerializeField] protected Transform RightHandSocket;
        [SerializeField] private VisualEffect MuzzleFlashVFX;

        public Transform GetRightHandSocket() => RightHandSocket;
        public event Action<int> OnWeaponFired; 
        
        protected PlayerController PlayerController;
        protected int CurrentAmmo = 0;

        
        //To save a weapon later on we need to know which weapons we have by the weapon id and then we need to also save how many shots were left in the gun itself
        public int GetCurrentWeaponAmmo() => CurrentAmmo;
        public int SetCurrentAmmo(int NewAmmoAmount) => CurrentAmmo = NewAmmoAmount; 
        public WeaponData GetWeaponData() => WeaponData;
        
        public float GetWeaponShotDelay() => WeaponData.GetShotDelay();
        

        /// <summary>
        /// Inits a weapon for the player
        /// </summary>
        /// <param name="PlayerInventory"></param>
        public void InitWeapon(Inventory PlayerInventory)
        {
            if (PlayerInventory.TryRemoveAmmo(WeaponData.WeaponAmmoTypeToUse, WeaponData.GetMaxAmmo(),
                    out int RemovedAmmoAmount))
            { 
                CurrentAmmo = RemovedAmmoAmount;
            }
            else
            {
                CurrentAmmo = 0;
            }
            
            PlayerController = GetComponentInParent<PlayerController>();
            BulletTrace.positionCount = 0;
        }

        /// <summary>
        /// inits a weapon for a npc
        /// </summary>
        public void InitWeapon(int AmountOfAmmo)
        {
            //Just clamp it so that if we have more than our max ammo we just fully load the magazine
            CurrentAmmo = Mathf.Clamp(AmountOfAmmo, 0, WeaponData.GetMaxAmmo());
            PlayerController = GetComponentInParent<PlayerController>();
            BulletTrace.positionCount = 0;         
        }
        
        protected void InvokeOnWeaponFired()
        {
            if (CurrentAmmo >= WeaponData.GetAmmoUsedPerShot())
            {
                CurrentAmmo -= WeaponData.GetAmmoUsedPerShot();
                OnWeaponFired?.Invoke(CurrentAmmo);
            }
        }

        protected void SpawnDmgPopUp(Vector3 LocationToSpawnPopUpAt, int DealtDmg, Color DmgColor)
        {
            DynamicUIManager.Instance.SpawnDmgPopUp(DealtDmg.ToString(), DmgColor, LocationToSpawnPopUpAt);
        }
        protected void PlayMuzzleFlash(Vector3 EndPosition)
        {
            MuzzleFlashVFX.Play();
            SetBulletTracePositions(EndPosition);
            StartCoroutine(AnimMuzzleFlash());
        }

        protected void PlayWeaponShotSound()
        {
            AudioManager.Instance.PlayAudioFileWithRandomParams(WeaponData.GetWeaponShotSound(), transform.position);
        }

        protected void SetBulletTracePositions(Vector3 EndPosition)
        {
            BulletTrace.positionCount = 2;
            BulletTrace.SetPosition(0, BulletTrace.transform.position);
            BulletTrace.SetPosition(1, EndPosition);
        }
        
        private IEnumerator AnimMuzzleFlash()
        {
            float FlashTime = 0.02f;
            float T = 0.0f;
            
            while (FlashTime > T)
            {
                T += Time.deltaTime;
                MuzzleFlash.intensity = Mathf.Lerp(0, WeaponData.GetMuzzleFlashIntensity(), T / FlashTime);
                yield return null;
            }

            MuzzleFlash.intensity = 0;
            BulletTrace.positionCount = 0;
            MuzzleFlashVFX.Stop();
        }
        public abstract void Shoot(CameraController CameraController);
    }
}
