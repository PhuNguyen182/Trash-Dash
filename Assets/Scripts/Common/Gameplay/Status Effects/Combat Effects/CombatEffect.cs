using StatusEffects.CombatEffects.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusEffects.Enums;

namespace StatusEffects.CombatEffects
{
    public class CombatEffect : MonoBehaviour, ICombatEffect
    {
        [Header("Name")]
        public StatusEffectEnum StatusEffectType;
        
        [Header("Graphics")]
        public Material BodyMaterial;
        public TrailRenderer trailEffect;
        public ParticleSystem particleEffects;

        public void Pause()
        {
            if (particleEffects != null)
                particleEffects.Pause();

            if (trailEffect != null)
                trailEffect.Clear();
        }

        public void Play()
        {
            if (particleEffects != null)
                particleEffects.Play();

            if (trailEffect != null)
                trailEffect.gameObject.SetActive(true);
        }

        public void Stop()
        {
            if (particleEffects != null)
                particleEffects.Stop();

            if (trailEffect != null)
                trailEffect.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            SimplePool.Despawn(gameObject);
        }
    }
}
