using System;
using System.Collections.Generic;
using RangerProject.Scripts.Player.WeaponSystem;
using UnityEditor;
using UnityEngine;

namespace RangerProject.Scripts.Player
{
    /// <summary>
    /// Holds all player items we assume that the player can only have one weapon of each type because why should he have more
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<SaveableWeapon> AllCollectedWeapons;
        
        private Dictionary<EAmmoType, int> AmmoAmountPerAmmoType = new();

        private int CurrentlyEquippedWeaponIndex = -1;

        public void SetEquippedWeaponIndex(int WeaponId)
        {
            for (int i = 0; i < AllCollectedWeapons.Count; i++)
            {
                SaveableWeapon CollectedWeapon = AllCollectedWeapons[i];

                if (CollectedWeapon.GetWeaponID() == WeaponId)
                {
                    CurrentlyEquippedWeaponIndex = i;
                    return;
                }
            }
        }

        public void ChangeWeaponUpOrDown(int Increment)
        {
            //If for some reason we pass in something bigger or smaller then 1
            Increment = Mathf.Clamp(Increment, -1, 1);

            CurrentlyEquippedWeaponIndex += Increment;

            if (CurrentlyEquippedWeaponIndex < 0)
            {
                CurrentlyEquippedWeaponIndex = AllCollectedWeapons.Count - 1;
            }
            else if (CurrentlyEquippedWeaponIndex > AllCollectedWeapons.Count - 1)
            {
                CurrentlyEquippedWeaponIndex = 0;
            }
        }

        public int GetCurrentlyEquippedWeaponID()
        {
            return AllCollectedWeapons[CurrentlyEquippedWeaponIndex].GetWeaponID();
        }
        public void AddAmmo(EAmmoType TypeOfAmmoToAdd, int AmountOfAmmoToAdd)
        {
            if (AmmoAmountPerAmmoType.ContainsKey(TypeOfAmmoToAdd))
            {
                AmmoAmountPerAmmoType[TypeOfAmmoToAdd] += AmountOfAmmoToAdd;
                Debug.Log(TypeOfAmmoToAdd + " added times " + AmountOfAmmoToAdd);
                Debug.Log(" Current amount =  " + AmmoAmountPerAmmoType[TypeOfAmmoToAdd]);
            }
            else
            {
                AmmoAmountPerAmmoType.Add(TypeOfAmmoToAdd, AmountOfAmmoToAdd);
                Debug.Log(TypeOfAmmoToAdd.ToString() + "added times " + AmountOfAmmoToAdd);
                Debug.Log(" Current amount =  " + AmmoAmountPerAmmoType[TypeOfAmmoToAdd]);
            }
        }
        public void AddWeaponToInventory(WeaponData WeaponData)
        {
            EAmmoType TypeOfAmmoWeaponUses = WeaponData.GetWeaponAmmoType();
            int AmountOfAmmoToFillIntoWeapon = AmmoAmountPerAmmoType.TryGetValue(TypeOfAmmoWeaponUses, out var value) ? value : 0;
            
            SaveableWeapon NewWeapon = new SaveableWeapon(WeaponData.GetWeaponId(), AmountOfAmmoToFillIntoWeapon);
            
            AllCollectedWeapons.Add(NewWeapon);
        }
        
        public void SetAmmoForWeapon(int WeaponId, int NewAmmoAmount)
        {
            foreach (SaveableWeapon CollectedWeapon in AllCollectedWeapons)
            {
                if (CollectedWeapon.GetWeaponID() == WeaponId)
                {
                    CollectedWeapon.SetAmmo(NewAmmoAmount);
                    return;
                }
            }
            
            Debug.Log("Didnt find the weapon in the inventory");
        }
        
        /// <summary>
        /// Returns whether you where able to remove the ammo amount or not also returns the amount of removed ammunition so that you can assign that amount as the current ammo to
        /// a weapon that you want to reload
        /// </summary>
        /// <param name="TypeOfAmmoToRemove"></param>
        /// <param name="AmmoAmountToRemove"></param>
        /// <param name="RemovedAmmoAmount"></param>
        /// <returns></returns>
        public bool TryRemoveAmmo(EAmmoType TypeOfAmmoToRemove, int AmmoAmountToRemove, out int RemovedAmmoAmount)
        {
            if (AmmoAmountPerAmmoType.ContainsKey(TypeOfAmmoToRemove))
            {
                int AmmoAmountOfType = AmmoAmountPerAmmoType[TypeOfAmmoToRemove];

                if (AmmoAmountOfType >= AmmoAmountToRemove)
                {
                    AmmoAmountPerAmmoType[TypeOfAmmoToRemove] -= AmmoAmountToRemove;
                    RemovedAmmoAmount = AmmoAmountToRemove;
                    return true;
                }
                else if (AmmoAmountOfType < AmmoAmountToRemove && AmmoAmountOfType > 0)
                {
                    RemovedAmmoAmount = AmmoAmountPerAmmoType[TypeOfAmmoToRemove];
                    AmmoAmountPerAmmoType[TypeOfAmmoToRemove] = 0;
                    return true;
                }
                else
                {
                    RemovedAmmoAmount = 0;
                    return false;
                }
            }

            RemovedAmmoAmount = 0;
            return false;
        }
        
        public int GetAmmoForWeapon(int WeaponID)
        {
            foreach (SaveableWeapon CollectedWeapon in AllCollectedWeapons)
            {
                if (CollectedWeapon.GetWeaponID() == WeaponID)
                {
                    Debug.Log(CollectedWeapon.GetCurrentAmmo());
                    return CollectedWeapon.GetCurrentAmmo();
                }
            }

            Debug.Log("Didnt find weapon in inventory");
            return 0;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IPickup Pickup))
            {
                Pickup.OnPickUp(gameObject);
            }
        }
    }
}
