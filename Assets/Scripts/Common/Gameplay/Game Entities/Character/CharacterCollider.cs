using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Pickups;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Obstacles;
using TrashDash.Scripts.Common.Interfaces;
using TrashDash.Scripts.Common.DataStructs.Messages;
using UniRx;
using TrashDash.Scripts.Common.GameSystem.Config;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Character
{
    public class CharacterCollider : MonoBehaviour, ICharacterHealth
    {
        [SerializeField] private BoxCollider characterCollider;
        [SerializeField] private CharacterConsumable characterConsumable;
        [SerializeField] private CharacterAnimation characterAnimation;

        private int _hp;
        private bool _canBeHit = true;
        private bool _hasSoundComponent = false;
        private CharacterSound _characterSound;

        public int HP => _hp;
        public Dictionary<int, Coin> CollectedCoins { get; private set; }

        public Action<Coin> OnGetCoin;
        public Action<int> OnGetDamage;

        private void Awake()
        {
            _characterSound = GetComponentInChildren<CharacterSound>();
            _hasSoundComponent = _characterSound != null;
            CollectedCoins = new Dictionary<int, Coin>();
        }

        private void Start()
        {
            _hp = PlayerConfig.Current.MaxLife;
            OnGetDamage.Invoke(_hp);
        }

        private void Update()
        {
            Tick();
        }

        public void SetHittable(bool hit)
        {
            _canBeHit = hit;
        }

        public void SetCollider(HighColliderEnum heightState)
        {
            if(heightState == HighColliderEnum.High)
            {
                characterCollider.center = new Vector3(0, 0.55f, 0);
                characterCollider.size = new Vector3(0.6f, 1.1f, 0.4f);
            }

            else if(heightState == HighColliderEnum.Low)
            {
                characterCollider.center = new Vector3(0, 0.35f, 0);
                characterCollider.size = new Vector3(0.6f, 0.7f, 0.4f);
            }
        }

        private void Tick()
        {
            CoinTick();
        }

        private void CoinTick()
        {
            if (CollectedCoins.Count > 0)
            {
                foreach (Coin coin in CollectedCoins.Values)
                {
                    if (CollectedCoins.ContainsKey(coin.Key))
                    {
                        coin.transform.position = Vector3.MoveTowards(coin.transform.position, transform.position, Time.deltaTime * 30);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<BaseObstacle>(out var obstacle))
            {
                if (_canBeHit)
                {
                    obstacle.HitObstacle();
                    TakeDamage(1);
                    PlayOnObstacleAnimation();
                }
            }

            if (other.TryGetComponent<Coin>(out var coin))
            {
                if (CollectedCoins.ContainsKey(coin.Key))
                    CollectedCoins.Remove(coin.Key);

                if (_hasSoundComponent)
                    _characterSound.PlayCoin();

                coin.StartPickup();
                coin.Free(false);
                AddCoin(coin);
            }

            if (other.TryGetComponent<Powerup>(out var powerup))
            {
                powerup.StartPickup();
                
                switch (powerup.PowerupType)
                {
                    case PowerupEnum.Heart:
                        characterConsumable.OnExtreLife();
                        break;
                    case PowerupEnum.Magnet:
                        characterConsumable.OnMagnet();
                        break;
                    case PowerupEnum.Multiply:
                        characterConsumable.OnMultiply();
                        break;
                    case PowerupEnum.Invincible:
                        characterConsumable.OnInvincible();
                        break;
                }

                powerup.ReleasePickUp();
            }
        }

        private void AddCoin(Coin c)
        {
            OnGetCoin?.Invoke(c);
        }

        public void TakeDamage(int damage)
        {
            int hp = Mathf.Clamp(_hp - damage, 0, 3);
            _hp = hp;
            OnGetDamage?.Invoke(_hp);
        }

        private void PlayOnObstacleAnimation()
        {
            if (_hp > 0)
            {
                characterAnimation.PlayHit();
                if (_hasSoundComponent)
                    _characterSound.PlayInjuredSound();

                MessageBroker.Default.Publish(new CharacterInjuredMessage { });
            }
            else
            {
                characterAnimation.PlayHit();
                characterAnimation.PlayDead(true);

                if (_hasSoundComponent)
                    _characterSound.PlayDeathSound();

                characterConsumable.Free();
                MessageBroker.Default.Publish(new CharacterDeathMessage { });
            }
        }

        public void RefillHealth()
        {
            _hp = PlayerConfig.Current.MaxLife;
            OnGetDamage?.Invoke(_hp);
            characterAnimation.PlayDead(false);
        }
    }
}
