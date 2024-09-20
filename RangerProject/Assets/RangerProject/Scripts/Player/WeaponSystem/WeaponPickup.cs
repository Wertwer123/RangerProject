using UnityEngine;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    public class WeaponPickup : MonoBehaviour, IPickup
    {
        [SerializeField] private WeaponData WeaponData;
        
        public void OnPickUp(GameObject ObjectPickingUp)
        {
            //Are we in general able to pick up a weapon
            if (ObjectPickingUp.TryGetComponent(out WeaponComponent WeaponComponent))
            {
                //If we are the player object meaning we have an inventory
                if (ObjectPickingUp.TryGetComponent(out Inventory PlayerInventory))
                {
                    PlayerInventory.AddWeaponToInventory(WeaponData);

                    if (!WeaponComponent.HasWeaponEquiped())
                    {
                        WeaponComponent.SetCurrentWeapon(WeaponData);
                    }
                }
            }
            
            Destroy(gameObject);
        }
    }
}