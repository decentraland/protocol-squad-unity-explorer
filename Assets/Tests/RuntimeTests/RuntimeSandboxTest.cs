using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DCLRuntime;
using JSInterop;
using NSubstitute;
using NUnit.Framework;
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

        [UnityTest]
        public IEnumerator RuntimeSandbox_RunLoop_InSeparatedThread() => UniTask.ToCoroutine(async () =>
        {
            var engineApi = Substitute.For<IEngineApi>();
            int loopThreadId = int.MinValue;
            var completionTask = new TaskCompletionSource<bool>();

            engineApi.crdtSendToRenderer(null).Returns(_ =>
            {
                Interlocked.Exchange(ref loopThreadId, Thread.CurrentThread.ManagedThreadId);
                completionTask.SetResult(true);
                return UniTask.CompletedTask;
            });

            var mainThreadName = Thread.CurrentThread.ManagedThreadId;

            var sandbox = new RuntimeSandbox(new SceneJsonWrapper(MinimalScene), engineApi);
            sandbox.Start();
            await completionTask.Task;
            
            Assert.That(loopThreadId, Is.Not.EqualTo(int.MinValue), "loop thread id is not set");
            Assert.That(mainThreadName, Is.Not.EqualTo(loopThreadId), "threads are the same");
        });


        
        [UnityTest]
        public IEnumerator RuntimeSandbox_Dispose_TerminateRunLoop() => UniTask.ToCoroutine(async () =>
        {
            var engineApi = Substitute.For<IEngineApi>();
            
            engineApi.crdtSendToRenderer(null).Returns(_ => UniTask.CompletedTask);

            var sandbox = new RuntimeSandbox(new SceneJsonWrapper(MinimalScene), engineApi);
            sandbox.Start();
            await UniTask.DelayFrame(3);
            Assert.That(Thread.CurrentThread.ManagedThreadId, Is.Not.EqualTo(sandbox.Thread.ManagedThreadId));
            
            sandbox.Dispose();
            if (sandbox.Thread != null &&  sandbox.Thread.IsAlive) // before any assert let's 
            {
                Assert.Fail("thread is still alive");
                sandbox.Thread.Abort();
            }
        });


        [UnityTest]
        public IEnumerator RuntimeSandbox_Dispose_TimingAreCorrect() => UniTask.ToCoroutine(async () =>
        {
            var engineApi = Substitute.For<IEngineApi>();
            var invocationNumber = 0;
            
            engineApi.crdtSendToRenderer(null).Returns(_ =>
            {
                Interlocked.Increment(ref invocationNumber);
                return UniTask.CompletedTask;
            });
        
            var sandbox = new RuntimeSandbox(new SceneJsonWrapper(MinimalScene), engineApi);
            sandbox.Start();
            await UniTask.DelayFrame(130);
            sandbox.Dispose();
            Assert.That(invocationNumber, Is.EqualTo(3));
        });
        
    }
}