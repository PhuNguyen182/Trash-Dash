using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TrashDash.Scripts.Common.GameSystem.Managers;

namespace TrashDash.Scripts.Common.UI.Gameplay
{
    public class GameplayPanel : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;

        [Header("Texts")]
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private TMP_Text premiumText;
        [SerializeField] private TMP_Text multiplierText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text distanceText;

        [Header("Images")]
        [SerializeField] private Image[] hearts;

        [Header("Panels")]
        [SerializeField] private PausePanel pausePanel;
        [SerializeField] private ContinuePanel continuePanel;

        [Header("Sound")]
        [SerializeField] private AudioClip gamePlaySound;
        [SerializeField] private AudioClip deadLoopSound;

        private void Awake()
        {
            pauseButton.onClick.AddListener(Pause);
        }

        public void UpdateMultiplier(int multiplier)
        {
            multiplierText.text = $"x{multiplier * GameDataManager.CurrentData.Multiplier}";
        }

        public void UpdateDistance(float d)
        {
            distanceText.text = $"{d:F0} m";
        }

        public void UpdateScore(float s)
        {
            scoreText.text = $"{s:F0}";
        }

        public void UpdateCoin(int c)
        {
            coinText.text = $"{c}";
        }

        public void UpdatePremium(int p)
        {
            premiumText.text = $"{p}";
        }

        public void UpdateLife(int life)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].color = life - 1 < i ? Color.black : Color.white;
            }
        }

        public void ShowContinuePanel()
        {
            continuePanel.gameObject.SetActive(true);
        }

        private void Pause()
        {
            pausePanel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        public void PlayGameMusic()
        {
            MusicController.Instance.PlaySingle(gamePlaySound, true);
        }

        public void PlayDeadLoop()
        {
            MusicController.Instance.PlaySingle(deadLoopSound, true);
        }
    }
}
