using JSContainer;
using NSubstitute;
using NUnit.Framework;

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