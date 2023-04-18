using Cysharp.Threading.Tasks;

namespace JSContainer
{
    public interface IEngineApi
    {
        /// <summary>
        /// TODO: Pass and receive CRDT messages here
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        UniTask crdtSendToRenderer();
    }
}