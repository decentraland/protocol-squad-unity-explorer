using DCL.Helpers;
using UnityEngine;

namespace DCLRuntime.AssetsDB
{
    public class MeshCache
    {
        private readonly Mesh _standardCubeSize1;

        public MeshCache()
        {
            _standardCubeSize1 = PrimitiveMeshBuilder.BuildCube(1f);
            _standardCubeSize1.RecalculateNormals();
        }
        
        public Mesh GetCube()
        {
            return _standardCubeSize1;
        }
    }
}