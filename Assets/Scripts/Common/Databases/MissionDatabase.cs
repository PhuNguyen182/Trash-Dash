using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.DataStructs.Datas;

namespace TrashDash.Scripts.Common.Databases
{
    [CreateAssetMenu(fileName = "Mission Database", menuName = "Scriptable Objects/Databases/Mission Database", order = 3)]
    public class MissionDatabase : ScriptableObject
    {
        [SerializeField] private Missions[] missions;

        public Missions[] Missions => missions;
    }
}
