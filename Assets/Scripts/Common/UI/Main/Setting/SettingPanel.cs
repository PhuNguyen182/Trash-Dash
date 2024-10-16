using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrashDash.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace TrashDash.Scripts.Common.UI.Main.Setting
{
    public class SettingPanel : MonoBehaviour, IPanelUI
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        private float _sfx = 0;
        private float _master = 0;
        private float _music = 0;

        public async UniTask Close()
        {
            await UniTask.CompletedTask;
            gameObject.SetActive(false);
        }

        public async UniTask OnAppear()
        {
            await UniTask.CompletedTask;
        }

        public void OnCLose()
        {
            
        }

        private void Awake()
        {
            closeButton.onClick.AddListener(() => Close().Forget());

            _sfx = MusicController.Instance.SoundVolume;
            _master = MusicController.Instance.MasterVolume;
            _music = MusicController.Instance.MusicVolume;

            masterSlider.onValueChanged.AddListener(OnMasterChange);
            musicSlider.onValueChanged.AddListener(OnMusicChange);
            sfxSlider.onValueChanged.AddListener(OnSFXChange);

            masterSlider.value = _master;
            musicSlider.value = _music;
            sfxSlider.value = _sfx;
        }

        private void OnMasterChange(float value)
        {
            MusicController.Instance.MasterVolume = value;
        }

        private void OnMusicChange(float value)
        {
            MusicController.Instance.MusicVolume = value;
        }

        private void OnSFXChange(float value)
        {
            MusicController.Instance.SoundVolume = value;
        }
    }
}
