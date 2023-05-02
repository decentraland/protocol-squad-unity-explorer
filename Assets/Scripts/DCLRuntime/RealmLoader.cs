using System;
using Cysharp.Threading.Tasks;
using RemoteData;
using UnityEngine;
using UnityEngine.Networking;

namespace DCLRuntime
{
    public class RealmLoader : MonoBehaviour
    {
        [SerializeField] private string endPoint = "https://sdk-team-cdn.decentraland.org/ipfs/goerli-plaza-main/about";


        public async UniTask Start()
        {
            var realmDataJson = (await UnityWebRequest.Get(endPoint).SendWebRequest()).downloadHandler.text;
            var realmData = realmDataJson.ToRealmData();

            foreach (var sceneUrn in realmData.configurations.scenesUrn)
            {
                var urn = Urn.FormatString(sceneUrn);
                Debug.Log($"loading {urn.URL}");
                var sceneJson = (await UnityWebRequest.Get(urn.URL).SendWebRequest()).downloadHandler.text;
                var sceneData = sceneJson.ToSceneData();
                SceneRoot.Create(sceneData);
            }
        }
    }
}