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
                Debug.Log("Out Of Ammunition");
                return;
            }
            
            CurrentAmmo--;
            InvokeOnWeaponFired();
            PlayWeaponShotSound();
            
            
            var WeaponTransform = transform;
            Vector3 WeaponSpray = new Vector3(Random.Range(-WeaponData.GetWeaponSpray().x, WeaponData.GetWeaponSpray().x), Random.Range(-WeaponData.GetWeaponSpray().y, WeaponData.GetWeaponSpray().y), 0);
            Vector3 StartPoint = WeaponTransform.position;
            Vector3 Direction = WeaponTransform.forward + WeaponSpray;
           
            CameraController.PlayCameraShakeDirectional(WeaponData.GetWeaponShakeSettings(), -Direction);
            
            bool bHitTarget = Physics.Raycast(StartPoint, Direction, out RaycastHit Hit, WeaponData.GetWeaponRange(), WeaponData.GetHitableLayer());

            if (bHitTarget)
            {
                //Deal dmg etc
                PlayMuzzleFlash(Hit.point);
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
            else
            {
                PlayMuzzleFlash(StartPoint + Direction * WeaponData.GetWeaponRange());
            }
        }
    }
}
