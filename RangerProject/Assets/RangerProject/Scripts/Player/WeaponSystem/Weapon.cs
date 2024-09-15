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
            BulletTrace.positionCount = 0;
        }

        protected void InvokeOnWeaponFired()
        {
            OnWeaponFired?.Invoke(CurrentAmmo);
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
