using VContainer;
using VContainer.Unity;

namespace DCLRuntime
{
    public class RealmScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneCreator>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<RealmLoader>();
        }
    }
}