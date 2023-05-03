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
        
        [Inject, UsedImplicitly]
        public MeshHandler(MaterialCache materialCache)
        {
            _materialCache = materialCache;
        }
        
        public void ApplyOn(PBMeshRenderer pbMeshRenderer, GameObject entity)
        {
            Mesh mesh = null;
            switch (pbMeshRenderer.MeshCase)
            {
                case PBMeshRenderer.MeshOneofCase.Box:
                    mesh = PrimitiveMeshBuilder.BuildCube(1f);
                    if (pbMeshRenderer.Box.Uvs != null && pbMeshRenderer.Box.Uvs.Count > 0)
                    {
                        mesh.uv = FloatArrayToV2List(pbMeshRenderer.Box.Uvs);
                    }
                    break;
                case PBMeshRenderer.MeshOneofCase.Sphere:
                    mesh = PrimitiveMeshBuilder.BuildSphere(1f);
                    break;
                case PBMeshRenderer.MeshOneofCase.Cylinder:
                    mesh = PrimitiveMeshBuilder.BuildCylinder(50, pbMeshRenderer.Cylinder.RadiusTop,
                        pbMeshRenderer.Cylinder.RadiusBottom, 2f, 0f, true, false);
                    break;
                case PBMeshRenderer.MeshOneofCase.Plane:
                    mesh = PrimitiveMeshBuilder.BuildPlaneV2(1f);    
                    
                    if (pbMeshRenderer.Plane.Uvs != null && pbMeshRenderer.Plane.Uvs.Count > 0)
                    {
                        var uvs =FloatArrayToV2List(pbMeshRenderer.Plane.Uvs);
                        mesh.uv = uvs;
                    }
                    else
                    {
                    }
                    break;
            }
            mesh.RecalculateNormals();
            
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