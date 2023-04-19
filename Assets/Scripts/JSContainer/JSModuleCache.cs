using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    // Method contains instantiated objedct by key which can be used at scene.js as 
    // require('moduel-id')
    // the same concept as in nodeJs is used, if you request module from multiple places
    // the same instance will be returned.
    // e.g. scene1.js and scene2.js will receive the same instance of `~system/EngineApi`
    internal class JSModuleCache
    {
        private readonly Dictionary<string, object> _moduleByKey = new();
        public void AddModuleObject(string key, object moduleObject)
        {
            _moduleByKey.Add(key, moduleObject);
        }

        public object GetModuleByKey(string moduleKey)
        {
            if (_moduleByKey.TryGetValue(moduleKey, out var module))
            {
                return module;
            }

            var errMessage = $"Module [{moduleKey}] Is not supported"; 
            Debug.LogError(errMessage);
            return errMessage;
        }
    }
}