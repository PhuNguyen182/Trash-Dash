using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Character
{
    public class CharacterSound : MonoBehaviour
    {
        [SerializeField] private AudioSource characterAudio;

        [Header("Sounds")]
        [SerializeField] private AudioClip deathClip;
        [SerializeField] private AudioClip injuredClip;
        [SerializeField] private AudioClip jumpClip;
        [SerializeField] private AudioClip slideClip;
        [SerializeField] private AudioClip coinClip;

        public void PlayDeathSound()
        {
            PlaySound(deathClip);
        }

        public void PlayInjuredSound()
        {
            PlaySound(injuredClip);
        }

        public void PlayJump()
        {
            PlaySound(jumpClip);
        }

        public void PlaySlide()
        {
            PlaySound(slideClip);
        }

        public void PlayCoin()
        {
            PlayOneShotSound(coinClip);
        }

        private void PlaySound(AudioClip clip, bool loop = false)
        {
            if (characterAudio == null || clip == null)
                return;

            characterAudio.clip = clip;
            characterAudio.loop = loop;
            characterAudio.Play();

        }

        private void PlayOneShotSound(AudioClip clip)
        {
            if (characterAudio == null || clip == null)
                return;

            characterAudio.PlayOneShot(clip);
        }
    }
}
