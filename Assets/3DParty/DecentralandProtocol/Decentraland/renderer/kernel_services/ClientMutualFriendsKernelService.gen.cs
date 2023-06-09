
// AUTOGENERATED, DO NOT EDIT
// Type definitions for server implementations of ports.
// package: decentraland.renderer.kernel_services
// file: decentraland/renderer/kernel_services/mutual_friends_kernel.proto
using Cysharp.Threading.Tasks;
using rpc_csharp;

namespace Decentraland.Renderer.KernelServices {
public interface IClientMutualFriendsKernelService
{
  UniTask<GetMutualFriendsResponse> GetMutualFriends(GetMutualFriendsRequest request);
}

public class ClientMutualFriendsKernelService : IClientMutualFriendsKernelService
{
  private readonly RpcClientModule module;

  public ClientMutualFriendsKernelService(RpcClientModule module)
  {
      this.module = module;
  }

  
  public UniTask<GetMutualFriendsResponse> GetMutualFriends(GetMutualFriendsRequest request)
  {
      return module.CallUnaryProcedure<GetMutualFriendsResponse>("GetMutualFriends", request);
  }

}
}
