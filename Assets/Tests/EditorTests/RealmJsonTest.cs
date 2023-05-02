using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using RemoteData;
using UnityEngine;

namespace EditorTests
{
    public class RealmJsonTest
    {
        [Test]
        public void TestJsonFile()
        {
            var testRealmJsonFileName = "test_realm.json";
            var json = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, testRealmJsonFileName));
            var realmData = JsonConvert.DeserializeObject<RealmData>(json);
            Assert.That(realmData.healthy, Is.True);
            Assert.That(realmData.configurations.scenesUrn[2],
                Is.EqualTo(
                    "urn:decentraland:entity:bafkreibkohnvfpienlsfdecj4gwphj3zg4eiq4komunloynahd6v4fqv44" +
                    "?=&baseUrl=https://sdk-team-cdn.decentraland.org/ipfs/"));
        }
    }
}