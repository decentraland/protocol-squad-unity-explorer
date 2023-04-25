using Cysharp.Threading.Tasks;
using JSInterop;

namespace DCLRuntime
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