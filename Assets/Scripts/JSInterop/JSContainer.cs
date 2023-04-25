using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JSInterop.Modules;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace JSInterop
{
    public class JSContainer : IDisposable
    {
        private readonly V8ScriptEngine _engine;
        private readonly IReadOnlyDictionary<string, Type> _ioModulesByName;

        private int _moduleIdCounter = 100;

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
            engine.DocumentSettings.LoadCallback = (ref DocumentInfo info) =>
            {
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

        /// <summary>
        ///     Evaluate script as CommonJS module
        /// </summary>
        /// <param name="code"></param>
        public SceneModule EvaluateModule(string code)
        {
            var moduleId = $"~tmp/module-{_moduleIdCounter++}";

            _engine.DocumentSettings.AddSystemDocument(moduleId, ModuleCategory.CommonJS, code);

            var result = _engine.Evaluate(new DocumentInfo { Category = ModuleCategory.CommonJS }, $@"
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
                    await __engineApi.crdtSendToRenderer();
                }}");
            return this;
        }

        public void Dispose()
        {
            _engine?.Dispose();
        }
    }
}