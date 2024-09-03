using RangerProject.Scripts.Interfaces;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField, Min(0)] private int MaxHealthPoints = 10;
    [SerializeField] private int CurrentHealthPoints = 10;

    private void Start()
    {
        CurrentHealthPoints = MaxHealthPoints;
    }

    public bool DealDmg(int Dmg)
    {
        CurrentHealthPoints -= Dmg;
        CurrentHealthPoints = Mathf.Clamp(CurrentHealthPoints, 0, MaxHealthPoints);

        //Meaning we are dead
        if (MaxHealthPoints == 0)
        {
            Destroy(gameObject);
            return true;
        }
        
        return false;
    }
}
