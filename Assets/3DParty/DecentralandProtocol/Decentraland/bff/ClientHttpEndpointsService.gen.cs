
// AUTOGENERATED, DO NOT EDIT
// Type definitions for server implementations of ports.
// package: decentraland.bff
// file: decentraland/bff/http_endpoints.proto
using Cysharp.Threading.Tasks;
using rpc_csharp;
using Google.Protobuf.WellKnownTypes;

namespace Decentraland.Bff {
public interface IClientHttpEndpoints
{
  UniTask<AboutResponse> About(Empty request);
}

public class ClientHttpEndpoints : IClientHttpEndpoints
{
  private readonly RpcClientModule module;

  public ClientHttpEndpoints(RpcClientModule module)
  {
      this.module = module;
  }

  
  public UniTask<AboutResponse> About(Empty request)
  {
      return module.CallUnaryProcedure<AboutResponse>("About", request);
  }

}
}
