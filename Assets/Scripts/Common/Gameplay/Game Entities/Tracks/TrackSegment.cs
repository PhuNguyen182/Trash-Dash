using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.Gameplay.GameEntities.Pickups;
using TrashDash.Scripts.Common.Interfaces;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Tracks
{
    public class TrackSegment : MonoBehaviour
    {
        [SerializeField] private SegmentPath path;
        [SerializeField] private Transform collectibleContainer;
        [SerializeField] private Transform obstacleContainer;
        [SerializeField] private Transform powerupContainer;

        [Header("Spawn Obstacles")]
        [SerializeField] private AssetReference[] spawnableObstacles;
        [SerializeField] private AssetReference[] spawnableAllLaneObstacles;
        [Range(-2f, 2f)] public float[] positionScale;

        private List<GameObject> _obstacles = new List<GameObject>();
        private Dictionary<int, Coin> _coins = new Dictionary<int, Coin>();
        private int[] _currencyLane = new int[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 5, 5, 6, 6, 7 };

        public SegmentPath Path => path;
        public float SegmentLength { get; private set; }

        public void SetTrackActive(bool active)
        {
            for (int i = 0; i < _obstacles.Count; i++)
            {
                if (_obstacles[i].TryGetComponent<IObstacleMoveable>(out var patroller))
                {
                    patroller.SetMoveable(active);
                }
            }
        }

        public async UniTask SpawnObstacle()
        {
            _obstacles.Clear();

            if(positionScale.Length > 0)
            {
                float random = Random.value;
                if (random >= 0.5f)
                {
                    int rand = Random.Range(0, spawnableAllLaneObstacles.Length);
                    Vector3 pos = obstacleContainer.TransformPoint(Vector3.zero);
                    GameObject obstacle = await CreateObstacle(spawnableAllLaneObstacles[rand], pos);
                    _obstacles.Add(obstacle);
                }

                else
                {
                    for (int i = 0; i < positionScale.Length; i++)
                    {
                        int rand = Random.Range(0, spawnableObstacles.Length);
                        Vector3 pos = obstacleContainer.TransformPoint(new Vector3(positionScale[i], 0, 0));
                        GameObject obstacle = await CreateObstacle(spawnableObstacles[rand], pos);
                        _obstacles.Add(obstacle);
                    }
                }
            }
        }

        public void GetSegmentLength()
        {
            SegmentLength = path.ExitPoint.position.z - path.EntryPoint.position.z;
        }

        public void SpawnCurrency(Coin coin, Coin premiumCoin, out bool hasSpawnPremium, float premiumChance = 0f, 
            int min = 0, int max = 0)
        {
            int startIndex, endIndex;

            float chance;
            float rand = Random.value;

            bool spawnPremium = false;
            bool spawnPremium1 = false;
            bool spawnPremium2 = false;
            bool hasPremium = false;

            if (rand <= 0.5f)
            {
                _coins.Clear();
                startIndex = min <= 0 ? 0 : min;
                endIndex = max >= SegmentLength ? (int)SegmentLength : max;
                int laneType = _currencyLane[Random.Range(0, _currencyLane.Length)];
                PickupLane pickupLane = (PickupLane)laneType;

                for (int i = startIndex; i < endIndex; i++)
                {
                    switch (pickupLane)
                    {
                        case PickupLane.Left:
                            chance = spawnPremium ? 0 : premiumChance;
                            SpawnCurrencyAtLine(coin, premiumCoin, -1.5f, i, chance, out spawnPremium);
                            break;
                        case PickupLane.Middle:
                            chance = spawnPremium1 ? 0 : premiumChance;
                            SpawnCurrencyAtLine(coin, premiumCoin, 0, i, chance, out spawnPremium1);
                            break;
                        case PickupLane.Right:
                            chance = spawnPremium2 ? 0 : premiumChance;
                            SpawnCurrencyAtLine(coin, premiumCoin, 1.5f, i, chance, out spawnPremium2);
                            break;
                        case PickupLane.LeftMid:
                            SpawnCurrencyAtLine(coin, premiumCoin, -1.5f, i, 0, out spawnPremium);
                            SpawnCurrencyAtLine(coin, premiumCoin, 0, i, 0, out spawnPremium1);
                            break;
                        case PickupLane.RightMid:
                            SpawnCurrencyAtLine(coin, premiumCoin, 0, i, 0, out spawnPremium1);
                            SpawnCurrencyAtLine(coin, premiumCoin, 1.5f, i, 0, out spawnPremium2);
                            break;
                        case PickupLane.LeftRight:
                            SpawnCurrencyAtLine(coin, premiumCoin, -1.5f, i, 0, out spawnPremium);
                            SpawnCurrencyAtLine(coin, premiumCoin, 1.5f, i, 0, out spawnPremium2);
                            break;
                        case PickupLane.AllLane:
                            SpawnCurrencyAtLine(coin, premiumCoin, -1.5f, i, 0, out spawnPremium);
                            SpawnCurrencyAtLine(coin, premiumCoin, 0, i, 0, out spawnPremium1);
                            SpawnCurrencyAtLine(coin, premiumCoin, 1.5f, i, 0, out spawnPremium2);
                            break;
                    }

                    if (spawnPremium || spawnPremium1 || spawnPremium2)
                        hasPremium = true;
                }

                hasSpawnPremium = hasPremium;
            }

            else
                hasSpawnPremium = false;
        }

        public void SpawnPowerup(Powerup powerup)
        {
            float z = Random.value >= 0.5f 
                      ? SegmentLength * 0.75f 
                      : SegmentLength * 0.25f;

            int laneType = Random.Range(0, 100) % 4;
            if (laneType == 0)
                laneType += 1;

            PickupLane pickupLane = (PickupLane)laneType;
            Vector3 spawnPosition = Vector3.zero;

            switch (pickupLane)
            {
                case PickupLane.Left:
                    spawnPosition = powerupContainer.TransformPoint(new Vector3(-1.5f, 0.5f, z));
                    break;
                case PickupLane.Middle:
                    spawnPosition = powerupContainer.TransformPoint(new Vector3(0, 0.5f, z));
                    break;
                case PickupLane.Right:
                    spawnPosition = powerupContainer.TransformPoint(new Vector3(1.5f, 0.5f, z));
                    break;
            }

            SimplePool.Spawn(powerup, powerupContainer, spawnPosition, Quaternion.identity);
        }

        private void SpawnCurrencyAtLine(Coin coin, Coin premiumCoin, float x, int z, float premiumChance, out bool hasSpawnPremium)
        {
            Coin c;
            Vector3 localPos = new Vector3(x, 0.5f, z * 1.5f - 4);

            if (Random.value < premiumChance)
            {
                hasSpawnPremium = true;
                c = SimplePool.Spawn(premiumCoin);
            }

            else
            {
                hasSpawnPremium = false;
                c = SimplePool.Spawn(coin);
            }

            c.transform.SetParent(collectibleContainer);
            c.transform.localPosition = localPos;
            c.OnFreeCoin = RemoveCoin;
            _coins.Add(c.Key, c);
        }

        private async UniTask<GameObject> CreateObstacle(AssetReference asset, Vector3 position)
        {
            AsyncOperationHandle<GameObject> handle = asset.InstantiateAsync(position, Quaternion.identity, obstacleContainer);

            if (!handle.IsDone)
                await handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            return null;
        }

        private void RemoveCoin(int k)
        {
            _coins.Remove(k);
        }

        public void Cleanup()
        {
            foreach (KeyValuePair<int, Coin> kvp in _coins)
            {
                _coins[kvp.Key].Free(true);
            }

            if (powerupContainer.childCount > 0)
                SimplePool.Despawn(powerupContainer.GetChild(0).gameObject, PowerupContainer.InstanceTransform);

            for(int i = 0; i < _obstacles.Count; i++)
            {
                Addressables.ReleaseInstance(_obstacles[i]);
            }

            _coins.Clear();
            _obstacles.Clear();

            Addressables.ReleaseInstance(this.gameObject);
        }
    }
}
