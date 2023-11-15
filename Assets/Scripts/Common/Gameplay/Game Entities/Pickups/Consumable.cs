using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Interfaces;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Pickups
{
    public abstract class Consumable : MonoBehaviour, IPickup
    {
        public abstract void ReleasePickUp();

        public abstract void StartPickup();
    }
}
