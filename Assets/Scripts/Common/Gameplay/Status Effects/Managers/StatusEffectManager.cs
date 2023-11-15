using StatusEffects.Effects;
using StatusEffects.Interfaces;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace StatusEffects.Managers
{
    public class StatusEffectManager : IService, IDisposable
    {
        private List<BaseStatusEffect> effects = new List<BaseStatusEffect>();
        private CompositeDisposable disposables = new CompositeDisposable();

        public StatusEffectManager()
        {
            Initialize();
        }

        public void Initialize()
        {
            Observable.EveryUpdate()
                      .Subscribe(_ =>
                      {
                          if (effects.Count > 0)
                          {
                              for (int i = 0; i < effects.Count; i++)
                              {
                                  effects[i].Tick();
                              }
                          }
                      })
                      .AddTo(disposables);
        }

        public void AddEffect(BaseStatusEffect effect, Vector3 pos, Quaternion rot, Transform parent = null)
        {
            if (effect.CanBeStacked)
            {
                effect.OnStop += () => RemoveEffect(effect);
                effect.PlayCombatEffect(pos, rot, parent);
                effect.Start();
                effects.Add(effect);
            }

            else
            {
                if (!TryFindEffect(effect, out var foundEffect))
                {
                    effect.OnStop += () => RemoveEffect(effect);
                    effect.PlayCombatEffect(pos, rot, parent);
                    effect.Start();
                    effects.Add(effect);
                }

                else
                    foundEffect.Reset();
            }
        }

        public void RemoveEffect(BaseStatusEffect effect)
        {
            effects.Remove(effect);
        }

        public void Pause()
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Pause(true);
            }
        }

        public void Stop()
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Stop();
            }
        }

        public void Reset()
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Reset();
            }
        }

        public bool ContainEffect(BaseStatusEffect effect)
        {
            return effects.Contains(effect);
        }

        public bool TryFindEffect(BaseStatusEffect effect, out BaseStatusEffect statusEffect)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].StatusEffectType == effect.StatusEffectType)
                {
                    statusEffect = effects[i];
                    return true;
                }
            }

            statusEffect = null;
            return false;
        }

        public void Dispose()
        {
            Stop();
            effects.Clear();
            disposables.Dispose();
        }
    }
}
