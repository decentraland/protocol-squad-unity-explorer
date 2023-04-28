
using DCL.ECS7;
using DCL.ECSComponents;
using DCLRuntime.ComponentHandlers;
using UnityEngine;

namespace DCLRuntime
{
    public partial class ComponentManager
    {
        
        private void HandleComponent(GameObject entity, int componentId, int timestamp, object data)
        {
            switch (componentId)
            {
                case ComponentID.TRANSFORM:
                    var transformData = TransformData.FromData((byte[])data);
                    transformData.ApplyOn(entity.transform);
                    break;
                case ComponentID.MESH_RENDERER:
                    var meshData = ProtoSerialization.Deserialize<PBMeshRenderer>(data);
                    meshData.Apply(entity);
                    break;
                default:
                    Debug.LogError($"not supported component {componentId}");
                    break;
            }
        }
    }
}