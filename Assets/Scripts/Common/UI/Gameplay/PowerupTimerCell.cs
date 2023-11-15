using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrashDash.Scripts.Common.UI.Gameplay
{
    public class PowerupTimerCell : MonoBehaviour
    {
        [SerializeField] private Image timerBar;

        private float _progress = 0;
        private float _elapsedTime = 100f;
        private float _duration = 100f;

        private bool _isActive = false;

        private void OnEnable()
        {
            _isActive = false;
        }

        private void Update()
        {
            if (_isActive)
            {
                if (_elapsedTime > 0)
                {
                    _elapsedTime -= Time.deltaTime;
                    _progress = _elapsedTime / _duration;
                    timerBar.fillAmount = _progress;
                }
                else
                {
                    _isActive = false;
                    gameObject.SetActive(false);
                }
            }
        }

        public void ShowTimer(float duration)
        {
            _duration = duration;
            _elapsedTime = duration;
            timerBar.fillAmount = 1;
            _isActive = true;
        }

        public void Free()
        {
            _isActive = false;
            _elapsedTime = _duration = 0;
            timerBar.fillAmount = 0;
            gameObject.SetActive(false);
        }
    }
}
