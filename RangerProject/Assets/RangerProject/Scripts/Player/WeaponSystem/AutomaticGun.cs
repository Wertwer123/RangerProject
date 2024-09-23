using RangerProject.Scripts.Gameplay;
using RangerProject.Scripts.Interfaces;
using UnityEngine;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    /// <summary>
    /// Can be everything except a shotgun or a rocket launcher grenade launcher etc which needs special implementation but
    /// can be a pistol and any machine gun etc
    /// </summary>
    public class AutomaticGun : Weapon
    {
        public override void Shoot(CameraController CameraController)
        {
            if (CurrentAmmo == 0 || CurrentAmmo < WeaponData.GetAmmoUsedPerShot())
            {
                //Also play a click sound to make the player recognize that he should reload the weapon
                Debug.Log("Out Of Ammunition");
                return;
            }
            
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
                    Damageable.DealDmg(WeaponData.GetWeaponDmg(), out int RecievedDmg);
                    SpawnDmgPopUp(Hit.point + Vector3.up *2.2f, RecievedDmg, Color.red);
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
