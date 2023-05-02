// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace RemoteData
{
    public class SceneData
    {
        public ContentData[] content;
        public PointerData[] pointers;
        public long timestamp;
        public string type;
        public Metadata metadata;
        public string version;

        public static SceneData FromJson(string jsonString)
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new Vector2IntConverter() }
            };
            
            return JsonConvert.DeserializeObject<SceneData>(jsonString, settings);
        } 
        
        public class ContentData
        {
            public string file;
            public string hash;
        }

        public class PointerData // TODO: fix that 
        {
            public bool stub;
        }

        public class Metadata
        {
            public bool ecs7;
            public string runtimeVersion;
            public DisplayData display;
            public ContactData contact;
            public string owner;
            public SceneData scene;
            public RequiredPermissionData[] requiredPermissions;
            public string main;
            public string[] tags;
            public SpawnPointsData[] spawnPoints;
            
            private string name;
            
            public class DisplayData
            {
                public string title;
                public string favicon;
            }

            public class ContactData
            {
                public string name;
                public string email;
            }
            
            public class SceneData
            {
                public Vector2Int[] parcels;
                [JsonProperty("base")]
                public Vector2Int Base;
            }
            
            public class RequiredPermissionData // TODO: define props
            {
                public bool stub;
            }

            public class SpawnPointsData
            {
                public string name;
                [JsonProperty("default")]
                public bool isDefault;
                // position // TODO - implement this
                public Vector3 cameraTarget; 
            }
        }
        
        
        
        
        private class Vector2IntConverter : JsonConverter<Vector2Int>
        {
            public override Vector2Int ReadJson(JsonReader reader, Type objectType, Vector2Int existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                string[] parts = ((string)reader.Value).Split(',');
                int x = int.Parse(parts[0]);
                int y = int.Parse(parts[1]);
                return new Vector2Int(x, y);
            }

            public override void WriteJson(JsonWriter writer, Vector2Int value, JsonSerializer serializer)
            {
                string stringValue = value.x + "," + value.y;
                writer.WriteValue(stringValue);
            }
        }   
    }
}