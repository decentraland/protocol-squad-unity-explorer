using Cysharp.Threading.Tasks;
using Microsoft.ClearScript;
using UnityEngine.Assertions;

/// <summary>
///  class which encapsulate scene module defined by java script
/// </summary>
public class SceneModule
{
    private readonly dynamic _sceneModule;
    public readonly bool HasOnStart;
        
    internal SceneModule(dynamic sceneModule) 
    {
        _sceneModule = sceneModule;
        HasOnStart = sceneModule.onStart is not Undefined;
        Assert.IsFalse(sceneModule.onUpdate is Undefined, "Module must have onUpdate");
    }

    public async UniTask OnStart()
    {
        Assert.IsTrue(HasOnStart);
        await _sceneModule.onStart();
    }

    public async UniTask OnUpdate()
    {
        await _sceneModule.onUpdate();
    }

}