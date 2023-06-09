
// AUTOGENERATED, DO NOT EDIT
// Type definitions for server implementations of ports.
// package: decentraland.bff
// file: decentraland/bff/comms_service.proto
using Cysharp.Threading.Tasks;
using rpc_csharp;

namespace Decentraland.Bff {
public interface IClientCommsService
{
  UniTask<Subscription> SubscribeToPeerMessages(SubscriptionRequest request);

  IUniTaskAsyncEnumerable<PeerTopicSubscriptionResultElem> GetPeerMessages(Subscription request);

  UniTask<UnsubscriptionResult> UnsubscribeToPeerMessages(Subscription request);

  UniTask<Subscription> SubscribeToSystemMessages(SubscriptionRequest request);

  IUniTaskAsyncEnumerable<SystemTopicSubscriptionResultElem> GetSystemMessages(Subscription request);

  UniTask<UnsubscriptionResult> UnsubscribeToSystemMessages(Subscription request);

  UniTask<PublishToTopicResult> PublishToTopic(PublishToTopicRequest request);
}

public class ClientCommsService : IClientCommsService
{
  private readonly RpcClientModule module;

  public ClientCommsService(RpcClientModule module)
  {
      this.module = module;
  }

  
  public UniTask<Subscription> SubscribeToPeerMessages(SubscriptionRequest request)
  {
      return module.CallUnaryProcedure<Subscription>("SubscribeToPeerMessages", request);
  }

  public IUniTaskAsyncEnumerable<PeerTopicSubscriptionResultElem> GetPeerMessages(Subscription request)
  {
      return module.CallServerStream<PeerTopicSubscriptionResultElem>("GetPeerMessages", request);
  }

  public UniTask<UnsubscriptionResult> UnsubscribeToPeerMessages(Subscription request)
  {
      return module.CallUnaryProcedure<UnsubscriptionResult>("UnsubscribeToPeerMessages", request);
  }

  public UniTask<Subscription> SubscribeToSystemMessages(SubscriptionRequest request)
  {
      return module.CallUnaryProcedure<Subscription>("SubscribeToSystemMessages", request);
  }

  public IUniTaskAsyncEnumerable<SystemTopicSubscriptionResultElem> GetSystemMessages(Subscription request)
  {
      return module.CallServerStream<SystemTopicSubscriptionResultElem>("GetSystemMessages", request);
  }

  public UniTask<UnsubscriptionResult> UnsubscribeToSystemMessages(Subscription request)
  {
      return module.CallUnaryProcedure<UnsubscriptionResult>("UnsubscribeToSystemMessages", request);
  }

  public UniTask<PublishToTopicResult> PublishToTopic(PublishToTopicRequest request)
  {
      return module.CallUnaryProcedure<PublishToTopicResult>("PublishToTopic", request);
  }

}
}
