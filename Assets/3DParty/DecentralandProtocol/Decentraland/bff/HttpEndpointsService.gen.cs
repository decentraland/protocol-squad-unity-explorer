// AUTOGENERATED, DO NOT EDIT
// Type definitions for server implementations of ports.
// package: decentraland.bff
// file: decentraland/bff/http_endpoints.proto
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Google.Protobuf;
using rpc_csharp.protocol;
using rpc_csharp;
using Google.Protobuf.WellKnownTypes;
namespace Decentraland.Bff {
public interface IHttpEndpoints<Context>
{

  UniTask<AboutResponse> About(Empty request, Context context, CancellationToken ct);

}

public static class HttpEndpointsCodeGen
{
  public const string ServiceName = "HttpEndpoints";

  public static void RegisterService<Context>(RpcServerPort<Context> port, IHttpEndpoints<Context> service)
  {
    var result = new ServerModuleDefinition<Context>();
      
    result.definition.Add("About", async (payload, context, ct) => { var res = await service.About(Empty.Parser.ParseFrom(payload), context, ct); return res?.ToByteString(); });

    port.RegisterModule(ServiceName, (port) => UniTask.FromResult(result));
  }
}
}
