using TrashDash.Scripts.Common.Enumerations;

namespace TrashDash.Scripts.Common.DataStructs.Messages
{
    public struct PowerupTimerMessage
    {
        public float Duration;
        public PowerupEnum Powerup;
    }

    public struct MultiplyPowerupMessage
    {
        public bool IsCompleted;
    }

    public struct FreePowerupUI
    {

    }
}
