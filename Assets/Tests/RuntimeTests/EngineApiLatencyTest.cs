using System.Collections;
using System.Diagnostics;
using System.Linq;
using Cysharp.Threading.Tasks;
using JSContainer;
using NUnit.Framework;
using UnityEngine.Profiling;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

namespace RuntimeTests
{
    public class EngineApiLatencyTest
    {
        private SceneModule _module;

        private class EngineApiMock : IEngineApi
        {
            public async UniTask crdtSendToRenderer()
            {
            }
        }
        
        public void TestAccessIsClosed()
        {
            var code = $@"
                    const criticalAPI = require('C://somefolder');

                    assert criticalApi == null;
                                            
                    module.exports.onUpdate = async function(dt) {{                  
                        await engineApi.crdtSendToRenderer();
                    }}";
        }
        
        [SetUp]
        public void SetupJSContainer()
        {
            var code = $@"
                    const engineApi = require('~system/EngineApi');                                            
                    module.exports.onUpdate = async function(dt) {{                  
                        await engineApi.crdtSendToRenderer();
                    }}";
                
                
            
            _module = new DefaultNamespace.JSContainer()
                .WithEngineApi(new EngineApiMock())
                .EvaluateModule(code);

            // warmup engine
            for (int i = 0; i < 5; i++)
            {
                _module.OnUpdate();
            }
        }

        
        // test first 10 execution that spikes not goes higher than 10 ms
        [UnityTest]
        [Repeat(10)]
        public IEnumerator EngineApi_ModuleRenderModuleLoopAverageFewCalls_LessThan10MS() => UniTask.ToCoroutine(
            async () =>
            {
                await UniTask.SwitchToThreadPool();

                
                var timer = new Stopwatch();
                
                    timer.Start();
                    await _module.OnUpdate();
                    timer.Stop();
                
                Assert.That(timer.ElapsedMilliseconds, Is.LessThan(10));
            });

        [UnityTest]
        public IEnumerator EngineApi_ModuleRenderModuleLoopAverage_LessThan1MS() => UniTask.ToCoroutine(
            async () =>
            {
                await UniTask.SwitchToThreadPool();

                var iteration = 1000;
                var time = new long[iteration];
                var timer = new Stopwatch();
                for (int i = 0; i < iteration; i++)
                {
                    timer.Reset();
                    timer.Start();
                    await _module.OnUpdate();
                    timer.Stop();
                    time[i] = timer.ElapsedMilliseconds;
                }

                Assert.That(time.Average(), Is.LessThan(1));
            });

        
    }
}