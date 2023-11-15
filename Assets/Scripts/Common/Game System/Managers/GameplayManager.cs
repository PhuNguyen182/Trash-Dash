using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.UI.Gameplay;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Character;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Tracks;
using TrashDash.Scripts.Common.DataStructs.Messages;
using TrashDash.Scripts.Common.Enumerations;
using Cysharp.Threading.Tasks;
using UniRx;
using TrashDash.Scripts.Common.GameSystem.Config;

namespace TrashDash.Scripts.Common.GameSystem.Managers
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private GameplayPanel gameplayPanelUI;
        [SerializeField] private PlayerObserver playerObserver;
        [SerializeField] private CharacterControllerPivot characterPivot;
        [SerializeField] private TrackManager trackManager;

        private bool _moveable = false;
        private float _trackSpeedProgress = 0;
        private float _smoothedSpeed = 10;
        private CancellationToken _cancellationToken;

        public PlayerObserver PlayerObserver => playerObserver;

        public static GameplayManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            if (PlayerConfig.Current == null)
            {
                PlayerConfig.Current = new PlayerConfig
                {
                    CurrentTheme = Theme.Day,
                    MaxLife = 3
                };
            }

            _cancellationToken = this.GetCancellationTokenOnDestroy();

            MessageBroker.Default.Receive<CharacterInjuredMessage>()
                                 .Subscribe(_ => OnPlayerInjured()).AddTo(this);

            MessageBroker.Default.Receive<CharacterDeathMessage>()
                                 .Subscribe(_ => OnPlayerDeath().Forget()).AddTo(this);

            MessageBroker.Default.Receive<RebornMessage>()
                                 .Subscribe(_ => Reborn().Forget())
                                 .AddTo(this);
        }

        private void Start()
        {
            StartGame();
        }

        private void Update()
        {
            if (_moveable)
            {
                ModerateTrackSpeedOvertime();
            }

            UpdateUI();
        }

        private void StartGame()
        {
            characterPivot.SkyThemeChanger.ChangeSky(PlayerConfig.Current.CurrentTheme);
            characterPivot.CharacterDecoration.Decorate(PlayerConfig.Current.Decor);
            gameplayPanelUI.PlayGameMusic();
            StartGameDelayed().Forget();
        }

        private void StartRun()
        {
            _moveable = true;
            trackManager.SetMoveable(true);
            trackManager.StartTrack();
        }

        private void OnPlayerInjured()
        {
            PauseGameForAWhile().Forget();
        }

        private async UniTask OnPlayerDeath()
        {
            _moveable = false;
            characterPivot.CharacterInput.IsActivate = false;
            trackManager.SetMoveable(false);

            playerObserver.SaveCurrency();
            playerObserver.SaveScore();

            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: _cancellationToken);
            if (_cancellationToken.IsCancellationRequested)
                return;

            gameplayPanelUI.ShowContinuePanel();
            gameplayPanelUI.PlayDeadLoop();
            trackManager.SetMoveable(false);
        }

        private async UniTask Reborn()
        {
            characterPivot.CharacterCollider.RefillHealth();
            await StartGameDelayed();
            ResetTrackProgress();

            trackManager.SetMoveable(true);
            _moveable = true;
            characterPivot.CharacterInput.IsActivate = true;
        }

        private async UniTask PauseGameForAWhile()
        {
            _moveable = false;
            characterPivot.CharacterInput.IsActivate = false;
            trackManager.SetMoveable(false);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _cancellationToken);
            if (_cancellationToken.IsCancellationRequested) return;

            ResetTrackProgress();
            trackManager.SetMoveable(true);
            _moveable = true;
            characterPivot.CharacterInput.IsActivate = true;
        }

        private async UniTask StartGameDelayed()
        {
            _moveable = false;

            characterPivot.CharacterInput.IsActivate = false;
            characterPivot.CharacterAnimation.PlayStart();

            await UniTask.Delay(TimeSpan.FromSeconds(3.5f), cancellationToken: _cancellationToken);
            if (_cancellationToken.IsCancellationRequested) return;

            StartRun();
            characterPivot.CharacterAnimation.PlayRunStart();
            characterPivot.CharacterInput.IsActivate = true;
        }

        private void ModerateTrackSpeedOvertime()
        {
            _trackSpeedProgress += Time.deltaTime;

            if (_trackSpeedProgress >= 25 && _trackSpeedProgress < 50)
            {
                _smoothedSpeed = Mathf.Lerp(_smoothedSpeed, 15, Time.deltaTime);
                trackManager.SetMoveSpeed(_smoothedSpeed);
            }

            else if (_trackSpeedProgress >= 50 && _trackSpeedProgress < 75)
            {
                _smoothedSpeed = Mathf.Lerp(_smoothedSpeed, 20, Time.deltaTime);
                trackManager.SetMoveSpeed(_smoothedSpeed);
            }

            else if (_trackSpeedProgress >= 75 && _trackSpeedProgress < 100)
            {
                _smoothedSpeed = Mathf.Lerp(_smoothedSpeed, 25, Time.deltaTime);
                trackManager.SetMoveSpeed(_smoothedSpeed);
            }

            else if (_trackSpeedProgress >= 100)
            {
                _smoothedSpeed = Mathf.Lerp(_smoothedSpeed, 30, Time.deltaTime);
                trackManager.SetMoveSpeed(_smoothedSpeed);
            }
        }

        private void ResetTrackProgress()
        {
            _trackSpeedProgress = 0;
            _smoothedSpeed = 10;
            trackManager.ResetTrackSpeedProgress();
        }

        private void UpdateUI()
        {
            gameplayPanelUI.UpdateDistance(playerObserver.Distance);
            gameplayPanelUI.UpdateScore(playerObserver.Score);
        }
    }
}
