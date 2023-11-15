using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusEffects.Interfaces;
using StatusEffects.Enums;
using System;
using StatusEffects.CombatEffects.Interfaces;
using StatusEffects.CombatEffects;

namespace StatusEffects.Effects
{
    public abstract class BaseStatusEffect : IStatusEffect, IDisposable
    {
        public abstract bool IsPaused { get; }
        public abstract bool IsExpired { get; }
        public abstract bool CanBeStacked { get; }

        public abstract float Duration { get; set; }
        public abstract float AffectingPercentage { get; }
        public abstract float TickRate { get; }

        public abstract Action OnStart { get; set; }
        public abstract Action OnStop { get; set; }
        public abstract Action<bool> OnPause { get; set; }
        public abstract Action<float> OnTick { get; set; }

        public abstract StatusEffectEnum StatusEffectType { get; }
        public abstract CombatEffect CombatEffect { get; set; }

        protected CombatEffect combatEffect;

        public virtual void Start()
        {
            OnStart?.Invoke();
        }

        public void PlayCombatEffect(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if(CombatEffect != null)
            {
                combatEffect = SimplePool.Spawn(CombatEffect, parent, position, rotation);
            }
        }

        public abstract void Pause(bool isPause);

        public abstract void Stop();

        public abstract void Reset();

        public abstract void Update();

        public void Tick()
        {
            if (!IsPaused && !IsExpired)
            {
                Update();
            }
            else if (IsExpired)
            {
                Stop();
            }
        }

        public void Dispose()
        {
            OnStart = null;
            OnStop = null;
            OnPause = null;
            OnTick = null;

            combatEffect?.Dispose();
        }
    }
}