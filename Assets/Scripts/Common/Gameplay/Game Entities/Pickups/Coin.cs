using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Pickups
{
    public class Coin : Consumable
    {
        public int Value;
        public bool IsPremium;
        public int Key => _key;

        [SerializeField] private Collider currencyCollider;
        [SerializeField] private ParticleSystem onCollectedEffect;
        [SerializeField] private ParticleSystem onReleaseEffect;

        private int _key;
        public Action<int> OnFreeCoin;

        private void Awake()
        {
            _key = GetInstanceID();
        }

        private void OnEnable()
        {
            currencyCollider.enabled = true;
        }

        public override void StartPickup()
        {
            currencyCollider.enabled = false;

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
        }

        public void Free(bool isTrackFree)
        {
            currencyCollider.enabled = false;

            if (!isTrackFree)
                OnFreeCoin.Invoke(Key);
            
            transform.SetParent(CurrencyContainer.InstanceTransform);
            transform.position = Vector3.zero;

            OnFreeCoin = null;
            SimplePool.Despawn(this.gameObject);
        }
    }
}
