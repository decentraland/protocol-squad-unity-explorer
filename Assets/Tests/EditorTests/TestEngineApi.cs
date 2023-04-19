using System.IO;
using Cysharp.Threading.Tasks;
using JSContainer;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Extensions;
using NUnit.Framework;
using TestUtils;
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
        
        [Test]
        public void EngineApi_JSModule_EvaluateCodeStepWorks()
        {
            var engineApi = Substitute.For<IEngineApi>();
            
            var code = @"
                const engineApi = require('~system/EngineApi');
                                
                // Evaluation phase
                console.log('evaluation phase');

                module.exports.onStart = async function() {};
                module.exports.onUpdate = async function(dt) {};
            ";
            
            using var logInterceptor = new LogInterceptor();
            new DefaultNamespace.JSContainer()
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
            
            var scene = new DefaultNamespace.JSContainer()
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
            
            var scene = new DefaultNamespace.JSContainer()
                .WithEngineApi(engineApi)
                .EvaluateModule(code);

            Assert.That(scene.HasOnStart, Is.False);
        }
        
        [Test]
        public void EngineApi_AccessToStandardRequire_IsProhibited()
        {
            var extraModule = @"
                module.exports = 'Any modules evaluated by JSContainer must not have access to modules in fileSystems'
            ";

            var tempPath = Path.GetTempFileName();
            File.WriteAllText(tempPath, extraModule);

            var testModule = $@"
                const testVar = require('{tempPath.Replace("\\","/").Normalize()}');
                console.log(testVar);          
            ";
            using var logInterceptor = new LogInterceptor();
            new DefaultNamespace.JSContainer().Execute(testModule);
            Assert.That(logInterceptor.Last, Is.Empty);
            File.Delete(tempPath);
        }
    }
}