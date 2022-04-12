using Autofac;
using Autofac;

namespace Security.Core;
public class CoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //builder.RegisterType<ToDoItemSearchService>()
        //    .As<IToDoItemSearchService>().InstancePerLifetimeScope();
    }
}
