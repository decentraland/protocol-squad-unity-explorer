using Cysharp.Threading.Tasks;
using JSContainer;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditorTests
{
    public class TestEngineApi
    {
        [Test]
        public void EngineApi_Available_AsSystemModuleInJS()
        {
            var engineApi = Substitute.For<IEngineApi>();
            var code = @"
                const engineApi = require('~system/EngineApi');
                
                engineApi.crdtSendToRenderer();
            ";
            
            new DefaultNamespace.JSContainer()
                .WithEngineApi(engineApi)
                .Execute(code);

            engineApi.Received().crdtSendToRenderer();
        }
        
      

        
    }
}