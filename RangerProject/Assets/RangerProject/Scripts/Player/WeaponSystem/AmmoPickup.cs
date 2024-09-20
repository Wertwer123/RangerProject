using UnityEngine;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    public class AmmoPickup : MonoBehaviour, IPickup
    {
        [SerializeField] private EAmmoType AmmoTypeToAdd;
        [SerializeField, Min(1)] private int AmountOfAmmoToAdd;

        public void OnPickUp(GameObject ObjectPickingUp)
        {
            if (ObjectPickingUp.TryGetComponent(out Inventory PlayerInventory))
            {
                PlayerInventory.AddAmmo(AmmoTypeToAdd, AmountOfAmmoToAdd);
            }
        }
    }
}
