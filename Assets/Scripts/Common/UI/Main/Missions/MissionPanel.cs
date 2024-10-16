using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.Interfaces;
using TrashDash.Scripts.Common.GameSystem.Managers;
using UnityEngine;
using UnityEngine.UI;
using TrashDash.Scripts.Common.DataStructs.Datas;

namespace TrashDash.Scripts.Common.UI.Main.Mission
{
    public class MissionPanel : MonoBehaviour, IPanelUI
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private MissionCell missionCell;
        [SerializeField] private Transform cellContainer;

        private List<MissionCell> missionCells = new List<MissionCell>();

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
            SimplePool.Preload(missionCell.gameObject, 3, cellContainer);
        }

        private void OnEnable()
        {
            ShowMissions();
        }

        private void ShowMissions()
        {
            for (int i = 0; i < missionCells.Count; i++)
            {
                SimplePool.Despawn(missionCells[i].gameObject);
            }

            missionCells.Clear();

            Missions missions = MissionManager.Instance.CurrentMission;

            for (int i = 0; i < missions.MissionDatas.Length; i++)
            {
                MissionCell mission = SimplePool.Spawn(missionCell);

                mission.transform.position = cellContainer.position;
                mission.transform.localScale = Vector3.one;
                mission.transform.SetParent(cellContainer, true);

                mission.SetMission(missions.MissionDatas[i]);
                missionCells.Add(mission);
            }
        }
    }
}
