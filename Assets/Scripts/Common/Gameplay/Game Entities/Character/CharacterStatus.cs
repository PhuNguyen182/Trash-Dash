using System;
using StatusEffects.CombatEffects.Datas;
using StatusEffects.Factories;
using StatusEffects.Managers;
using StatusEffects.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Gameplay.StatusEffects.Effects;
using TrashDash.Scripts.Common.GameSystem.Managers;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.DataStructs.Messages;
using UniRx;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Character
{
    public class CharacterStatus : MonoBehaviour
    {
        [SerializeField] private CombatEffectDatabase effectDatabase;
        [SerializeField] private AnimationCurve durationCurveCalc;

        private CompositeDisposable _disposables;
        private StatusEffectManager _statusEffectManager;
        private StatusEffectFactory _statusEffectFactory;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _disposables = new CompositeDisposable();
            _statusEffectManager = new StatusEffectManager().AddTo(_disposables);
            _statusEffectFactory = new StatusEffectFactory(effectDatabase);
        }

        public void OnMagnet(Vector3 pos, Quaternion rot, Transform parent = null
            , Action onStart = null, Action onStop = null, Action<float> onTick = null, Action<bool> onPause = null)
        {
            MagnetEffect magnetEffect = _statusEffectFactory.Create(StatusEffectEnum.Magnet) as MagnetEffect;
            float duration = 6 * durationCurveCalc.Evaluate(1.0f * GameDataManager.CurrentData.MagnetLevel / 6f);
            magnetEffect.Duration = 6 + duration;

            if (!_statusEffectManager.ContainEffect(magnetEffect))
            {
                magnetEffect.OnStart += onStart;
                magnetEffect.OnStop += onStop;
                magnetEffect.OnTick += onTick;
                magnetEffect.OnPause += onPause;
            }

            _statusEffectManager.AddEffect(magnetEffect, pos, rot, parent);

            MessageBroker.Default.Publish(new PowerupTimerMessage
            {
                Duration = magnetEffect.Duration,
                Powerup = PowerupEnum.Magnet
            });
        }

        public void OnInvincible(Vector3 pos, Quaternion rot, Transform parent = null
            , Action onStart = null, Action onStop = null, Action<float> onTick = null, Action<bool> onPause = null)
        {
            InvincibleEffect invincibleEffect = _statusEffectFactory.Create(StatusEffectEnum.Invincible) as InvincibleEffect;
            float duration = 6 * durationCurveCalc.Evaluate(1.0f * GameDataManager.CurrentData.InvincibleLevel / 6f);
            invincibleEffect.Duration = 6 + duration;

            if (!_statusEffectManager.ContainEffect(invincibleEffect))
            {
                invincibleEffect.OnStart += onStart;
                invincibleEffect.OnStop += onStop;
                invincibleEffect.OnTick += onTick;
                invincibleEffect.OnPause += onPause;
            }

            _statusEffectManager.AddEffect(invincibleEffect, pos, rot, parent);

            MessageBroker.Default.Publish(new PowerupTimerMessage
            {
                Duration = invincibleEffect.Duration,
                Powerup = PowerupEnum.Invincible
            });
        }

        public void OnMultiply(Vector3 pos, Quaternion rot, Transform parent = null
            , Action onStart = null, Action onStop = null, Action<float> onTick = null, Action<bool> onPause = null)
        {
            MultiplyEffect multiplyEffect = _statusEffectFactory.Create(StatusEffectEnum.Multiply) as MultiplyEffect;
            float duration = 6 * durationCurveCalc.Evaluate(1.0f * GameDataManager.CurrentData.InvincibleLevel / 6f);
            multiplyEffect.Duration = 6 + duration;

            if (!_statusEffectManager.ContainEffect(multiplyEffect))
            {
                multiplyEffect.OnStart += onStart;
                multiplyEffect.OnStop += onStop;
                multiplyEffect.OnTick += onTick;
                multiplyEffect.OnPause += onPause;
            }

            _statusEffectManager.AddEffect(multiplyEffect, pos, rot, parent);

            MessageBroker.Default.Publish(new PowerupTimerMessage
            {
                Duration = multiplyEffect.Duration,
                Powerup = PowerupEnum.Multiply
            });
        }

        public void OnExtraLife(Vector3 pos, Quaternion rot, Transform parent = null
            , Action onStart = null, Action onStop = null, Action<float> onTick = null, Action<bool> onPause = null)
        {
            ExtraLifeEffect extraLifeEffect = _statusEffectFactory.Create(StatusEffectEnum.ExtraLife) as ExtraLifeEffect;
            extraLifeEffect.Duration = 3;

            if (!_statusEffectManager.ContainEffect(extraLifeEffect))
            {
                extraLifeEffect.OnStart += onStart;
                extraLifeEffect.OnStop += onStop;
                extraLifeEffect.OnTick += onTick;
                extraLifeEffect.OnPause += onPause;
            }

            _statusEffectManager.AddEffect(extraLifeEffect, pos, rot, parent);
        }

        public void Free()
        {
            _statusEffectManager.Stop();
        }

        private void OnDestroy()
        {
            _disposables?.Dispose();
        }
    }
}
