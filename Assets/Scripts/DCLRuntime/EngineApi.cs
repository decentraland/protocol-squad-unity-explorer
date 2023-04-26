using Cysharp.Threading.Tasks;
using JSInterop;
using UnityEngine;

namespace DCLRuntime
{
    public class EngineApi : IEngineApi
    {
        public UniTask crdtSendToRenderer(dynamic data)
        {
            var length = (int)data.length;
            Debug.Log("come message from here");
            for (int i = 0; i < length; i++)
            {
                Debug.Log("message " + data[i]);
            }

            Debug.Log("-------end---------");
            return UniTask.CompletedTask;
        }

        public async UniTask<object> crdtGetState(dynamic data)
        {
            var length = (int)data.length;
            for (int i = 0; i < length; i++)
            {
                Debug.Log("message " + data[i]);
            }
            return null;
        }

    }
}