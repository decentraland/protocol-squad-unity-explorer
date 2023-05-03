using System;
using DCL.CRDT;
using DCL.ECS7;
using DCL.ECSComponents;
using DCLRuntime.ComponentHandlers;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace DCLRuntime
{
    public class ComponentManager : IDisposable
    {
        private readonly SceneEntityManager _sceneEntityManager;


        [Inject]
        public ComponentManager(SceneEntityManager sceneEntityManager)
        {
            _sceneEntityManager = sceneEntityManager;
        }

        public void Dispose()
        {
            Object.Destroy(_sceneEntityManager.gameObject);
        }


        public void HandleMessage(CrdtMessage message)
        {
            if (message.Type == CrdtMessageType.PUT_COMPONENT)
            {
                var entity = _sceneEntityManager.GetCreateEntity(message.EntityId);
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
                        var parent = _sceneEntityManager.GetCreateEntity(transformData.parentId);
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
    }
}