using StatusEffects.Effects;
using StatusEffects.Enums;
using StatusEffects.CombatEffects.Datas;
using TrashDash.Scripts.Common.Gameplay.StatusEffects.Effects;

namespace StatusEffects.Factories
{
    public class StatusEffectFactory : IFactory<StatusEffectEnum, BaseStatusEffect>
    {
        private readonly CombatEffectDatabase _combatEffectDatabase;

        public StatusEffectFactory(CombatEffectDatabase combatEffectDatabase)
        {
            _combatEffectDatabase = combatEffectDatabase;
        }

        public BaseStatusEffect Create(StatusEffectEnum effectType)
        {
            BaseStatusEffect statusEffect;

            switch (effectType)
            {
                case StatusEffectEnum.Magnet:
                    statusEffect = new MagnetEffect();
                    break;
                case StatusEffectEnum.ExtraLife:
                    statusEffect = new ExtraLifeEffect();
                    break;
                case StatusEffectEnum.Invincible:
                    statusEffect = new InvincibleEffect();
                    break;
                case StatusEffectEnum.Multiply:
                    statusEffect = new MultiplyEffect();
                    break;
                default:
                    statusEffect = null;
                    break;
            }

            statusEffect.CombatEffect = _combatEffectDatabase.CombatEffects[effectType];
            return statusEffect;
        }
    }
}
