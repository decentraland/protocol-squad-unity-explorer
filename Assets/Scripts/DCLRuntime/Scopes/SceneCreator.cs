using System.Linq;
using Cysharp.Threading.Tasks;
using DCLRuntime.Utils;
using RemoteData;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;
using VContainer.Unity;

namespace DCLRuntime
{
    public class SceneCreator
    {
        readonly LifetimeScope currentScope;

        LifetimeScope instantScope;

        public SceneCreator(LifetimeScope lifetimeScope)
        {
            currentScope = lifetimeScope;
        }

        public void Create(SceneData sceneData, string urnBaseUrl) => DoCreate(sceneData, urnBaseUrl).Forget();
        

        private async UniTaskVoid DoCreate(SceneData sceneData, string urnBaseUrl)
        {
            var urnFactory = new UrnFactory(urnBaseUrl);
            
            var data = sceneData.content.First(data => data.file == "bin/game.js");

            var sceneUrn = urnFactory.Create(data.hash);

            var sceneJson = (await UnityWebRequest.Get(sceneUrn.URL).SendWebRequest()).downloadHandler.text;
            var sceneJsonWrapper = new SceneJsonWrapper(sceneJson);
            
            using (LifetimeScope.EnqueueParent(currentScope))
            {
                using (LifetimeScope.Enqueue(builder =>
                       {
                           builder.RegisterInstance(urnFactory);
                           builder.RegisterInstance(sceneData);
                           builder.RegisterInstance(sceneJsonWrapper);
                       }))
                {
                    var gameObject = new GameObject(sceneData.metadata.name);
                    gameObject.transform.position = sceneData.metadata.scene.Base.ToWorldPosition();
                    gameObject.AddComponent<SceneScope>();
                }
            }
        }
    }
}