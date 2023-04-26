using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.ClearScript.JavaScript;

namespace JSInterop
{
    /// <summary>
    ///     ClearScript struggle to add host object through interface.
    ///     Use this adapter to forward the call to actual IEngineApi implementation
    /// </summary>
    public class EngineApiAdapter
    {
        private readonly IEngineApi _api;

        public EngineApiAdapter(IEngineApi api)
        {
            _api = api;
        }

        // ReSharper disable once InconsistentNaming
        // convert unitask to Task here
        [UsedImplicitly]
        public async Task crdtSendToRenderer()
        {
            await _api.crdtSendToRenderer(null);
        }

        // ReSharper disable once InconsistentNaming
        [UsedImplicitly]
        public async Task<object> crdtGetState(dynamic parameter)
        {
            var data = parameter.data;
            // data.key data.value
            return await _api.crdtGetState(data);
        }

        // ReSharper disable once InconsistentNaming
        [UsedImplicitly]
        public async Task crdtGetState()
        {
        }
    }
}