using System;
using RangerProject.Scripts.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    public class WeaponComponent : MonoBehaviour
    {
        [SerializeField] private Transform WeaponAttachmentParent;
        [SerializeField] private Weapon CurrentWeapon;
        [SerializeField] private WeaponDataBase WeaponDataBase;
        [SerializeField] protected CameraController CameraController;
        
        [Header("Test")] 
        [SerializeField] private WeaponData TestWeapon;

        private float NextShotTime = 0.0f;
        private bool IsFiring;

        //Delete later
        private void Start()
        {
            ChangeWeapon(WeaponDataBase.GetWeaponById(TestWeapon.GetWeaponId()));
        }

        private void Update()
        {
            if (IsFiring && NextShotTime <= Time.time)
            {
                NextShotTime = Time.time + CurrentWeapon.GetWeaponShotDelay();
                CurrentWeapon.Shoot(CameraController);
            }    
        }
        
        void ChangeWeapon(Weapon NewWeapon)
        {
            if (CurrentWeapon)
            {
                Destroy(CurrentWeapon);
            }
            
            var NewWeaponInstance = Instantiate(NewWeapon, WeaponAttachmentParent);
            NewWeaponInstance.transform.position = WeaponAttachmentParent.position;
            CurrentWeapon = NewWeaponInstance;
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
