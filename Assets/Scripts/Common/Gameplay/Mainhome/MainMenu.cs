using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.UI.Main;
using UnityEngine;

namespace TrashDash.Scripts.Common.Gameplay.Mainhome
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private CharacterPreview characterPreview;

        public CharacterPreview CharacterPreview => characterPreview;

        public static MainMenu Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}
