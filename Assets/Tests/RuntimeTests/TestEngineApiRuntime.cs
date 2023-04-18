using System.Collections;
using Cysharp.Threading.Tasks;
using JSContainer;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
    }
}