using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TrashDash.Scripts.Common.UI.Main.Leaderboard
{
    public class LeaderboardCell : MonoBehaviour
    {
        [SerializeField] private Image cellBackground;
        [SerializeField] private TMP_Text rankText;
        [SerializeField] private TMP_Text scoreText;

        public void SetLeaderboardDataCell(int rank, int score)
        {
            rankText.text = $"{rank}";
            scoreText.text = $"{score}";
            cellBackground.color = rank % 2 == 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 0.5f);
        }
    }
}
