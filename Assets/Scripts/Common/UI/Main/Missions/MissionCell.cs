using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrashDash.Scripts.Common.DataStructs.Datas;
using TrashDash.Scripts.Common.GameSystem.Managers;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.DataStructs.Messages;
using TMPro;
using UniRx;

namespace TrashDash.Scripts.Common.UI.Main.Mission
{
    public class MissionCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text missionName;
        [SerializeField] private TMP_Text missionProgress;
        [SerializeField] private TMP_Text priceToSkipText;
        [SerializeField] private Image missionBackground;
        [SerializeField] private Button skipMissionButton;

        private int _priceToSkip = 0;
        private int _maxQuantity = 0;
        private string _missionID = "";

        private void Awake()
        {
            skipMissionButton.onClick.AddListener(PayToSkip);

            MessageBroker.Default.Receive<UpdateMissionMessage>()
                                 .Subscribe(value =>
                                 {
                                     if (string.CompareOrdinal(value.ID, _missionID) == 0)
                                         SetDoneState(value.IsDone);
                                 })
                                 .AddTo(this);
        }

        public void SetMission(MissionData missionData)
        {
            _missionID = missionData.MissionID;
            missionName.text = missionData.MissionName;
            _maxQuantity = missionData.MaxQuantity;
            _priceToSkip = missionData.PriceToSkip;
            priceToSkipText.text = $"{_priceToSkip}";
        }

        public void UpdateMission(int value = 0)
        {
            missionProgress.text = $"{value} / {_maxQuantity}";
        }

        private void SetDoneState(bool isDone)
        {
            missionName.color = !isDone ? new Color(0.3f, 0.57f, 1, 1) : new Color(1, 1, 1, 1);
            missionBackground.color = isDone ? new Color(0.3f, 0.57f, 1, 1) : new Color(1, 1, 1, 1);
        }

        private void PayToSkip()
        {
            GameDataManager.AddCoins(_priceToSkip, CurrencyUsage.Spend);
            MessageBroker.Default.Publish(new SkipMissionMessage { MissionID = _missionID });
        }
    }
}
