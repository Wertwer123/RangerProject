using RangerProject.Scripts.Gameplay;
using RangerProject.Scripts.Interfaces;
using UnityEngine;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    public class SingleShotRifle : Weapon
    {
        public override void Shoot(CameraController CameraController)
        {
            if (CurrentAmmo == 0)
            {
                Debug.Log("Out Of Ammo");
                return;
            }
            
            CurrentAmmo--;
            InvokeOnWeaponFired();
            PlayMuzzleFlash();
            
            var WeaponTransform = transform;
            Vector3 StartPoint = WeaponTransform.position;
            Vector3 Direction = WeaponTransform.forward;
           
            CameraController.PlayCameraShakeDirectional(WeaponData.GetWeaponShakeSettings(), -Direction);
            
            bool bHitTarget = Physics.Raycast(StartPoint, Direction, out RaycastHit Hit, WeaponData.GetWeaponRange(), WeaponData.GetHitableLayer());

            if (bHitTarget)
            {
                //Deal dmg etc
                
                if (Hit.collider.gameObject.TryGetComponent(out IDamageable Damageable))
                {
                    Damageable.DealDmg(WeaponData.GetWeaponDmg());
                    Debug.DrawRay(StartPoint, Direction * Hit.distance, Color.yellow, 5.0f);
                }
                else
                {
                    Vector3 Temp = Hit.point;
                    Temp.y = WeaponTransform.position.y;
                    Debug.DrawRay(Temp, Hit.point, Color.yellow, 5.0f);
                }
            }
        }
    }
}
