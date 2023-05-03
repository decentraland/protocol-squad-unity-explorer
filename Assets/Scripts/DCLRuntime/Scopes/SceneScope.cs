using VContainer;
using VContainer.Unity;

namespace DCLRuntime
{
    public class SceneScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(gameObject.AddComponent<SceneEntityManager>());
            builder.Register<ComponentManager>(Lifetime.Singleton);
            builder.Register<EngineApi>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterEntryPoint<RuntimeSandbox>();
        }
    }
}