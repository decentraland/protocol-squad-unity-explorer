using System.Collections.Generic;
using DCL.ECSComponents;
using DCL.Helpers;
using UnityEngine;

namespace DCLRuntime.ComponentHandlers
{
    internal static class MeshHandler
    {
        public static void ApplyOn(this PBMeshRenderer pbMeshRenderer, GameObject entity)
        {
            Mesh mesh = null;
            switch (pbMeshRenderer.MeshCase)
            {
                case PBMeshRenderer.MeshOneofCase.Box:
                    var uv = pbMeshRenderer.Box.Uvs;
                    mesh = PrimitiveMeshBuilder.BuildCube(1f);
                    if (pbMeshRenderer.Box.Uvs != null && pbMeshRenderer.Box.Uvs.Count > 0)
                        mesh.uv = FloatArrayToV2List(pbMeshRenderer.Box.Uvs);
                    break;
                case PBMeshRenderer.MeshOneofCase.Sphere:
                case PBMeshRenderer.MeshOneofCase.Cylinder:
                case PBMeshRenderer.MeshOneofCase.Plane:
                    Debug.LogError("Unsuported mesh renderer");
                    break;
            }

            mesh.RecalculateNormals();
            entity.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
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