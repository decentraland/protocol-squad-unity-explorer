using System.Threading.Tasks;

namespace DCLRuntime
{
    
    // this is the instance per scene(JS) which will handle incoming CRDT Messages
    // Keep in mind that the Process method will be called on the main thread
    public interface ICRDTMessageHandler
    {
        /// <summary>
        /// Method which handle incoming messages.
        /// Run on the main thread
        /// </summary>
        Task Process();
    }
}