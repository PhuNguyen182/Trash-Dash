using System.Collections;
using System.Collections.Generic;
using TrashDash.Scripts.Common.Databases;
using TrashDash.Scripts.Common.GameSystem.Config;
using UnityEngine;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Character
{
    public class CharacterDecoration : MonoBehaviour
    {
        [SerializeField] private ShopItemDatabase itemDatabase;
        [SerializeField] private SkinnedMeshRenderer accessoriesRenderer;

        public void Decorate(int decor)
        {
            if (decor == -1)
                return;

            GameObject obj = itemDatabase.Accessories[decor].Preview;
            if (obj.TryGetComponent<SkinnedMeshRenderer>(out var smr))
            {
                ShowAccessories(smr.sharedMesh, smr.sharedMaterial, obj.transform.localPosition, obj.transform.localRotation);
            }
        }

        private void ShowAccessories(Mesh accessoryMesh, Material accessoryMaterial, Vector3 position, Quaternion rotation)
        {
            accessoriesRenderer.transform.localPosition = position;
            accessoriesRenderer.transform.localRotation = rotation;
            accessoriesRenderer.sharedMesh = accessoryMesh;
            accessoriesRenderer.sharedMaterial = accessoryMaterial;
        }
    }
}
