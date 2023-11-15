using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Miscs
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Transform rotator;
        [SerializeField] private float rotateSpeed = 10f;

        private void FixedUpdate()
        {
            rotator.Rotate(Vector3.right * rotateSpeed);
        }
    }
}
