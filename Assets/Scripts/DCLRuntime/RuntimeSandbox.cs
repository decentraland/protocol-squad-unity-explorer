using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JSInterop;

[assembly: InternalsVisibleTo("RuntimeTests")]

namespace DCLRuntime
{
    public class RuntimeSandbox : IDisposable
    {
        private readonly ICRDTMessageHandler _crdtMessageHandler;
        private readonly CancellationTokenSource _cts = new();

        private readonly JSContainer _jsContainer = new();
        private readonly SceneModule _sceneModule;
        internal Thread Thread;

        public RuntimeSandbox(string scene) : this(scene, new EngineApi(), new CRDTMessageHandler())
        {
        }

        internal RuntimeSandbox(string scene, IEngineApi engineApi) : this(scene, engineApi, new CRDTMessageHandler())
        {
        }

        internal RuntimeSandbox(string scene, IEngineApi engineApi, ICRDTMessageHandler crdtMessageHandler)
        {
            _sceneModule = _jsContainer.WithEngineApi(engineApi).EvaluateModule(scene);
            _crdtMessageHandler = crdtMessageHandler;
        }

        public void Dispose()
        {
            _cts.Cancel();
            if (Thread != null)
            {
                Thread.Abort(); // TODO: fix that with cancellation token or graceful shutdown
                Thread = null;
            }
            _jsContainer.Dispose();
        }

        public void Run()
        {
            RunFullLoop(_cts.Token);
        }

        private async void RunFullLoop(CancellationToken token)
        {
            var previousTime = Process.GetCurrentProcess().TotalProcessorTime;
            if (_sceneModule.HasOnStart)
                await Task.Run(async () =>
                {
                    Thread = Thread.CurrentThread;
                    await _sceneModule.OnStart();
                    token.ThrowIfCancellationRequested();
                    token.ThrowIfCancellationRequested();
                }, token);

            while (!token.IsCancellationRequested)
            {
                var newTime = Process.GetCurrentProcess().TotalProcessorTime;
                var dt = (newTime - previousTime).TotalMilliseconds;
                previousTime = newTime;
                await Task.Run(async () =>
                {
                    Thread = Thread.CurrentThread;
                    await _sceneModule.OnUpdate(dt);
                    token.ThrowIfCancellationRequested();
                }, token);
                token.ThrowIfCancellationRequested();
                await _crdtMessageHandler.Process();
            }
        }
    }
}