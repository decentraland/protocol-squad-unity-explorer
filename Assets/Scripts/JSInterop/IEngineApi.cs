using Cysharp.Threading.Tasks;

namespace JSInterop
{
    public interface IEngineApi
    {
        /// <summary>
        ///     TODO: Pass and receive CRDT messages here
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        UniTask crdtSendToRenderer(dynamic data);

        UniTask<object> crdtGetState(dynamic data);
    }
}