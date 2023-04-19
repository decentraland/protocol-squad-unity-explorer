using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Modules;
using JSContainer;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using UnityEngine;

namespace DefaultNamespace
{
    public class JSContainer
    {
        private readonly V8ScriptEngine _engine;
        private readonly IReadOnlyDictionary<string, Type> _ioModulesByName;
        
        static JSContainer()
        {
            V8Settings.GlobalFlags |= V8GlobalFlags.DisableBackgroundWork;
        }

        public JSContainer()
        {
            _engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableTaskPromiseConversion);
        
            _engine.AddHostType("console", typeof(ConsoleModule));
            _engine.Script.waitMilliseconds = new Func<int, object>(WaitMilliSeconds);
            SetupDocumentLoader(_engine);
        }

        private static void SetupDocumentLoader(V8ScriptEngine engine)
        {
            engine.DocumentSettings.LoadCallback = (ref DocumentInfo info) => {
                info.Category = ModuleCategory.CommonJS;
            };
        }
        
        private async Task WaitMilliSeconds(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }

        /// <summary>
        ///     Execute JS code as CommonJS module
        /// </summary>
        /// <param name="code">Java script code</param>
        public void Execute(string code)
        {
            _engine.Execute(new DocumentInfo { Category = ModuleCategory.CommonJS }, code);
        }

        private int _moduleIdCounter = 100;
        
        /// <summary>
        ///     Evaluate script as CommonJS module
        /// </summary>
        /// <param name="code"></param>
        public SceneModule EvaluateModule(string code)
        {
            var moduleId = $"~tmp/module-{_moduleIdCounter++}";

            _engine.DocumentSettings.AddSystemDocument(moduleId, ModuleCategory.CommonJS, code);

            var result =_engine.Evaluate(new DocumentInfo { Category = ModuleCategory.CommonJS }, $@"
                let result = require('{moduleId}');
                return result;
            ");

            
            return new SceneModule(result);
        }

        public JSContainer WithEngineApi(IEngineApi engineApi)
        {
            _engine.AddHostObject("__engineApi", new EngineApiAdapter(engineApi));
            _engine.DocumentSettings.AddSystemDocument("~system/EngineApi", ModuleCategory.CommonJS, @"
                module.exports.crdtSendToRenderer = async function() {{                  
                    console.log('before export');
                    await __engineApi.crdtSendToRenderer();
                    console.log('after export');
                }}");
            return this;
        }
    }
}