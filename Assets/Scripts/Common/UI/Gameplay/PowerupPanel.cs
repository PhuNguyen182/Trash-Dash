using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.DataStructs.Messages;
using UniRx;

namespace TrashDash.Scripts.Common.UI.Gameplay
{
    public class PowerupPanel : MonoBehaviour
    {
        [SerializeField] private PowerupTimerCell magnetTimer;
        [SerializeField] private PowerupTimerCell multiplyTimer;
        [SerializeField] private PowerupTimerCell invincibleTimer;

        private void Awake()
        {
            MessageBroker.Default.Receive<PowerupTimerMessage>()
                                 .Subscribe(Timer)
                                 .AddTo(this);
            MessageBroker.Default.Receive<FreePowerupUI>()
                                 .Subscribe(_ => FreePowerups())
                                 .AddTo(this);
        }

        private void Timer(PowerupTimerMessage message)
        {
            PowerupTimerCell timerCell = null;

            switch (message.Powerup)
            {
                case PowerupEnum.Magnet:
                    timerCell = magnetTimer;
                    break;
                case PowerupEnum.Multiply:
                    timerCell = multiplyTimer;
                    break;
                case PowerupEnum.Invincible:
                    timerCell = invincibleTimer;
                    break;
            }

            if (!timerCell.isActiveAndEnabled)
                timerCell.gameObject.SetActive(true);

            timerCell.ShowTimer(message.Duration);
        }

        private void FreePowerups()
        {
            magnetTimer.Free();
            multiplyTimer.Free();
            invincibleTimer.Free();
        }
    }
}
