using System.Threading.Tasks;

namespace DCLRuntime
{
    public class CRDTMessageHandler : ICRDTMessageHandler
    {
        public Task Process()
        {
            return Task.CompletedTask;
        }
    }
}