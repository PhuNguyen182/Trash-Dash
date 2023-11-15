using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Character;
using TrashDash.Scripts.Common.Databases;
using Cysharp.Threading.Tasks;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Pickups;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Miscs;
using TrashDash.Scripts.Common.GameSystem.Managers;
using TrashDash.Scripts.Common.GameSystem.Config;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Tracks
{
    public class TrackManager : MonoBehaviour
    {
        [SerializeField] private float minSpeed = 10f;
        [SerializeField] private float maxSpeed = 30f;
        [SerializeField] private CharacterControllerPivot characterController;
        [SerializeField] private ConsumableDatabase consumableDatabase;
        [SerializeField] private ThemeDatabase themeDatabase;

        [SerializeField] private WorldCurver worldCurver;

        private const int SAFE_SEGMENT_COUNT = 3;
        private const int MAX_SEGMENT_COUNT = 12;
        private const float DELETE_DISTANCE = 45f;
        private const float MAX_LENGTH_DISTANCE = 100f;

        private bool _moveable = false;
        private bool _canSpawnNew = true;
        private float _moveSpeed = 0;
        private float _switchBundleElapsedTime = 0;
        private float _switchBundleDuration = 0;
        private float _timeSinceStartPowerup = 0;
        private float _timeSincePremiumStart = 0;
        private float _pingpongTimeX = 0;
        private float _pingpongXLerp = 0;
        private float _distance = 0;
        private float _score = 0;
        private int _multiply = 1;
        private int _bundleRange = 0;

        private Vector3 _spawnPosition;
        private Coin _coinPrefab, _premiumCoinPrefab;
        private List<TrackSegment> _trackSegments = new List<TrackSegment>();

        public float RunDistance => _distance;
        public float Score => _score;

        private void Awake()
        {
            _coinPrefab = consumableDatabase.Coin;
            _premiumCoinPrefab = consumableDatabase.PremiumCoin;

            SimplePool.Preload(_coinPrefab.gameObject, 100, CurrencyContainer.InstanceTransform);
            SimplePool.Preload(_premiumCoinPrefab.gameObject, 20, CurrencyContainer.InstanceTransform);

            SpawnTrackSeqment().Forget();
            SetMoveSpeed(10);

            GetRandomSwitchBundleTime();
        }

        private void Update()
        {
            UpdateTime();
            UpdateBundleTime();
            CalculateDistance();

            if (_trackSegments.Count > 0)
            {
                if (characterController.transform.position.z - _trackSegments[0].transform.position.z >= DELETE_DISTANCE)
                {
                    if (_canSpawnNew)
                        ContinueSpawn().Forget();
                }

                MoveForward();
            }
        }

        public void SetTracksActive(bool active)
        {
            for (int i = 0; i < _trackSegments.Count; i++)
            {
                _trackSegments[i].SetTrackActive(active);
            }
        }

        public void StartTrack()
        {
            SetMoveSpeed(10);
            SetMoveable(true);
        }

        private void UpdateTime()
        {
            _timeSinceStartPowerup += Time.deltaTime;
            _timeSincePremiumStart += Time.deltaTime;

            if (_moveable)
            {
                _pingpongTimeX += Time.deltaTime * 0.15f;
                _pingpongXLerp = Mathf.PingPong(_pingpongTimeX, 1);

                worldCurver.curveStrengthX = Mathf.Lerp(0.002f, -0.002f, _pingpongXLerp);
            }
        }

        private void UpdateBundleTime()
        {
            if (_moveable)
            {
                _switchBundleElapsedTime += Time.deltaTime;
                if(_switchBundleElapsedTime > _switchBundleDuration)
                {
                    _switchBundleElapsedTime = 0;
                    int nextBundle = Random.Range(0, 4);
                    
                    if (nextBundle == _bundleRange)
                        _bundleRange = (nextBundle + 1) % 4;

                    GetRandomSwitchBundleTime();
                }
            }
        }

        private void GetRandomSwitchBundleTime()
        {
            _switchBundleDuration = Random.Range(9f, 12f);
        }

        private async UniTask ContinueSpawn()
        {
            _trackSegments[0].Cleanup();
            _trackSegments.RemoveAt(0);
            await SpawnTrackSeqment();
        }

        public void SetMultiply(int m)
        {
            _multiply = m;
        }

        public void SetMoveable(bool moveable)
        {
            _moveable = moveable;
        }

        public void ResetTrackSpeedProgress()
        {
            SetMoveSpeed(10);
        }

        public void SetMoveSpeed(float moveSpeed)
        {
            float speed = Mathf.Clamp(moveSpeed, minSpeed, maxSpeed);
            _moveSpeed = speed;
        }

        private void MoveForward()
        {
            if (_moveable)
            {
                characterController.transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime, Space.World);

                if (characterController.transform.position.z >= MAX_LENGTH_DISTANCE)
                {
                    if (_trackSegments.Count >= MAX_SEGMENT_COUNT)
                    {
                        _canSpawnNew = false;
                        float z = characterController.transform.position.z;

                        for (int i = 0; i < _trackSegments.Count; i++)
                        {
                            _trackSegments[i].transform.position += Vector3.back * z;
                        }

                        characterController.transform.position = Vector3.zero;
                        _canSpawnNew = true;
                    }
                }
            }
        }

        private async UniTask SpawnTrackSeqment()
        {
            while (_trackSegments.Count < MAX_SEGMENT_COUNT)
            {
                if (_trackSegments.Count == 0)
                    _spawnPosition = Vector3.zero;

                else
                {
                    TrackSegment lastSegment = _trackSegments[_trackSegments.Count - 1];
                    _spawnPosition = lastSegment.Path.ExitPoint.position + new Vector3(0, 0, 4.5f);
                }

                TrackSegment seqment = await CreateSegment(themeDatabase.GetRandomBundle(_bundleRange), _spawnPosition);
                
                if (_trackSegments.Count >= SAFE_SEGMENT_COUNT)
                {
                    SpawnObstacle(seqment);
                    SpawnCurrency(seqment);
                    SpawnPowerup(seqment);
                }

                _trackSegments.Add(seqment);
            }
        }

        private async UniTask<TrackSegment> CreateSegment(AssetReference asset, Vector3 position)
        {
            AsyncOperationHandle<GameObject> handle = asset.InstantiateAsync(position, Quaternion.identity, TrackContainer.InstanceTransform);
            
            if (!handle.IsDone)
                await handle;
            
            if(handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result.GetComponent<TrackSegment>();
            }

            return null;
        }

        private void SpawnObstacle(TrackSegment trackSegment)
        {
            trackSegment.SpawnObstacle().Forget();
        }

        private void SpawnCurrency(TrackSegment trackSegment)
        {
            bool hasSpawnPremium;
            trackSegment.GetSegmentLength();
            int randStart = Random.Range(0, 5); 
            int randEnd = (int)trackSegment.SegmentLength - Random.Range(6, 10);
            float premiumChance = Mathf.Clamp01(Mathf.Floor(_timeSincePremiumStart) * 0.05f * 0.005f);

            trackSegment.SpawnCurrency(consumableDatabase.Coin, consumableDatabase.PremiumCoin, out hasSpawnPremium
                                       , premiumChance, randStart, randEnd);
            if (hasSpawnPremium)
                _timeSincePremiumStart = 0;
        }

        private void SpawnPowerup(TrackSegment trackSegment)
        {
            float powerupChance = Mathf.Clamp01(Mathf.Floor(_timeSinceStartPowerup) * 0.05f * 0.01f);
            if(Random.value <= powerupChance)
            {
                bool isFullLife = GameplayManager.Instance.PlayerObserver.Life == PlayerConfig.Current.MaxLife;
                Powerup powerup = isFullLife ? consumableDatabase.GetRandomPowerupWithoutHeart(PlayerConfig.Current.CurrentTheme) 
                                             : consumableDatabase.GetRandomPowerupWithHeart(PlayerConfig.Current.CurrentTheme);
                trackSegment.SpawnPowerup(powerup);
                _timeSinceStartPowerup = 0;
            }
        }

        private void CalculateDistance()
        {
            if (_moveable)
            {
                _distance += Time.deltaTime * _moveSpeed / 3;
                _score += Time.deltaTime * _moveSpeed * GameDataManager.CurrentData.Multiplier * _multiply;
            }
        }
    }
}
