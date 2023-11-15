using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Character
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;

        private static int _randomIdleHash = Animator.StringToHash("RandomIdle");
        private static int _startHash = Animator.StringToHash("Start");
        private static int _runStartHash = Animator.StringToHash("Start");
        private static int _movingHash = Animator.StringToHash("Moving");
        private static int _hitHash = Animator.StringToHash("Hit");
        private static int _jumpingHash = Animator.StringToHash("Jumping");
        private static int _jumpSpeedHash = Animator.StringToHash("JumpSpeed");
        private static int _slidingHash = Animator.StringToHash("Sliding");
        private static int _deadHash = Animator.StringToHash("Dead");
        private static int _randomDeadHash = Animator.StringToHash("RandomDeath");

        public void SetAnimator(Animator animator)
        {
            characterAnimator = animator;
        }

        public void PlayRandomIdle()
        {
            int rand = Random.Range(0, 5);
            characterAnimator.SetInteger(_randomIdleHash, rand);
        }

        public void PlayJump(bool jump)
        {
            characterAnimator.SetBool(_jumpingHash, jump);
        }

        public void SetJumpSpeed(float jumpSpeed)
        {
            characterAnimator.SetFloat(_jumpSpeedHash, jumpSpeed);
        }

        public void PlaySlide(bool slide)
        {
            characterAnimator.SetBool(_slidingHash, slide);
        }

        public void PlayHit()
        {
            characterAnimator.SetTrigger(_hitHash);
        }

        public void PlayDead(bool dead)
        {
            characterAnimator.SetBool(_deadHash, dead);
            PlayDeadRandom();
        }

        public void PlayDeadRandom()
        {
            int dead = Random.Range(0, 2);
            characterAnimator.SetInteger(_randomDeadHash, dead);
        }

        public void PlayMove(bool move)
        {
            characterAnimator.SetBool(_movingHash, move);
        }

        public void PlayStart()
        {
            characterAnimator.Play(_startHash);
        }

        public void PlayRunStart()
        {
            characterAnimator.SetTrigger(_runStartHash);
        }
    }
}
