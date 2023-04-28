using JSInterop;
using Microsoft.ClearScript;
using NSubstitute;
using NUnit.Framework;
using TestUtils;

namespace EditorTests
{
    public class EngineApiTest
    {
        [Test]
        public void EngineApi_AccessToEngineApi_IsAvailable()
        {
            var engineApi = Substitute.For<IEngineApi>();
            var code = @"
                const engineApi = require('~system/EngineApi');
                
                engineApi.crdtSendToRenderer();
            ";

            new JSContainer()
                .WithEngineApi(engineApi)
                .Execute(code);

            engineApi.Received().crdtSendToRenderer(null);
        }

        [Test]
        public void EngineApi_JSModule_EvaluateCodeStepWorks()
        {
            var engineApi = Substitute.For<IEngineApi>();

            var code = @"
                // const engineApi = require('~system/EngineApi');
                                
                // Evaluation phase
                console.log('evaluation phase');

                module.exports.onStart = async function() {};
                module.exports.onUpdate = async function(dt) {};
            ";

            using var logInterceptor = new LogInterceptor();
            new JSContainer()
                .WithEngineApi(engineApi)
                .EvaluateModule(code);

            Assert.That(logInterceptor.Last, Is.EqualTo("evaluation phase"));
        }


        [Test]
        public void EngineApi_JSModuleHasOnStart_EqualTrue()
        {
            var engineApi = Substitute.For<IEngineApi>();

            var code = @"
                const engineApi = require('~system/EngineApi');                                
                
                module.exports.onStart = async function() {
                    console.log('onStart');
                };
                module.exports.onUpdate = async function(dt) {
                    console.log('onUpdate');
                };
            ";

            var scene = new JSContainer()
                .WithEngineApi(engineApi)
                .EvaluateModule(code);

            Assert.That(scene.HasOnStart, Is.True);
        }


        [Test]
        public void EngineApi_JSModuleHasOnStart_EqualFalse()
        {
            var engineApi = Substitute.For<IEngineApi>();

            var code = @"
                const engineApi = require('~system/EngineApi');            
                module.exports.onUpdate = async function(dt) {};
            ";

            var scene = new JSContainer()
                .WithEngineApi(engineApi)
                .EvaluateModule(code);

            Assert.That(scene.HasOnStart, Is.False);
        }

        [Test]
        public void EngineApi_AccessToStandardRequireInExecute_IsProhibited()
        {
            var testString = "Any modules evaluated by JSContainer must not have access to modules in fileSystems";
            var extraModule = $@"
                module.exports = '{testString}'
            ";

            using var tmpFile = new TempFileWithContent(extraModule);

            var testModule = $@"
                const testVar = require('{tmpFile.NormalizedPath}');
                console.log(testVar);          
            ";
            using var logInterceptor = new LogInterceptor();

            Assert.Catch(typeof(ScriptEngineException), () => { new JSContainer().Execute(testModule); });
            Assert.That(logInterceptor.Last, Is.Not.EqualTo(testString));
        }


        [Test]
        public void EngineApi_AccessToStandardRequireInEvaluate_IsProhibited()
        {
            var testString = "Any modules evaluated by JSContainer must not have access to modules in fileSystems";
            var extraModule = $@"
                module.exports = '{testString}'
            ";

            using var tmpFile = new TempFileWithContent(extraModule);

            var testModule = $@"
                const testVar = require('{tmpFile.NormalizedPath}');
                console.log(testVar);    
                module.exports.onStart = async function() {{}};
                module.exports.onUpdate = async function(dt) {{}};      
            ";
            using var logInterceptor = new LogInterceptor();

            Assert.Catch(typeof(ScriptEngineException), () => { new JSContainer().EvaluateModule(testModule); });
            Assert.That(logInterceptor.Last, Is.Not.EqualTo(testString));
        }
    }
}