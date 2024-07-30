using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    public class WeaponComponent : MonoBehaviour
    {
        [SerializeField] private Weapon CurrentWeapon;
        [SerializeField] private MeshFilter WeaponMeshFilter;

        [Header("Test")] 
        [SerializeField] private WeaponData TestWeaponData;

        private float NextShotTime = 0.0f;
        private bool IsFiring;

        //Delete later
        private void Start()
        {
            ChangeWeapon(TestWeaponData);
        }

        private void Update()
        {
            if (IsFiring && NextShotTime <= Time.time)
            {
                NextShotTime = Time.time + CurrentWeapon.GetWeaponShotDelay();
                CurrentWeapon.Shoot();
            }    
        }
        
        void ChangeWeapon(WeaponData NewWeaponData)
        {
            Destroy(CurrentWeapon);

            switch (NewWeaponData.GetWeaponType())
            {
                case EWeaponType.SingleShot:
                {
                    CurrentWeapon = gameObject.AddComponent<SingleShotRifle>();
                    CurrentWeapon.InitWeapon(NewWeaponData, WeaponMeshFilter);
                    break;
                }
                case EWeaponType.ShotGun:
                {
                    break;
                }
                case EWeaponType.AutomaticRifle:
                {
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
                
            }
        }
        
        public void OnFire(InputAction.CallbackContext CallbackContext)
        {
            if (CallbackContext.started)
            {
                IsFiring = true;
            }
            else if(CallbackContext.canceled)
            {
                IsFiring = false;
            }
        }
    }
}
