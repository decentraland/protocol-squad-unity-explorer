using System.IO;
using Cysharp.Threading.Tasks;
using DCLRuntime;
using UnityEngine;
using UnityEngine.Networking;

namespace RuntimeTests.Manual
{
    public class RunJSModuleFromStreamingAssets : MonoBehaviour
    {
        [SerializeField] private string jsFileName = "cube_waves.js";
        
        private RuntimeSandbox _sandbox;
        async UniTask Start()
        {
            var fullPath = Path.Combine(Application.streamingAssetsPath, jsFileName);
            Debug.Log($"loading scene from {fullPath}");
            using (var request = UnityWebRequest.Get(fullPath))
            {
                await request.SendWebRequest();
                _sandbox = new RuntimeSandbox(request.downloadHandler.text);
                _sandbox.Start();
            }
        }
        
        private void OnDestroy()
        {
            _sandbox.Dispose();
        }
    }
}
