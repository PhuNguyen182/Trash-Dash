using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusEffects.Effects;
using System;
using StatusEffects.Enums;
using StatusEffects.CombatEffects;

namespace TrashDash.Scripts.Common.Gameplay.StatusEffects.Effects
{
    public class MultiplyEffect : BaseStatusEffect
    {
        private bool _isPause = false;
        private bool _isExpired = false;
        private float _elapsedTime = 0; // Use this variable to calculate available time

        public override bool IsPaused => _isPause;
        public override bool IsExpired => _isExpired;
        public override bool CanBeStacked => false;

        public override float Duration { get; set; }
        public override float AffectingPercentage { get; }
        public override float TickRate => Time.deltaTime;

        public override Action OnStart { get; set; }
        public override Action OnStop { get; set; }
        public override Action<bool> OnPause { get; set; }
        public override Action<float> OnTick { get; set; }

        public override StatusEffectEnum StatusEffectType => StatusEffectEnum.Multiply;

        public override CombatEffect CombatEffect { get; set; }

        public override void Pause(bool isPause)
        {
            _isPause = isPause;
            OnPause?.Invoke(isPause);

            if (combatEffect != null)
            {
                if (_isPause)
                    combatEffect.Pause();
                else
                    combatEffect.Play();
            }
        }

        public override void Reset()
        {
            _elapsedTime = 0;
            _isPause = false;
            _isExpired = false;
        }

        public override void Stop()
        {
            OnStop?.Invoke();
            Dispose();
        }

        public override void Update()
        {
            if (_elapsedTime < Duration)
            {
                _elapsedTime += TickRate;
                OnTick?.Invoke(_elapsedTime / Duration);
            }
            else
                _isExpired = true;
        }
    }
}