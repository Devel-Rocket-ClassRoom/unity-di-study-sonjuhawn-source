using VContainer;
using VContainer.Unity;

public class SingletoneALifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<UISingletone>();
    }
}
