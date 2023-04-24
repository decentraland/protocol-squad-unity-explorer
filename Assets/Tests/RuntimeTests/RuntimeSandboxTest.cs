using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using JSInterop;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace RuntimeTests
{
    public class RuntimeSandboxTest
    {
        // TODO: move that to separated class and reuse
        public static readonly string MinimalScene = @"
            const engineApi = require('~system/EngineApi');                                            
            module.exports.onUpdate = async function(dt) {                  
                await engineApi.crdtSendToRenderer();
            }";

        //[UnityTest]
        public IEnumerator RuntimeSandbox_RunLoop_InSeparatedThread() => UniTask.ToCoroutine(async () =>
        {
            var engineApi = Substitute.For<IEngineApi>();
            int loopThreadName = int.MinValue;
            engineApi.crdtSendToRenderer().Returns(_ =>
            {
                loopThreadName = Thread.CurrentThread.ManagedThreadId;
                return UniTask.CompletedTask;
            });

            var mainThreadName = Thread.CurrentThread.ManagedThreadId;

            var sandbox = new RuntimeSandbox(MinimalScene, engineApi);
            sandbox.Run(); 
            
            await UniTask.Yield();
            Assert.That(mainThreadName, Is.Not.EqualTo(loopThreadName), "threads are the same");
        });



        private class MockEngineApi : IEngineApi
        {
            public UniTask crdtSendToRenderer()
            {
                return UniTask.CompletedTask;
            }
        }
        

        [UnityTest]
        public IEnumerator RuntimeSandbox_Dispose_TerminateRunLoop() => UniTask.ToCoroutine(async () =>
        {
            var engineApi = Substitute.For<IEngineApi>();
            
            engineApi.crdtSendToRenderer().Returns(_ => UniTask.CompletedTask);

            var sandbox = new RuntimeSandbox(MinimalScene, new MockEngineApi());
            sandbox.Run();
            await UniTask.DelayFrame(3);
            Assert.That(Thread.CurrentThread.ManagedThreadId, Is.Not.EqualTo(sandbox.Thread.ManagedThreadId));
            await UniTask.SwitchToMainThread();
            sandbox.Dispose();
            await UniTask.DelayFrame(15);
            if (sandbox.Thread != null &&  sandbox.Thread.IsAlive) // before any assert let's 
            {
                Assert.Fail("thread is still alive");
                sandbox.Thread.Abort();
            }
        });


        public IEnumerator RuntimeSandbox_Dispose_TimingAreCorrect() => UniTask.ToCoroutine(async () =>
        {
            Assert.Fail();
        });


        public void RuntimeSandbox_OnStart_IsOptional()
        {
            Assert.Fail();
        }
    }
}