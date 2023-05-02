using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace RemoteData
{
    public static class SceneDataDeserializer
    {
        public static SceneData ToSceneData(this string jsonString)
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new Vector2IntConverter() }
            };
            
            return JsonConvert.DeserializeObject<SceneData>(jsonString, settings);
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