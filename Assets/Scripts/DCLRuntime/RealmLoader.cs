using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using RemoteData;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;

namespace DCLRuntime
{
    public class RealmLoader : MonoBehaviour
    {
        [SerializeField] private string endPoint = "https://sdk-team-cdn.decentraland.org/ipfs/goerli-plaza-main/about";
        [Inject] [UsedImplicitly] private SceneCreator _sceneCreator;
        [Inject] [UsedImplicitly] private CancellationToken _globalCancellationToken;

        public async void Start()
        {
            var realmDataJson = (await UnityWebRequest.Get(endPoint).SendWebRequest()).downloadHandler.text;
            var realmData = realmDataJson.ToRealmData();

            UrnFactory urnFactory = null;

            foreach (var sceneUrn in realmData.configurations.scenesUrn)
            {
                var urn = UrnFactory.FormatString(sceneUrn);
                urnFactory ??= new UrnFactory(urn.BaseUrl);

                Debug.Log($"loading {urn.URL}");
                var sceneJson = (await UnityWebRequest.Get(urn.URL).SendWebRequest().WithCancellation(_globalCancellationToken)).downloadHandler.text;
                _globalCancellationToken.ThrowIfCancellationRequested();
                var sceneData = sceneJson.ToSceneData();
                _sceneCreator.Create(sceneData, urn.BaseUrl);
            }
        }
    }
}