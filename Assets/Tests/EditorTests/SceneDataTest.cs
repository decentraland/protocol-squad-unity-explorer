using System.IO;
using NUnit.Framework;
using RemoteData;
using UnityEngine;

namespace EditorTests
{
    public class SceneDataTest
    {
        [Test]
        public void DeserializedSceneData_HasCorrectFields()
        {
            var json = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "test_scene.json"));
            var sceneData = SceneData.FromJson(json);
            Assert.That(sceneData.content[1].file, Is.EqualTo("models/zombieskinbase.png"));
            Assert.That(sceneData.timestamp, Is.EqualTo(1682704677242));
            Assert.That(sceneData.metadata.display.title, Is.EqualTo("zombie-attack"));
            Assert.That(sceneData.metadata.scene.parcels[2], Is.EqualTo(new Vector2Int(79, -1)));
            Assert.That(sceneData.metadata.scene.Base, Is.EqualTo(new Vector2Int(78, -1)));
        }
    }
}