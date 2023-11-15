using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TrashDash.Scripts.Common.Enumerations;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Pickups
{
    public class Powerup : Consumable
    {
        [SerializeField] private PowerupEnum powerup;
        [SerializeField] private ParticleSystem onCollectedEffect;
        [SerializeField] private ParticleSystem onReleaseEffect;
        [SerializeField] private AudioSource powerUpAudio;

        public PowerupEnum PowerupType => powerup;

        public override void StartPickup()
        {
            if (powerUpAudio != null)
                powerUpAudio.Play();

            if (onCollectedEffect != null)
            {
                SimplePool.Spawn(onCollectedEffect, EffectContainer.InstanceTransform, transform.position, Quaternion.Euler(-90, 0, 0));
            }
        }

        public override void ReleasePickUp()
        {
            if (onReleaseEffect != null)
            {
                SimplePool.Spawn(onReleaseEffect, EffectContainer.InstanceTransform, transform.position, Quaternion.identity);
            }

            transform.SetParent(PowerupContainer.InstanceTransform);
            SimplePool.Despawn(gameObject);
        }
    }
}
