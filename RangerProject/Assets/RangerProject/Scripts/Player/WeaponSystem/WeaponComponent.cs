using System;
using RangerProject.Scripts.Gameplay;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    public class WeaponComponent : MonoBehaviour
    {
        [SerializeField] private float WeaponAttachmentOffset = 0.05f;
        [SerializeField] private Transform WeaponAttachmentParent;
        [SerializeField] private Weapon CurrentWeapon;
        [SerializeField] private WeaponDataBase WeaponDataBase;
        [SerializeField] protected CameraController CameraController;
        [SerializeField] private TwoBoneIKConstraint RightHandAimConstraint;
        [SerializeField] private RigBuilder RigBuilder;
        
        [Header("Test")] 
        [SerializeField] private WeaponData TestWeapon;

        private Animator PlayerAnimator;

        private float NextShotTime = 0.0f;
        private bool IsFiring;

        private int IsFiringId = Animator.StringToHash("IsFiring");

        //Delete later
        private void Start()
        {
            PlayerAnimator = GetComponentInChildren<Animator>();
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
            NewWeaponInstance.transform.localPosition += new Vector3(WeaponAttachmentOffset, 0, 0);
            CurrentWeapon = NewWeaponInstance;
            CameraController.SetCurrentCameraSettings(NewWeaponInstance.GetWeaponData().GetWeaponShakeSettings());
            
            AttachRightHandToWeaponSocket(CurrentWeapon);
        }

        void AttachRightHandToWeaponSocket(Weapon NewWeapon)
        {
            PlayerAnimator.enabled = false;
            RightHandAimConstraint.data.target = NewWeapon.GetRightHandSocket();
            RigBuilder.Build();

            PlayerAnimator.enabled = true;
        }
        
        public void OnFire(InputAction.CallbackContext CallbackContext)
        {
            if (CallbackContext.started)
            {
                IsFiring = true;
                PlayerAnimator.SetBool(IsFiringId, IsFiring);
            }
            else if(CallbackContext.canceled)
            {
                IsFiring = false;
                PlayerAnimator.SetBool(IsFiringId, IsFiring);
            }
        }
    }
}
