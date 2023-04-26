using System.Collections;
using System.Diagnostics;
using System.Linq;
using Cysharp.Threading.Tasks;
using JSInterop;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace RuntimeTests
{
    public class EngineApiLatencyTest
    {
        private SceneModule _module;

        [SetUp]
        public void SetupJSContainer()
        {
            var code = @"
                    const engineApi = require('~system/EngineApi');                                            
                    module.exports.onUpdate = async function(dt) {                  
                        await engineApi.crdtSendToRenderer();
                    }";


            _module = new JSContainer()
                .WithEngineApi(new EngineApiMock())
                .EvaluateModule(code);

            // warmup engine
            for (var i = 0; i < 5; i++) _module.OnUpdate(0);
        }


        // test first 10 execution that spikes not goes higher than 10 ms
        [UnityTest]
        [Repeat(10)]
        public IEnumerator EngineApi_ModuleRenderModuleLoopAverageFewCalls_LessThan10MS()
        {
            return UniTask.ToCoroutine(
                async () =>
                {
                    await UniTask.SwitchToThreadPool();


                    var timer = new Stopwatch();

                    timer.Start();
                    await _module.OnUpdate(0);
                    timer.Stop();

                    Assert.That(timer.ElapsedMilliseconds, Is.LessThan(10));
                });
        }

        [UnityTest]
        public IEnumerator EngineApi_ModuleRenderModuleLoopAverage_LessThan1MS()
        {
            return UniTask.ToCoroutine(
                async () =>
                {
                    await UniTask.SwitchToThreadPool();

                    var iteration = 1000;
                    var time = new long[iteration];
                    var timer = new Stopwatch();
                    for (var i = 0; i < iteration; i++)
                    {
                        timer.Reset();
                        timer.Start();
                        await _module.OnUpdate(0);
                        timer.Stop();
                        time[i] = timer.ElapsedMilliseconds;
                    }

                    Assert.That(time.Average(), Is.LessThan(1));
                });
        }

        private class EngineApiMock : IEngineApi
        {
            public async UniTask crdtSendToRenderer(dynamic data)
            {
            }

            public async UniTask<object> crdtGetState(dynamic data)
            {
                return null;
            }
        }
    }
}