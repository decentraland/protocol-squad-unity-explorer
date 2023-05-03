using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JSInterop;
using VContainer;
using VContainer.Unity;

[assembly: InternalsVisibleTo("RuntimeTests")]

namespace DCLRuntime
{
    public class RuntimeSandbox : IDisposable, IStartable
    {
        private readonly CancellationTokenSource _cts = new();

        private readonly JSContainer _jsContainer = new();
        private readonly SceneModule _sceneModule;
        internal Thread Thread;

        [Inject]
        public RuntimeSandbox(SceneJsonWrapper scene, SceneEntityManager sceneEntityManager) : this(scene.Json,
            new EngineApi(new ComponentManager(sceneEntityManager)))
        {
        }

        public RuntimeSandbox(string scene) : this(scene,
            new EngineApi(new ComponentManager(SceneEntityManager.Create())))
        {
        }

        internal RuntimeSandbox(string scene, IEngineApi engineApi)
        {
            _sceneModule = _jsContainer.WithEngineApi(engineApi).EvaluateModule(scene);
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

        public void Start()
        {
            RunFullLoop(_cts.Token);
        }

        private async void RunFullLoop(CancellationToken token)
        {
            var previousTime = DateTime.Now;
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
                var newTime = DateTime.Now;
                var dt = (newTime - previousTime).TotalMilliseconds / 1000d;
                previousTime = newTime;
                await Task.Run(async () =>
                {
                    Thread = Thread.CurrentThread;
                    await _sceneModule.OnUpdate(dt);
                    token.ThrowIfCancellationRequested();
                }, token);
                token.ThrowIfCancellationRequested();
            }
        }
    }
}