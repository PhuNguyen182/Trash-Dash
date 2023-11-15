using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.Enumerations;
using TrashDash.Scripts.Common.Interfaces;
using UnityEngine;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Obstacles
{
    public abstract class BaseObstacle : MonoBehaviour, IObstacle
    {
        [SerializeField] protected ObstacleType obstacleType;
        [SerializeField] protected Collider obstacleCollider;

        public abstract void HitObstacle();
        
    }
}
