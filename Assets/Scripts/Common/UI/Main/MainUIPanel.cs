using System;
using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.UI.Main.Leaderboard;
using TrashDash.Scripts.Common.UI.Main.Mission;
using TrashDash.Scripts.Common.UI.Main.Setting;
using TrashDash.Scripts.Common.UI.Main.Shop;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TrashDash.Scripts.Common.GameSystem.Scenes;
using TrashDash.Scripts.Common.GameSystem.Config;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.Databases;
using TrashDash.Scripts.Common.GameSystem.Managers;
using TrashDash.Scripts.Common.Inventory;
using TrashDash.Scripts.Common.DataStructs.Datas;
using TrashDash.Scripts.Common.Gameplay.Mainhome;

namespace TrashDash.Scripts.Common.UI.Main
{
    public class MainUIPanel : MonoBehaviour, IProgress<float>
    {
        [SerializeField] private ShopItemDatabase itemDatabase;
        [SerializeField] private ThemeDatabase themeDatabase;

        [Header("Panels")]
        [SerializeField] private ShopPanel shopPanel;
        [SerializeField] private LeaderboardPanel leaderboardPanel;
        [SerializeField] private MissionPanel missionPanel;
        [SerializeField] private SettingPanel settingPanel;

        [Header("Play")]
        [SerializeField] private Button playButton;

        [Header("Switch Theme")]
        [SerializeField] private Image themePreview;
        [SerializeField] private Button switchThemeLeft;
        [SerializeField] private Button switchThemeRight;

        [Header("Bottom Button")]
        [SerializeField] private Button leaderboardButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button missionButton;
        [SerializeField] private Button settingButton;

        [Header("Sound")]
        [SerializeField] private AudioClip menuSound;

        private int _currentTheme = 0;

        private int _themeCount => itemDatabase.Themes.Length;

        private void Awake()
        {
            PlayerConfig.LoadSelf();

            leaderboardButton.onClick.AddListener(() => OnLeaderboard().Forget());
            shopButton.onClick.AddListener(() => OnShop().Forget());
            missionButton.onClick.AddListener(() => OnMission().Forget());
            settingButton.onClick.AddListener(() => OnSetting().Forget());

            switchThemeLeft.onClick.AddListener(OnLeftTheme);
            switchThemeRight.onClick.AddListener(OnRightTheme);

            playButton.onClick.AddListener(UniTask.UnityAction(async () => await Play()));
        }

        private void Start()
        {
            CheckThemeButtons();
            MusicController.Instance.PlaySingle(menuSound, true);
        }

        private async UniTask Play()
        {
            PlayerConfig.Current = new PlayerConfig
            {
                MaxLife = 3,
                CurrentTheme = (Theme)_currentTheme,
                Decor = ConsumableManager.Instance.CharacterDecor
            };

            PlayerConfig.SaveSelf(PlayerConfig.Current);
            await SceneLoader.LoadScene(SceneLoader.GAMEPLAY, this);
        }

        private async UniTask OnLeaderboard()
        {
            leaderboardPanel.gameObject.SetActive(true);
            await leaderboardPanel.OnAppear();
        }

        private async UniTask OnShop()
        {
            shopPanel.gameObject.SetActive(true);
            await shopPanel.OnAppear();
        }

        private async UniTask OnMission()
        {
            missionPanel.gameObject.SetActive(true);
            await missionPanel.OnAppear();
        }

        private async UniTask OnSetting()
        {
            settingPanel.gameObject.SetActive(true);
            await settingPanel.OnAppear();
        }

        private void OnLeftTheme()
        {
            _currentTheme -= 1;
            themePreview.sprite = itemDatabase.Themes[_currentTheme].ItemIcon;
            switchThemeLeft.interactable = _currentTheme > 0;
            switchThemeRight.interactable = _currentTheme < _themeCount - 1;
            CheckTheme();
        }

        private void OnRightTheme()
        {
            _currentTheme += 1;
            themePreview.sprite = itemDatabase.Themes[_currentTheme].ItemIcon;
            switchThemeLeft.interactable = _currentTheme > 0;
            switchThemeRight.interactable = _currentTheme < _themeCount - 1;
            CheckTheme();
        }

        private void CheckThemeButtons()
        {
            if (PlayerConfig.Current != null)
                _currentTheme = (int)PlayerConfig.Current.CurrentTheme;

            themePreview.sprite = itemDatabase.Themes[_currentTheme].ItemIcon;
            switchThemeLeft.interactable = _currentTheme > 0;
            switchThemeRight.interactable = _currentTheme < _themeCount - 1;

            CheckTheme();
        }

        private void CheckTheme()
        {
            Theme skyTheme = (Theme)(_currentTheme);
            SkyData sky = themeDatabase.GetThemeSky(skyTheme);
            MainMenu.Instance.CharacterPreview.ShowPreviewSky(sky);

            string themeId = itemDatabase.Themes[_currentTheme].ID;
            bool canPlay = ShopInventory.Instance.ThemeItems[themeId];
            playButton.interactable = canPlay;
        }

        public void Report(float value)
        {
            
        }
    }
}
