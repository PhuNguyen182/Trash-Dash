using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.DataStructs.Messages;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Pickups;
using UniRx;
using UnityEngine;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Character
{
    public class CharacterConsumable : MonoBehaviour
    {
        [SerializeField] private LayerMask coinMask;
        [SerializeField] private Transform effectContainer;
        [SerializeField] private CharacterCollider characterCollider;
        [SerializeField] private CharacterStatus characterStatus;

        private RaycastHit[] _coinHits;

        public void OnMagnet()
        {
            characterStatus.OnMagnet(effectContainer.position
                                     , Quaternion.identity
                                     , effectContainer
                                     , onTick: _ => ProcessMagnet()
                                     , onStop: () => characterCollider.CollectedCoins.Clear());
        }

        public void OnMultiply()
        {
            characterStatus.OnMultiply(effectContainer.position
                                       , Quaternion.identity
                                       , effectContainer
                                       , () => ProcessMultiply(false)
                                       , () => ProcessMultiply(true));
        }

        public void OnInvincible()
        {
            characterStatus.OnInvincible(effectContainer.position
                                         , Quaternion.identity
                                         , effectContainer
                                         , () => ProcessInvincible(false)
                                         , () => ProcessInvincible(true));
        }

        public void OnExtreLife()
        {
            characterStatus.OnExtraLife(effectContainer.position
                                        , Quaternion.identity
                                        , effectContainer
                                        , onStart: () => ProcessExtralife());
        }

        private void ProcessMagnet()
        {
            _coinHits = Physics.BoxCastAll(transform.position, new Vector3(4f, 1.5f, 6f), Vector3.forward, Quaternion.identity, 1.5f, coinMask);
            for (int i = 0; i < _coinHits.Length; i++)
            {
                if (_coinHits[i].transform.TryGetComponent<Coin>(out var coin))
                {
                    if (!coin.IsPremium)
                    {
                        if (!characterCollider.CollectedCoins.ContainsKey(coin.Key))
                            characterCollider.CollectedCoins.Add(coin.Key, coin);
                    }
                }
            }
        }

        public void Free()
        {
            characterStatus.Free();
            MessageBroker.Default.Publish(new FreePowerupUI { });
        }

        private void ProcessExtralife()
        {
            characterCollider.TakeDamage(-1);
        }

        private void ProcessMultiply(bool hasDone)
        {
            MessageBroker.Default.Publish(new MultiplyPowerupMessage
            {
                IsCompleted = hasDone
            });
        }

        private void ProcessInvincible(bool invincible)
        {
            characterCollider.SetHittable(invincible);
        }
    }
}
