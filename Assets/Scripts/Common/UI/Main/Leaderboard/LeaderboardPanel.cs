using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrashDash.Scripts.Common.Interfaces;
using TrashDash.Scripts.Common.GameSystem.Managers;
using Cysharp.Threading.Tasks;
using TrashDash.Scripts.Common.DataStructs.Datas;

namespace TrashDash.Scripts.Common.UI.Main.Leaderboard
{
    public class LeaderboardPanel : MonoBehaviour, IPanelUI
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private LeaderboardCell leaderboardCell;
        [SerializeField] private Transform cellContainer;

        private List<LeaderboardCell> leaderboardCells = new List<LeaderboardCell>();

        public async UniTask Close()
        {
            await UniTask.CompletedTask;
            gameObject.SetActive(false);
        }

        public async UniTask OnAppear()
        {
            
        }

        public void OnCLose()
        {
            
        }

        private void Awake()
        {
            closeButton.onClick.AddListener(() => Close().Forget());
            SimplePool.Preload(leaderboardCell.gameObject, 10, cellContainer);
        }

        private void OnEnable()
        {
            ShowLeaderboardData();
        }

        private void ShowLeaderboardData()
        {
            for (int i = 0; i < leaderboardCells.Count; i++)
            {
                SimplePool.Despawn(leaderboardCells[i].gameObject);
            }

            leaderboardCells.Clear();

            GameData gameData = GameDataManager.CurrentData;
            for (int i = 0; i < gameData.HighScores.Count; i++)
            {
                LeaderboardCell cell = SimplePool.Spawn(leaderboardCell);
                cell.transform.position = cellContainer.position;
                cell.transform.localScale = Vector3.one;
                cell.transform.SetParent(cellContainer, true);

                cell.SetLeaderboardDataCell(i, gameData.HighScores[i]);
                leaderboardCells.Add(cell);
            }
        }
    }
}
