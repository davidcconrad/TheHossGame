using Autofac;
using TheHossGame.Core.Interfaces;
using TheHossGame.Core.Services;

namespace TheHossGame.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ToDoItemSearchService>()
        .As<IToDoItemSearchService>().InstancePerLifetimeScope();
  }
}
