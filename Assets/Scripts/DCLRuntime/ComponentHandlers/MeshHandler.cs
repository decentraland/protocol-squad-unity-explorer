using System.Collections.Generic;
using DCL.ECSComponents;
using DCL.Helpers;
using DCLRuntime.AssetsDB;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace DCLRuntime.ComponentHandlers
{
    public class MeshHandler
    {
        private readonly MaterialCache _materialCache;
        private readonly MeshCache _meshCache;
        [Inject, UsedImplicitly]
        public MeshHandler(MaterialCache materialCache, MeshCache meshCache)
        {
            _materialCache = materialCache;
            _meshCache = meshCache;
        }
        
        public void ApplyOn(PBMeshRenderer pbMeshRenderer, GameObject entity)
        {
            Mesh mesh = null;
            switch (pbMeshRenderer.MeshCase)
            {
                case PBMeshRenderer.MeshOneofCase.Box:
                    var uv = pbMeshRenderer.Box.Uvs;
                    mesh =pbMeshRenderer.Box.Uvs == null ? _meshCache.GetCube() : PrimitiveMeshBuilder.BuildCube(1f);
                    if (pbMeshRenderer.Box.Uvs != null && pbMeshRenderer.Box.Uvs.Count > 0)
                    {
                        mesh.uv = FloatArrayToV2List(pbMeshRenderer.Box.Uvs);
                        mesh.RecalculateNormals();
                    }
                    break;
                case PBMeshRenderer.MeshOneofCase.Sphere:
                case PBMeshRenderer.MeshOneofCase.Cylinder:
                case PBMeshRenderer.MeshOneofCase.Plane:
                    Debug.LogError("Unsuported mesh renderer");
                    break;
            }
            
            entity.AddComponent<MeshRenderer>().sharedMaterial = _materialCache.GetStandard();
            entity.AddComponent<MeshFilter>().sharedMesh = mesh;
        }

        private static Vector2[] FloatArrayToV2List(IList<float> uvs)
        {
            var uvsResult = new Vector2[uvs.Count / 2];
            var uvsResultIndex = 0;

            for (var i = 0; i < uvs.Count;) uvsResult[uvsResultIndex++] = new Vector2(uvs[i++], uvs[i++]);

            return uvsResult;
        }
    }
}