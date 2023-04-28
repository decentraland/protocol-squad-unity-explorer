using System;
using DCL.CRDT;
using DCL.ECS7;
using Object = UnityEngine.Object;

namespace DCLRuntime
{
    public partial class ComponentManager : IDisposable
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
        
        
        public void Dispose()
        {
            Object.Destroy(_sceneRoot.gameObject);
        }
    }
}