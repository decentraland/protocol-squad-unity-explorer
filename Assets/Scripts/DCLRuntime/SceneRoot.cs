using System.Collections.Generic;
using UnityEngine;

namespace DCLRuntime
{
    /// <summary>
    /// Parent component for all scenes
    /// </summary>
    public class SceneRoot : MonoBehaviour
    {
        private readonly Dictionary<long, GameObject> _entities = new();
        

        public static SceneRoot Create()
        {
            var go = new GameObject("sceneRoot");
            return go.AddComponent<SceneRoot>();
        }

        public void ContainsEntity(long entityId) => _entities.ContainsKey(entityId);


        public GameObject GetCreateEntity(long entityId)
        {
            if (_entities.TryGetValue(entityId, out var entity)) return entity;
            entity = new GameObject("E-" + entityId);
            gameObject.transform.parent = transform;
            _entities.Add(entityId, entity);

            return entity;
        }
    }
}