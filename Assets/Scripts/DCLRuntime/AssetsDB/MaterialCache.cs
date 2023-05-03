using System.Collections.Generic;
using UnityEngine;

namespace DCLRuntime.AssetsDB
{
    public class MaterialCache
    {
        private Dictionary<string, Material> materialByShader = new();

        private Material GetMaterial(string shaderName)
        {
            if (materialByShader.TryGetValue(shaderName, out var m))
            {
                return m;
            }
            var material = new Material(Shader.Find(shaderName));

            materialByShader.Add(shaderName, material);
            return material;
        }

        public Material GetStandard()
        {
            return GetMaterial("Standard");
        }
    }
}