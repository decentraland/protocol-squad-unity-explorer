using DCL.ECSComponents;
using UnityEngine;

namespace DCLRuntime.ComponentHandlers
{
    internal static class MeshColliderHandler
    {
        public static void Apply(this PBMeshCollider pbMeshCollider, GameObject entity)
        {

            switch (pbMeshCollider.MeshCase)
            {
                case PBMeshCollider.MeshOneofCase.Box:
                    entity.AddComponent<BoxCollider>();
                    break;
                case PBMeshCollider.MeshOneofCase.Plane:
                    BoxCollider box = entity.AddComponent<BoxCollider>();
                    box.size = new Vector3(1, 1, 0.01f);
                    break;
                default:
                    Debug.LogError($"Not supported collider {pbMeshCollider.MeshCase}");
                    break;
            }
        }
    }
}