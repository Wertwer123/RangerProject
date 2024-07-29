using UnityEngine;

namespace RangerProject.Scripts.Player.WeaponSystem
{
    [CreateAssetMenu(menuName = "Weapons/NewWeaponData", fileName = "NewWeapon")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField, Min(1)] private int Dmg = 10;
        [SerializeField, Min(0)] private int ShotsPerSecond = 3;
        [SerializeField] private string WeaponName = "DefaultWeaponName";
        [SerializeField] private EWeaponType WeaponType;
        [SerializeField] private Mesh WeaponMesh;

        public int GetWeaponDmg() => Dmg;
        public float GetShotDelay() => 1.0f / ShotsPerSecond;
        public string GetWeaponName() => WeaponName;
        public EWeaponType GetWeaponType() => WeaponType;
        public Mesh GetWeaponMesh() => WeaponMesh;
    }
}
