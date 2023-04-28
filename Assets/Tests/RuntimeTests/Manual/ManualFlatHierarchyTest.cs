using DCLRuntime;
using UnityEngine;
using File = System.IO.File;

namespace RuntimeTests.Manual
{
    public class ManualFlatHierarchyTest : MonoBehaviour
    {
        [SerializeField] private string jsPath = "Assets/Tests/TestFiles/cube_waves.js";
        
        //private string FilePath = "Assets/Tests/TestFiles/flat_hierarchy.js";
        private RuntimeSandbox _sandbox;
        void Start()
        {
            var scene = File.ReadAllText(jsPath);
            _sandbox = new RuntimeSandbox(scene);
            _sandbox.Run();
        }
        
        private void OnDestroy()
        {
            _sandbox.Dispose();
        }
    }
}
