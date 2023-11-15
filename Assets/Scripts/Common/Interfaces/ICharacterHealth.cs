namespace TrashDash.Scripts.Common.Interfaces
{
    public interface ICharacterHealth
    {
        public int HP { get; }
        public void TakeDamage(int damage);
        public void RefillHealth();
    }
}
