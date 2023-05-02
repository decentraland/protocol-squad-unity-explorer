using System.Linq;
using Cysharp.Threading.Tasks;
using DCLRuntime.Utils;
using RemoteData;
using UnityEngine;
using UnityEngine.Networking;

namespace DCLRuntime
{
    [RequireComponent(typeof(SceneEntityManager))]
    public class SceneRoot : MonoBehaviour
    {
        private RuntimeSandbox _sandbox;
        private UrnFactory _urnFactory;
        
        public static SceneRoot Create(SceneData sceneData, UrnFactory urnFactory)
        {
            var gameObject = new GameObject(sceneData.metadata.name);
            var sceneRoot=  gameObject.AddComponent<SceneRoot>();
            sceneRoot.Initialize(sceneData, urnFactory).Forget();
            return sceneRoot;
        }

        private async UniTaskVoid Initialize(SceneData sceneData, UrnFactory urnFactory)
        {
            _urnFactory = urnFactory;
            var entityManager = gameObject.AddComponent<SceneEntityManager>();
            gameObject.transform.position = sceneData.metadata.scene.Base.ToWorldPosition();

            var data = sceneData.content.Where(data => data.file == "bin/game.js").First();

            var sceneUrn = urnFactory.Create(data.hash);
            
            var sceneJson = (await UnityWebRequest.Get(sceneUrn.URL).SendWebRequest()).downloadHandler.text;
            _sandbox = new RuntimeSandbox(sceneJson, entityManager);
            _sandbox.Run();
        }
        
        
        private void OnDestroy()
        {
            _sandbox.Dispose();
        }
    }
}