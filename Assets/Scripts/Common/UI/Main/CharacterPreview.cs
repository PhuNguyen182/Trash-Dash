using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.Databases;
using TrashDash.Scripts.Common.DataStructs.Datas;
using TrashDash.Scripts.Common.GameSystem.Config;
using UnityEngine;

namespace TrashDash.Scripts.Common.UI.Main
{
    public class CharacterPreview : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private SkinnedMeshRenderer characterRenderer;
        [SerializeField] private SkinnedMeshRenderer accessoriesRenderer;
        [SerializeField] private ShopItemDatabase itemDatabase;

        [Header("Sky Preview")]
        [SerializeField] private MeshFilter skyMesh;
        [SerializeField] private MeshFilter skyCircleMesh;

        private static int _startHash = Animator.StringToHash("StartState");
        private static int _idleHash = Animator.StringToHash("RandomIdle");

        private void Start()
        {
            if(PlayerConfig.Current != null)
            {
                int decor = PlayerConfig.Current.Decor;
                if (decor != -1)
                {
                    GameObject obj = itemDatabase.Accessories[decor].Preview;
                    if (obj.TryGetComponent<SkinnedMeshRenderer>(out var smr))
                    {
                        ShowAccessories(smr.sharedMesh, smr.sharedMaterial, obj.transform.localPosition, obj.transform.localRotation);
                    }
                }
            }

            PlayIdle();
        }

        public void PlayIdle()
        {
            characterAnimator.Play(_startHash);
            int rand = Random.Range(0, 5);
            characterAnimator.SetInteger(_idleHash, rand);
        }

        public void ShowCharacter(Mesh characterMesh, Material characterMaterial, Vector3 position, Quaternion rotation)
        {
            transform.GetChild(0).localPosition = position;
            transform.GetChild(0).localRotation = rotation;
            characterRenderer.sharedMesh = characterMesh;
            characterRenderer.sharedMaterial = characterMaterial;
        }

        public void ShowAccessories(Mesh accessoryMesh, Material accessoryMaterial, Vector3 position, Quaternion rotation)
        {
            accessoriesRenderer.transform.localPosition = position;
            accessoriesRenderer.transform.localRotation = rotation;
            accessoriesRenderer.sharedMesh = accessoryMesh;
            accessoriesRenderer.sharedMaterial = accessoryMaterial;
        }

        public void ShowPreviewSky(SkyData sky)
        {
            skyMesh.mesh = sky.Sky;
            skyCircleMesh.mesh = sky.SkyCircle;
        }
    }
}
