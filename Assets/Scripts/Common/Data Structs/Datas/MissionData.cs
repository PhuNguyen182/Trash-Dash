using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashDash.Scripts.Common.DataStructs.Datas
{
    [Serializable]
    public class MissionData
    {
        public string MissionID;
        public string MissionName;
        public int MaxQuantity;
        public int PriceToSkip;

        [HideInInspector]
        public bool IsDone;
    }

    [Serializable]
    public struct Missions
    {
        public MissionData[] MissionDatas;
    }
}
