using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JSInterop;

[assembly:InternalsVisibleTo("RuntimeTests")]

namespace DefaultNamespace
{
    public class RuntimeSandbox : IDisposable
    {
        private readonly JSContainer _jsContainer = new();
        private readonly SceneModule _sceneModule;
        private readonly CancellationTokenSource _cts = new();
        internal Thread Thread;
        private readonly TaskCompletionSource<bool> _exitSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public Task WaitForExitAsync() => _exitSource.Task;
        
        
        public RuntimeSandbox(string scene) : this(scene, new EngineApi())
        {
        }
        
        internal RuntimeSandbox(string scene, IEngineApi engineApi)
        {
            _sceneModule = _jsContainer.WithEngineApi(engineApi).EvaluateModule(scene);
        }
        
        public void Run()
        {
            Task.Run(() => DoRun(_cts.Token));
        }

        private async Task DoRun(CancellationToken token)
        {
            Thread = Thread.CurrentThread;
            var previousTime = Process.GetCurrentProcess().TotalProcessorTime;
            if (_sceneModule.HasOnStart)
            {
                await _sceneModule.OnStart();
            }

            while (!token.IsCancellationRequested)
            {
                var newTime = Process.GetCurrentProcess().TotalProcessorTime;
                var dt = (newTime - previousTime).TotalMilliseconds;
                previousTime = newTime;
                await _sceneModule.OnUpdate(dt);
            }
            Thread = null;
            _exitSource.TrySetResult(true);
        }

        public void Dispose()
        {
            _cts.Cancel();
            WaitForExitAsync().Wait();    
            _jsContainer.Dispose();
        }
    }
}