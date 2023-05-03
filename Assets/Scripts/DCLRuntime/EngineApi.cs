using System;
using Cysharp.Threading.Tasks;
using DCL.CRDT;
using JSInterop;
using Microsoft.ClearScript.JavaScript;
using VContainer;

namespace DCLRuntime
{
    public class EngineApi : IEngineApi
    {
        private readonly ComponentManager _componentManager;

        [Inject]
        public EngineApi(ComponentManager componentManager)
        {
            _componentManager = componentManager;
        }

        public async UniTask crdtSendToRenderer(dynamic data)
        {
            await UniTask.SwitchToMainThread();
            HandleData(data);

            await UniTask.SwitchToThreadPool();
        }

        public async UniTask<object> crdtGetState(dynamic data)
        {
            await UniTask.SwitchToMainThread();
            HandleData(data);
            await UniTask.SwitchToThreadPool();
            return null;
        }


        private void HandleData(dynamic data)
        {
            if ((int)data.length <= 0) return;
            var buffer = data.buffer;

            var bytes = ((IArrayBuffer)buffer).GetBytes();
            var memory = new ReadOnlyMemory<byte>(bytes);
            using var iterator = CRDTDeserializer.DeserializeBatch(memory);
            while (iterator.MoveNext())
                if (iterator.Current is CrdtMessage msg)
                    _componentManager.HandleMessage(msg);
        }
    }
}