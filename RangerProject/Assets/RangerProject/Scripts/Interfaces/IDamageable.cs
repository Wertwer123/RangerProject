namespace RangerProject.Scripts.Interfaces
{
    //interface useable for everything that should recieve dmg in any way
    public interface IDamageable
    {
        //Should return if the hit was the final hit
        public bool DealDmg(int Dmg);
    }
}
