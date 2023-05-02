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
        public void DeserializedJson_HasCorrectFields()
        {
            var testRealmJsonFileName = "test_realm.json";
            var json = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, testRealmJsonFileName));
            var realmData = json.ToRealmData();
            Assert.That(realmData.healthy, Is.True);
            Assert.That(realmData.configurations.scenesUrn[2],
                Is.EqualTo(
                    "urn:decentraland:entity:bafkreibkohnvfpienlsfdecj4gwphj3zg4eiq4komunloynahd6v4fqv44" +
                    "?=&baseUrl=https://sdk-team-cdn.decentraland.org/ipfs/"));
        }

        [Test]
        public void DeserializedURN_HasCorrectFields()
        {
            var str =
                "urn:decentraland:entity:bafkreidvjhbobdwnnhwyaqbxskwyaxwc5cfuilzqa3gho6lqu7abcrwsti?=&baseUrl=https://sdk-team-cdn.decentraland.org/ipfs/";
            var urn = UrnFactory.FormatString(str);
            Assert.That(urn.Hash, Is.EqualTo("bafkreidvjhbobdwnnhwyaqbxskwyaxwc5cfuilzqa3gho6lqu7abcrwsti"));
            Assert.That(urn.BaseUrl, Is.EqualTo("https://sdk-team-cdn.decentraland.org/ipfs/"));
            Assert.That(urn.URL, Is.EqualTo("https://sdk-team-cdn.decentraland.org/ipfs/bafkreidvjhbobdwnnhwyaqbxskwyaxwc5cfuilzqa3gho6lqu7abcrwsti"));
        }
    }
}