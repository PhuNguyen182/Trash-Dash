using System;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using TrashDash.Scripts.Common.GameSystem.Scenes;
using TrashDash.Scripts.Common.GameSystem.Managers;

namespace TrashDash.Scripts.Common.UI.Gameplay
{
    public class PausePanel : MonoBehaviour, IPanelUI, IProgress<float>
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private PlayerObserver playerObserver;

        private void Awake()
        {
            resumeButton.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                await Close();
            }));

            mainMenuButton.onClick.AddListener(UniTask.UnityAction(async () => 
            { 
                await BackToMainMenu(); 
            }));
        }

        public async UniTask Close()
        {
            OnCLose();
            await UniTask.CompletedTask;
            gameObject.SetActive(false);
        }

        public async UniTask OnAppear()
        {
            await UniTask.CompletedTask;
        }

        public void OnCLose()
        {
            Time.timeScale = 1;
        }

        private async UniTask BackToMainMenu()
        {
            playerObserver.SaveCurrency();
            playerObserver.SaveScore();
            await SceneLoader.LoadScene(SceneLoader.MAINHOME, this);
        }

        public void Report(float value)
        {
            if (value == 1f)
                Time.timeScale = 1;
        }
    }
}
