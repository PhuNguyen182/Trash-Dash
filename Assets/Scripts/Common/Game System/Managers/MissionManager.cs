using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.DataStructs.Datas;
using TrashDash.Scripts.Common.DataStructs.Messages;
using TrashDash.Scripts.Common.Databases;
using MyEasySaveSystem;
using UniRx;

namespace TrashDash.Scripts.Common.GameSystem.Managers
{
    public class MissionManager : Singleton<MissionManager>
    {
        [SerializeField] private MissionDatabase missionDatabase;

        private const string MISSION_INDEX_KEY = "CurrentMissionIndex";

        private int _currentMissionIndex = 0;
        private Missions _currentMission;

        public int CurrentMissionIndex
        {
            get
            {
                _currentMissionIndex = PlayerPrefs.GetInt(MISSION_INDEX_KEY, 0);
                return _currentMissionIndex;
            }
            set
            {
                _currentMissionIndex = value;
                PlayerPrefs.SetInt(MISSION_INDEX_KEY, value);
            }
        }

        public Missions CurrentMission
        {
            get
            {
                if (_currentMission.MissionDatas == null)
                    _currentMission = BasicSaveSystem<Missions>.Load("CurrentMissions");
                
                if (_currentMission.MissionDatas == null)
                    _currentMission = GetCurrentMissions();

                return _currentMission;
            }
            private set => _currentMission = value;
        }

        protected override void OnAwake()
        {
            MessageBroker.Default.Receive<SkipMissionMessage>()
                                 .Subscribe(value => SkipMission(value.MissionID))
                                 .AddTo(this);
        }

        public Missions GetCurrentMissions()
        {
            if (CurrentMissionIndex < missionDatabase.Missions.Length)
                return missionDatabase.Missions[CurrentMissionIndex];

            return default;
        }

        private void SkipMission(string missionId)
        {
            for (int i = 0; i < CurrentMission.MissionDatas.Length; i++)
            {
                if (string.CompareOrdinal(CurrentMission.MissionDatas[i].MissionID, missionId) == 0)
                {
                    MessageBroker.Default.Publish(new UpdateMissionMessage
                    {
                        ID = missionId,
                        IsDone = true
                    });

                    CurrentMission.MissionDatas[i].IsDone = true;
                }
            }

            BasicSaveSystem<Missions>.Save("CurrentMissions", CurrentMission);
            
            if (HasMissionComplete(CurrentMission))
            {
                CurrentMissionIndex += 1;
                CurrentMission = GetCurrentMissions();
                BasicSaveSystem<Missions>.Save("CurrentMissions", CurrentMission);

                MessageBroker.Default.Publish(new UpdateNewMission { });
                GameDataManager.AddMultiplier();
            }
        }

        private bool HasMissionComplete(Missions missions)
        {
            int count = 0;
            int length = missions.MissionDatas.Length;

            for (int i = 0; i < length; i++)
            {
                if (missions.MissionDatas[i].IsDone)
                    count += 1;
            }

            return count == length;
        }
    }
}
