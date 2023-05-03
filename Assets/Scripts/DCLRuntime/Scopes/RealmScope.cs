using Cysharp.Threading.Tasks;
using DCLRuntime.AssetsDB;
using DCLRuntime.ComponentHandlers;
using VContainer;
using VContainer.Unity;

namespace DCLRuntime
{
    public class RealmScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneCreator>(Lifetime.Singleton);
            builder.Register<MaterialCache>(Lifetime.Singleton);
            builder.Register<MeshHandler>(Lifetime.Singleton);
            builder.RegisterInstance(this.GetCancellationTokenOnDestroy());
            builder.RegisterComponentInHierarchy<RealmLoader>();
        }
    }
}