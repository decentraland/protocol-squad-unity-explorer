using RemoteData;
using UnityEngine;

namespace DCLRuntime
{
    [RequireComponent(typeof(SceneEntityManager))]
    public class SceneRoot : MonoBehaviour
    {
        public static SceneRoot Create(SceneData sceneData)
        {
            var gameObject = new GameObject(sceneData.metadata.name);
            gameObject.AddComponent<SceneEntityManager>();
            return gameObject.AddComponent<SceneRoot>();
        }
    }
}