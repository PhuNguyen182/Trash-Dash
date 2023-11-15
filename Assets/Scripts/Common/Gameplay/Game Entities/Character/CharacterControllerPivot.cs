using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Character
{
    public class CharacterControllerPivot : MonoBehaviour
    {
        [SerializeField] private CharacterInput characterInput;
        [SerializeField] private CharacterAnimation characterAnimation;
        [SerializeField] private CharacterCollider characterCollider;
        [SerializeField] private CharacterMovement characterMovement;
        [SerializeField] private CharacterDecoration characterDecoration;
        [SerializeField] private SkyThemeChanger skyThemeChanger;

        public CharacterInput CharacterInput => characterInput;
        public CharacterCollider CharacterCollider => characterCollider;
        public CharacterAnimation CharacterAnimation => characterAnimation;
        public CharacterMovement CharacterMovement => characterMovement;
        public CharacterDecoration CharacterDecoration => characterDecoration;
        public SkyThemeChanger SkyThemeChanger => skyThemeChanger;
    }
}
