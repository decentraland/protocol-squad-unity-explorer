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
            public string[] requiredPermissions;
            public string main;
            public string[] tags;
            public SpawnPointsData[] spawnPoints;
            
            public string name;
            
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

            public class SpawnPointsData
            {
                public string name;
                [JsonProperty("default")]
                public bool isDefault;
                // position // TODO - implement this
                public Vector3 cameraTarget; 
            }
        }
    }
}