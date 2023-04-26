using System;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using DCL.CRDT;
using JSInterop;
using Microsoft.ClearScript.JavaScript;
using UnityEngine;
using UnityEngine.Assertions;

namespace DCLRuntime
{
    public class EngineApi : IEngineApi
    {
       public UniTask crdtSendToRenderer(dynamic data)
        {
            HandleData(data);
            return UniTask.CompletedTask;
        }

        public async UniTask<object> crdtGetState(dynamic data)
        {
            HandleData(data);
            return null;
        }


        private void HandleData(dynamic data)
        {
            
            if ((int)data.length > 0)
            {
                var buffer = data.buffer;
                
                var bytes = ((IArrayBuffer) buffer).GetBytes();
                var memory = new ReadOnlyMemory<byte>(bytes);
                using (var iterator = CRDTDeserializer.DeserializeBatch(memory))
                {
                    while (iterator.MoveNext())
                    {
                        if (iterator.Current is CrdtMessage msg)
                        {
                            Debug.Log($"${msg.Type} - {msg.GetType()} - {msg.EntityId} - {msg.ComponentId} - {string.Join("", (byte[])msg.Data)}");
                        }
                    }
                }
            }
        }
        
    }
}