using System.Linq;
using System.Collections;
using System.Collections.Generic;
using StatusEffects.Enums;
using UnityEngine;

namespace StatusEffects.CombatEffects.Datas
{
    [CreateAssetMenu(fileName = "Combat Effect Database", menuName = "Scriptable Objects/Databases/Combat Effect Database", order = 2)]
    public class CombatEffectDatabase : ScriptableObject
    {
        [SerializeField] private List<CombatEffect> combatEffects;

        private Dictionary<StatusEffectEnum, CombatEffect> _combatEffects;

        public Dictionary<StatusEffectEnum, CombatEffect> CombatEffects
        {
            get
            {
                if (_combatEffects == null)
                    _combatEffects = combatEffects.ToDictionary(key => key.StatusEffectType);

                return _combatEffects;
            }
        }
    }
}
