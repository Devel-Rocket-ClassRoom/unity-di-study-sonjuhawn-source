using VContainer;
using VContainer.Unity;

public class AppRootLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<GlobalCounterService>(Lifetime.Singleton);
    }
}
