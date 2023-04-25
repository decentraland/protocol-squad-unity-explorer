using System.Threading.Tasks;
using JetBrains.Annotations;

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
            await _api.crdtSendToRenderer();
        }
    }
}