using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.Databases;
using TrashDash.Scripts.Common.Enumerations;
using UnityEngine;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Character
{
    public class SkyThemeChanger : MonoBehaviour
    {
        [SerializeField] private MeshFilter skyMesh;
        [SerializeField] private ThemeDatabase themeDatabase;

        public void ChangeSky(Theme theme)
        {
            Mesh sky = themeDatabase.GetThemeSky(theme).Sky;
            skyMesh.mesh = sky;
        }
    }
}
