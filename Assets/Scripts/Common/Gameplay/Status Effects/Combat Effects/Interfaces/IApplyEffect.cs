namespace StatusEffects.CombatEffects.Interfaces
{
    public interface IApplyCombatEffect
    {
        public void ApplyEffect(CombatEffect combatEffect);
        public void RemoveEffect(CombatEffect combatEffect);
        public void DisposeAll();
    }
}
