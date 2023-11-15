using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Interfaces;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Obstacles
{
    public class PatrollerObstacle : BaseObstacle, IObstacleMoveable, ICreatureObstacle
    {
        [SerializeField] private Transform ratTransform;
        [SerializeField] private Animator animator;
        [SerializeField] private Collider pattrollerCollider;
        
        [Header("Movement")]
        [SerializeField] private float minSpeed = 1f;
        [SerializeField] private float maxSpeed = 3f;

        [Header("Audios")]
        [SerializeField] private AudioSource patrollerAudio;
        [SerializeField] private AudioClip[] ratLoopClips;
        [SerializeField] private AudioClip ratDeadClip;

        private static int _speedRatioHash = Animator.StringToHash("SpeedRatio");
        private static int _deadHash = Animator.StringToHash("Dead");

        private bool _moveable = true;

        private float _time = 0;
        private float _actualTime = 0;
        private float _maxSpeed = 0;
        private float _speedSign = 1;
        private float _runDuration = 0;
        private Vector3 _originalPosition;

        private void OnEnable()
        {
            _originalPosition = Random.value >= 0.5f ? new Vector3(1.5f, 0, 0) 
                                                     : new Vector3(-1.5f, 0, 0);
            _runDuration = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

            _speedSign = -Mathf.Sign(_originalPosition.x);
            _actualTime = Random.Range(minSpeed, maxSpeed);

            _maxSpeed = 3 / _actualTime;
            ratTransform.localRotation = Quaternion.Euler(0, 90 * _speedSign, 0);
        }

        private void Update()
        {
            animator.SetFloat(_speedRatioHash, _moveable ? _runDuration / _actualTime / 2 : 0);
            
            if (_moveable)
            {
                _time += Time.deltaTime * _maxSpeed;
                transform.localPosition = _originalPosition + _speedSign * transform.right * Mathf.PingPong(_time, 3);
            }
        }

        public override void HitObstacle()
        {
            SetMoveable(false);
            pattrollerCollider.enabled = false;
            animator.SetTrigger(_deadHash);

            Death();
        }

        public void SetMoveable(bool moveable)
        {
            _moveable = moveable;

            if (moveable)
            {
                patrollerAudio.clip = ratLoopClips[Random.Range(0, ratLoopClips.Length)];
                patrollerAudio.loop = true;
                patrollerAudio.Play();
            }
        }

        public void Death()
        {
            patrollerAudio.clip = ratDeadClip;
            patrollerAudio.loop = false;
            patrollerAudio.Play();
        }
    }
}
