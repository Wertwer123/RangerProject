using RangerProject.Scripts.Player.WeaponSystem;
using TMPro;
using UnityEngine;

public class PlayerHud : BaseSingleton<PlayerHud>
{
   [SerializeField] private WeaponComponent PlayerWeaponComponent;
   [SerializeField] private Canvas RootCanvas;
   [SerializeField] private TMP_Text MaxAmmoText;
   [SerializeField] private TMP_Text CurrentAmmoText;
   [SerializeField] private TMP_Text WeaponName;

   private void Start()
   {
      PlayerWeaponComponent.OnWeaponChanged += InitWeaponUI;
      PlayerWeaponComponent.OnCurrentWeaponReloaded += UpdateCurrentAmmoWeaponText;
   }

   private void OnDisable()
   {
      PlayerWeaponComponent.OnWeaponChanged -= InitWeaponUI;
      PlayerWeaponComponent.OnCurrentWeaponReloaded -= UpdateCurrentAmmoWeaponText;
   }

   void InitWeaponUI(Weapon OldWeapon, Weapon NewWeapon)
   {
      if (OldWeapon)
      {
         OldWeapon.OnWeaponFired -= UpdateCurrentAmmoWeaponText;
      }
      else

      {
         RootCanvas.gameObject.SetActive(true);
      }
      
      MaxAmmoText.text = NewWeapon.GetWeaponData().GetMaxAmmo().ToString();
      CurrentAmmoText.text = NewWeapon.GetCurrentWeaponAmmo().ToString();
      WeaponName.text = NewWeapon.GetWeaponData().GetWeaponName();

      NewWeapon.OnWeaponFired += UpdateCurrentAmmoWeaponText;
   }

   void UpdateCurrentAmmoWeaponText(int CurrentAmmo)
   {
      CurrentAmmoText.text = CurrentAmmo.ToString();
   }
}
