using DCLRuntime;
using UnityEngine;
using File = System.IO.File;

namespace RuntimeTests.Manual
{
    public class ManualFlatHierarchyTest : MonoBehaviour
    {
        private string FilePath = "Assets/Tests/TestFiles/flat_hierarchy.js";
        private RuntimeSandbox _sandbox;
        void Start()
        {
            var scene = File.ReadAllText(FilePath);
            _sandbox = new RuntimeSandbox(scene);
            _sandbox.Run();
        }
        
        private void OnDestroy()
        {
            _sandbox.Dispose();
        }
    }
}
