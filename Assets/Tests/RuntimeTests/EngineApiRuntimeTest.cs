using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JSInterop;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

namespace RuntimeTests
{
    public class EngineApiRuntimeTest
    {
        [UnityTest]
        public IEnumerator EngineApi_CallBack_AsyncReturnIn1Frame()
        {
            return UniTask.ToCoroutine(async () =>
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

                new JSContainer()
                    .WithEngineApi(engineApi)
                    .Execute(code);

                await UniTask.DelayFrame(2);

                Assert.That(endFrame, Is.EqualTo(startFrame + 1));
            });
        }

        [UnityTest]
        public IEnumerator EngineApi_onUpdate_DelayBetweenCalsLatency()
        {
            return UniTask.ToCoroutine(
                async () =>
                {
                    var engineApi = Substitute.For<IEngineApi>();
                    await UniTask.SwitchToThreadPool();

                    var code = @"
                    const engineApi = require('~system/EngineApi');                                            
                    module.exports.onUpdate = async function(dt) {                  
                        await engineApi.crdtSendToRenderer();
                    }";


                    var module = new JSContainer()
                        .WithEngineApi(engineApi)
                        .EvaluateModule(code);

                    await module.OnUpdate(0);
                    var numberOfCalls = 1000;
                    var resultTime = new long[numberOfCalls];
                    var resultTicks = new long[numberOfCalls];
                    var timer = new Stopwatch();
                    for (var i = 0; i < numberOfCalls; i++)
                    {
                        timer.Reset();
                        timer.Start();
                        await module.OnUpdate(0);
                        timer.Stop();
                        resultTime[i] = timer.ElapsedMilliseconds;
                        resultTicks[i] = timer.ElapsedTicks;
                    }

                    var average = resultTime.Average();

                    var allTimes = "time = " + string.Join(",", resultTime);
                    Assert.That(average, Is.LessThan(1), allTimes);
                    Debug.Log(allTimes);
                    Debug.Log($"averaget ticks:{resultTicks.Average()}  all:" + string.Join(", ", resultTicks));
                });
        }

        // 1. test run chain
        [UnityTest]
        public IEnumerator EngineApi_AsyncManagedAsyncJS_ExecuteAsyncSequentially()
        {
            return UniTask.ToCoroutine(async () =>
            {
                await UniTask.SwitchToThreadPool();
                var threadName = Thread.CurrentThread.Name;

                var fakeEngineApi = new FakeEngineApi();
                var code = @"
                const engineApi = require('~system/EngineApi');            
                
                module.exports.onStart = async function(){
                    // do nothing just warmup?
                }
               
                module.exports.onUpdate = async function(dt) {                   
                    await waitMilliseconds(50);
                    await engineApi.crdtSendToRenderer();
                };
            ";

                var module = new JSContainer()
                    .WithEngineApi(fakeEngineApi)
                    .EvaluateModule(code);

                // warmup first 10 executions
                for (var i = 0; i < 10; i++) await module.OnUpdate(0);

                var startTime = DateTime.Now;
                await module.OnUpdate(0);
                var endTime = DateTime.Now;

                Assert.That(threadName, Is.EqualTo(fakeEngineApi.threadName));

                // 1 milisecond tolerance for all measurement
                // We can not use StopWatch in thread pool
                Assert.That((fakeEngineApi.ManagedCallTimeStart - startTime).Milliseconds,
                    Is.EqualTo(50).Within(1), $"1. Subtest {nameof(fakeEngineApi.ManagedCallTimeStart)} problem");

                Assert.That((fakeEngineApi.ManagedCallTimeEnd - fakeEngineApi.ManagedCallTimeStart).Milliseconds,
                    Is.EqualTo(50).Within(1), $"2. Subtest {nameof(fakeEngineApi.ManagedCallTimeEnd)} problem  " +
                                              $"msEnd:{fakeEngineApi.ManagedCallTimeEnd.Millisecond} " +
                                              $"msStart:{fakeEngineApi.ManagedCallTimeStart.Millisecond}");

                Assert.That((endTime - fakeEngineApi.ManagedCallTimeEnd).Milliseconds,
                    Is.EqualTo(0).Within(1), "3. Subtest end problem");
            });
        }


        private class MockEngineApi : IEngineApi
        {
            public UniTask crdtSendToRenderer()
            {
                return UniTask.CompletedTask;
            }
        }


        private class FakeEngineApi : IEngineApi
        {
            public DateTime ManagedCallTimeEnd;
            public DateTime ManagedCallTimeStart;
            public string threadName;

            public async UniTask crdtSendToRenderer()
            {
                ManagedCallTimeStart = DateTime.Now;
                await Task.Delay(50);
                ManagedCallTimeEnd = DateTime.Now;
                threadName = Thread.CurrentThread.Name;
            }
        }
    }
}