using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JSContainer;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

namespace RuntimeTests
{
    public class TestEngineApiRuntime
    {
        [UnityTest]
        public IEnumerator EngineApi_CallBack_AsyncReturnIn1Frame() => UniTask.ToCoroutine(async () =>
        {
            var engineApi = Substitute.For<IEngineApi>();

            var startFrame = Time.frameCount;
            var endFrame = int.MinValue;
            
            async UniTask DelayFrame(CallInfo arg)
            {
                await UniTask.DelayFrame(1);
                endFrame = Time.frameCount;
            }

            engineApi.crdtSendToRenderer().Returns(DelayFrame);
            
            var code = @"
                const engineApi = require('~system/EngineApi');
                
                engineApi.crdtSendToRenderer();
            ";

            new DefaultNamespace.JSContainer()
                .WithEngineApi(engineApi)
                .Execute(code);

            await UniTask.DelayFrame(2);
            
            Assert.That(endFrame, Is.EqualTo(startFrame + 1));
        });


        private class MockEngineApi : IEngineApi
        {
            public UniTask crdtSendToRenderer()
            {
                return UniTask.CompletedTask;
            }
        }
        
        [UnityTest]
        public IEnumerator EngineApi_onUpdate_DelayBetweenCalsLatency() => UniTask.ToCoroutine(
            async () =>
            {
                var engineApi = Substitute.For<IEngineApi>();
                await UniTask.SwitchToThreadPool();
                
                var code = $@"
                    const engineApi = require('~system/EngineApi');                                            
                    module.exports.onUpdate = async function(dt) {{                  
                        await engineApi.crdtSendToRenderer();
                    }}";
                
                
                var module = new DefaultNamespace.JSContainer()
                    .WithEngineApi(engineApi)
                    .EvaluateModule(code);
                
                await module.OnUpdate();
                var numberOfCalls = 1000;
                var resultTime = new long[numberOfCalls]; 
                var resultTicks = new long[numberOfCalls]; 
                var timer = new Stopwatch();
                for (int i = 0; i < numberOfCalls; i++)
                {
                    timer.Reset();
                    timer.Start();
                    await module.OnUpdate();
                    timer.Stop();
                    resultTime[i] = timer.ElapsedMilliseconds;
                    resultTicks[i] = timer.ElapsedTicks;
                }

                var average = resultTime.Average();

                string allTimes = "time = " + string.Join(",", resultTime);
                Assert.That(average, Is.LessThan(1), allTimes);
                Debug.Log(allTimes);
                Debug.Log($"averaget ticks:{resultTicks.Average()}  all:"+string.Join(", ", resultTicks));
            });

                // 1. test run chain
        [UnityTest]
        public IEnumerator EngineApi_AsyncManagedAsyncJS_ExecuteAsyncSequentially() => UniTask.ToCoroutine(async () =>
        {
            
            var timer = new Stopwatch();
            
            var engineApi = Substitute.For<IEngineApi>();
            
            long managedCallTimeStart = long.MinValue;
            long managedCallTimeEnd = long.MinValue;
            long onUpdateEnd = long.MinValue;
            
            async UniTask DelayFrame(CallInfo arg)
            {
                // js wait 50 ms before call this
                managedCallTimeStart = timer.ElapsedMilliseconds; 
                await UniTask.Delay(50);
                managedCallTimeEnd = timer.ElapsedMilliseconds;
            }

            engineApi.crdtSendToRenderer().Returns(DelayFrame);
            
            timer.Start();
            var code = $@"
                const engineApi = require('~system/EngineApi');            
                
                module.exports.onStart = async function(){{
                    // do nothing just warmup?
                }}
               
                module.exports.onUpdate = async function(dt) {{
                   //await waitMilliseconds(50);
                    engineApi.crdtSendToRenderer();
                }};
            ";
            
            
            
            var module = new DefaultNamespace.JSContainer()
                .WithEngineApi(engineApi)
                .EvaluateModule(code);

            
            timer.Start();
            await module.OnUpdate();
            timer.Stop();
            onUpdateEnd = timer.ElapsedMilliseconds;
            
            Assert.That(managedCallTimeStart, Is.EqualTo(50));
            Assert.That(managedCallTimeEnd, Is.EqualTo(100));
            Assert.That(onUpdateEnd, Is.EqualTo(managedCallTimeEnd));
        });

        // 2. Test run on managed thread



    }
}