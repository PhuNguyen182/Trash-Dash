using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusEffects.Enums;
using StatusEffects.CombatEffects.Interfaces;
using StatusEffects.CombatEffects;

namespace StatusEffects.Interfaces
{
    public interface IStatusEffect
    {
        public bool IsPaused { get; }
        public bool IsExpired { get; }
        public bool CanBeStacked { get; }

        public float Duration { get; set; }
        public float AffectingPercentage { get; }
        public float TickRate { get; }

        public Action OnStart { get; set; }
        public Action OnStop { get; set; }
        public Action<bool> OnPause { get; set; }
        public Action<float> OnTick { get; set; }

        public StatusEffectEnum StatusEffectType { get; }
        public CombatEffect CombatEffect { get; set; }

        public void Tick();
        public void Reset();
        public void Stop();
        public void Pause(bool isPause);
    }
}
