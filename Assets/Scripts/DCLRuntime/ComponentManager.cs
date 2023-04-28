using System;
using DCL.CRDT;
using DCL.ECS7;
using DCL.ECSComponents;
using DCLRuntime.ComponentHandlers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DCLRuntime
{
    public class ComponentManager : IDisposable
    {
        private readonly SceneRoot _sceneRoot = SceneRoot.Create();


        public void HandleMessage(CrdtMessage message)
        {
            if (message.Type == CrdtMessageType.PUT_COMPONENT)
            {
                var entity = _sceneRoot.GetCreateEntity(message.EntityId);
                HandleComponent(entity, message.ComponentId, message.Timestamp, message.Data);
            }
        }
        
        private void HandleComponent(GameObject entity, int componentId, int timestamp, object data)
        {
            switch (componentId)
            {
                case ComponentID.TRANSFORM:
                    var transformData = TransformData.FromData((byte[])data);
                    if (transformData.parentId != 0)
                    {
                        var parent = _sceneRoot.GetCreateEntity(transformData.parentId);
                        transformData.ApplyWithParentOn(entity.transform, parent);
                    }
                    else
                    {
                        transformData.ApplyOn(entity.transform);
                    }
                    break;
                case ComponentID.MESH_RENDERER: 
                    var meshRendererData = ProtoSerialization.Deserialize<PBMeshRenderer>(data);
                    meshRendererData.ApplyOn(entity);
                    break;
                case ComponentID.MESH_COLLIDER:
                    var meshColliderData = ProtoSerialization.Deserialize<PBMeshCollider>(data);
                    meshColliderData.ApplyOn(entity);
                    break;
                default:
                    Debug.LogError($"not supported component {componentId}");
                    break;
            }
        }
        
        
        public void Dispose()
        {
            Object.Destroy(_sceneRoot.gameObject);
        }
    }
}