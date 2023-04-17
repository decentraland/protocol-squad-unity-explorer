using System;
using System.Collections.Generic;
using System.Numerics;
using DefaultNamespace.Modules;
using JSContainer;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;


namespace DefaultNamespace
{
    public class JSContainer
    {
        private readonly V8ScriptEngine _engine;
        private readonly Dictionary<string, object> _globalModuleInstances = new();
        private readonly IReadOnlyDictionary<string, Type> _ioModulesByName;

        static JSContainer()
        {
            V8Settings.GlobalFlags |= V8GlobalFlags.DisableBackgroundWork;
        }

        public JSContainer()
        {
            _engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableTaskPromiseConversion);
            _engine.AddHostType("console", typeof(ConsoleModule));
            _engine.DocumentSettings.AccessFlags = DocumentAccessFlags.EnableFileLoading;
        }

        /// <summary>
        ///  Execute JS code as CommonJS module
        /// </summary>
        /// <param name="code">Java script code</param>
        public void Execute(string code)
        {
            _engine.Execute(new DocumentInfo { Category = ModuleCategory.CommonJS }, code);
        }

        public JSContainer WithEngineApi(IEngineApi engineApi)
        {
            // this guy works
            // _engine.AddHostObject("vec", new Vector2(12,13));
            // _engine.DocumentSettings.AddSystemDocument($"~system/vec", ModuleCategory.CommonJS,
            //     @"module.exports = vec;");
            
            _engine.AddHostObject("__engineApiInternal", new EngineApiAdapter(engineApi));
            _engine.DocumentSettings.AddSystemDocument($"~system/EngineApi", ModuleCategory.CommonJS,
                @"module.exports = __engineApiInternal;");
            return this;
        }
    }
}