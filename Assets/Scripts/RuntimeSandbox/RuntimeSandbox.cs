using System.Diagnostics;
using Cysharp.Threading.Tasks;
using JSInterop;

namespace DefaultNamespace
{
    public class RuntimeSandbox
    {
        private readonly SceneModule _jsContainer;
        
        public RuntimeSandbox(string scene)
        {
            _jsContainer = new JSContainer().EvaluateModule(scene);
        }
        
        public async UniTask Run()
        {
            await UniTask.SwitchToThreadPool();
            var previousTime =  Process.GetCurrentProcess().TotalProcessorTime;
            await _jsContainer.OnStart();
            
            while (true)
            {
                var newTime = Process.GetCurrentProcess().TotalProcessorTime;
                var dt = (newTime - previousTime).TotalMilliseconds;
                previousTime = newTime;
                await _jsContainer.OnUpdate(dt);
            }
        }
    }
}