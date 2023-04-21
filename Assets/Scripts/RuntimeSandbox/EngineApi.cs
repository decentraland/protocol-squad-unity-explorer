using Cysharp.Threading.Tasks;
using JSInterop;

namespace DefaultNamespace
{
    public class EngineApi : IEngineApi
    {
        public UniTask crdtSendToRenderer()
        {
            // do nothing at the moment
            return UniTask.CompletedTask;
        }
    }
}