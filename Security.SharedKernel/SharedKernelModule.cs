using Autofac;

using Module = Autofac.Module;

namespace Security.Core;
public class SharedKernelModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //builder.RegisterType<ToDoItemSearchService>()
        //    .As<IToDoItemSearchService>().InstancePerLifetimeScope();

    }
}
