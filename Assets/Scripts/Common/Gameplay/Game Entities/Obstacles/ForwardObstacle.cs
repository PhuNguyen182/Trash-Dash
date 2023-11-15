using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Interfaces;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Obstacles
{
    public class ForwardObstacle : BaseObstacle, IObstacleMoveable, ICreatureObstacle
    {
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody dogBody;
        [SerializeField] private LayerMask playerMask;

        [Header("Audios")]
        [SerializeField] private AudioSource dogAudio;
        [SerializeField] private AudioClip[] dogLoopClips;
        [SerializeField] private AudioClip dogHitClip;

        private static int _runHash = Animator.StringToHash("Run");
        private static int _deadHash = Animator.StringToHash("Death");

        private bool _moveable = true;
        private bool _startMove = false;
        private Ray _playerDetechRay;
        private RaycastHit _playerCastHit;

        private void Update()
        {
            if (_moveable)
            {
                DetectPlayer();
                MoveForward();
            }
        }

        public override void HitObstacle()
        {
            obstacleCollider.enabled = false;
            Death();
        }

        public void SetMoveable(bool moveable)
        {
            _moveable = moveable;
        }

        public void Death()
        {
            animator.SetTrigger(_deadHash);

            dogAudio.clip = dogHitClip;
            dogAudio.loop = false;
            dogAudio.Play();
        }

        private void DetectPlayer()
        {
            _playerDetechRay = new Ray { origin = transform.position, direction = transform.forward };
            if(Physics.Raycast(_playerDetechRay, out _playerCastHit, 12f, playerMask))
            {
                if (!_startMove)
                {
                    _startMove = true;
                    animator.SetTrigger(_runHash);

                    dogAudio.clip = dogLoopClips[Random.Range(0, dogLoopClips.Length)];
                    dogAudio.loop = false;
                    dogAudio.Play();
                }
            }
        }

        private void MoveForward()
        {
            if (_startMove)
            {
                dogBody.MovePosition(dogBody.position + transform.forward * Time.deltaTime * moveSpeed);
            }
        }
    }
}
