using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
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

        /// <summary>
        /// Evaluate script as CommonJS module
        /// </summary>
        /// <param name="code"></param>
        public SceneModule EvaluateModule(string code)
        {
            var tmpPath = Path.GetTempFileName();
            //var tmpPath = Path.Combine(Path.GetTempPath(), "tempfile.txt");
            //var tmpPath = Application.temporaryCachePath+ "/tmp.js";
            File.WriteAllText(path:tmpPath, contents: code);

            //var result = _engine.Evaluate(new DocumentInfo { Category = ModuleCategory.CommonJS }, code);
            var result = _engine.Evaluate(new DocumentInfo { Category = ModuleCategory.CommonJS }, 
                $"return require('{tmpPath.Replace('\\', '/').Normalize()}')");
            File.Delete(tmpPath);
            return new SceneModule(result);
        }

        public JSContainer WithEngineApi(IEngineApi engineApi)
        {
            _engine.AddHostObject("__engineApiInternal", new EngineApiAdapter(engineApi));
            _engine.DocumentSettings.AddSystemDocument($"~system/EngineApi", ModuleCategory.CommonJS,
                @"module.exports = __engineApiInternal;");
            return this;
        }
    }
}