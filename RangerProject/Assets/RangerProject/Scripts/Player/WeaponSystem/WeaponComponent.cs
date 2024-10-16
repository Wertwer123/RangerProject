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
        [SerializeField] private Vector3 WeaponAttachmentSpotRotationOffset;
        [SerializeField] private Inventory PlayerInventory;
        [SerializeField] private Transform WeaponAttachmentSpot;
        [SerializeField] private Transform WeaponAttachmentSpotParent;
        [SerializeField] private Weapon CurrentWeapon;
        [SerializeField] private WeaponDataBase WeaponDataBase;
        [SerializeField] protected CameraController CameraController;
        [SerializeField] private TwoBoneIKConstraint RightHandAimConstraint;
        [SerializeField] private RigBuilder RigBuilder;
        public event Action<Weapon, Weapon> OnWeaponChanged; 
        public event Action<int> OnCurrentWeaponReloaded; 
        
        private bool IsFiring;
        private int IsFiringId = Animator.StringToHash("IsFiring");
        private float NextShotTime = 0.0f;
        private Animator PlayerAnimator;
       
        
        public bool HasWeaponEquiped() => CurrentWeapon != null;
        
        private void Start()
        {
            PlayerAnimator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            SetPositionOfWeaponAttachmentSpot();
            
            if (!HasWeaponEquiped())
            {
                return;
            }
            
            if (IsFiring && NextShotTime <= Time.time)
            {
                if (!WeaponHasEnoughAmmoToFire())
                {
                    IsFiring = false;
                    PlayerAnimator.SetBool(IsFiringId, IsFiring);
                }
                
                NextShotTime = Time.time + CurrentWeapon.GetWeaponShotDelay();
                CurrentWeapon.Shoot(CameraController);
            } 
            
        }
        
        public void SetCurrentWeapon(WeaponData NewWeapon)
        {
            ChangeWeapon(WeaponDataBase.GetWeaponById(NewWeapon.GetWeaponId()));
        }

        public void OnMouseWheelUp(InputAction.CallbackContext CallbackContext)
        {
            if (!CallbackContext.started)
            {
                return;
            }
            
            PlayerInventory.ChangeWeaponUpOrDown(1);
            ChangeWeapon(WeaponDataBase.GetWeaponById(PlayerInventory.GetCurrentlyEquippedWeaponID()));
        }

        public void OnMouseWheelDown(InputAction.CallbackContext CallbackContext)
        {
            if (!CallbackContext.started)
            {
                return;
            }
            
            PlayerInventory.ChangeWeaponUpOrDown(-1);
            ChangeWeapon(WeaponDataBase.GetWeaponById(PlayerInventory.GetCurrentlyEquippedWeaponID()));
        }

        public void ReloadCurrentWeapon(InputAction.CallbackContext CallbackContext)
        {
            if (!HasWeaponEquiped())
            {
                return;
            }
            
            if (CallbackContext.canceled)
            {
                //Only remove as much ammo as needed to fill up the magazine
                if (PlayerInventory.TryRemoveAmmo(CurrentWeapon.GetWeaponData().GetWeaponAmmoType(),
                        CurrentWeapon.GetWeaponData().GetMaxAmmo() - CurrentWeapon.GetCurrentWeaponAmmo(), out int RemovedAmmoAmount))
                {
                    //add the ammo to the current weapon ammo if the current ammo is 0 we should completly fill up if its for example 12 and max size 15 we use three bullets and add them
                    //to our current ammo
                    CurrentWeapon.SetCurrentAmmo(CurrentWeapon.GetCurrentWeaponAmmo() + RemovedAmmoAmount);
                    SetAmmoForInventoryWeapon(CurrentWeapon.GetCurrentWeaponAmmo());
                    
                    OnCurrentWeaponReloaded?.Invoke(CurrentWeapon.GetCurrentWeaponAmmo());
                }
            }
        }

        public bool WeaponHasEnoughAmmoToFire()
        {
            return CurrentWeapon.GetCurrentWeaponAmmo() >= CurrentWeapon.GetWeaponData().GetAmmoUsedPerShot();
        }
        public void OnFire(InputAction.CallbackContext CallbackContext)
        {
            if (HasWeaponEquiped() && CallbackContext.started)
            {
                if (!WeaponHasEnoughAmmoToFire())
                {
                    Debug.Log("Not enough ammo");
                    return;
                }
                
                IsFiring = true;
                PlayerAnimator.SetBool(IsFiringId, IsFiring);
                
            }
            else if(CallbackContext.canceled)
            {
                IsFiring = false;
                PlayerAnimator.SetBool(IsFiringId, IsFiring);
            }
        }

        private void SetPositionOfWeaponAttachmentSpot()
        {
            WeaponAttachmentSpot.position = WeaponAttachmentSpotParent.position;
            WeaponAttachmentSpot.eulerAngles = WeaponAttachmentSpotParent.eulerAngles + WeaponAttachmentSpotRotationOffset;
        }
        private void SetAmmoForInventoryWeapon(int NewAmmoAmount)
        {
            PlayerInventory.SetAmmoForWeapon(CurrentWeapon.GetWeaponData().GetWeaponId(), NewAmmoAmount);
        }
        private void ChangeWeapon(Weapon NewWeapon)
        {
            Weapon OldWeapon = CurrentWeapon;
            
            if (CurrentWeapon)
            {
                //If we change to the same weapon do nothing
                if (CurrentWeapon.GetWeaponData().GetWeaponId() == NewWeapon.GetWeaponData().GetWeaponId())
                {
                    Debug.Log("SameWeapon");
                    return;
                }
                
                CurrentWeapon.OnWeaponFired -= SetAmmoForInventoryWeapon;
                Destroy(CurrentWeapon.gameObject);
            }
            
            var NewWeaponInstance = Instantiate(NewWeapon, WeaponAttachmentSpot);
            NewWeaponInstance.transform.localPosition += new Vector3(WeaponAttachmentOffset, 0, 0);
            
            Debug.Log(OldWeapon);
            CurrentWeapon = NewWeaponInstance;
            
            //Initialize the weapon
            CurrentWeapon.OnWeaponFired += SetAmmoForInventoryWeapon;
            CurrentWeapon.InitWeapon(PlayerInventory);
            CameraController.SetCurrentCameraSettings(NewWeaponInstance.GetWeaponData().GetWeaponShakeSettings());
            
            //Call the change event to notify other systems that a weapon has been changed
            OnWeaponChanged?.Invoke(OldWeapon, CurrentWeapon);
            
            //Faulty because the arm gets stretched and fucks up the whole rig
            // AttachRightHandToWeaponSocket(CurrentWeapon);
        }

        private void AttachRightHandToWeaponSocket(Weapon NewWeapon)
        {
            PlayerAnimator.enabled = false;
            
            RightHandAimConstraint.data.target = NewWeapon.GetRightHandSocket();
            RigBuilder.Build();

            PlayerAnimator.enabled = true;
        }
    }
}
