using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Enumerations;
using Cysharp.Threading.Tasks;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Obstacles
{
    public class Obstacle : BaseObstacle
    {
        [SerializeField] private Animation obstacleAnimation;
        [SerializeField] private AudioSource obstacleSound;

        private CancellationToken _token;

        public ObstacleType ObstacleType => obstacleType;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
        }

        private void OnEnable()
        {
            obstacleCollider.enabled = true;
        }

        public override void HitObstacle()
        {
            obstacleCollider.enabled = false;
            obstacleAnimation.Play();
            obstacleSound.Play();
            DisableSelfAwait().Forget();
        }

        private async UniTask DisableSelfAwait()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(obstacleAnimation.clip.length + 0.5f), cancellationToken: _token);
            if (_token.IsCancellationRequested)
                return;

            this.gameObject.SetActive(false);
        }
    }
}
