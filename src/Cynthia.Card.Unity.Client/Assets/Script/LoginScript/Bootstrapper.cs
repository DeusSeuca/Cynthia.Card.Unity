using UnityEngine;
using Autofac;
using Alsein.Utilities;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using Alsein.Utilities.LifetimeAnnotations;

public class Bootstrapper : MonoBehaviour {

    public void Awake()
    {
        if (DependencyResolver.Container != null)
            return;
        var builder = new ContainerBuilder();
        builder.Register(x => DependencyResolver.Container).SingleInstance();
        builder.RegisterType<HubConnectionBuilder>().SingleInstance();
        builder.Register(x => DependencyResolver.Container.Resolve<HubConnectionBuilder>().WithUrl("http://cynthia.ovyno.com/hub/gwent").Build()).SingleInstance();
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        var services = types.Where(x=>x.Name.EndsWith("Service")&&x.IsClass&&!x.IsAbstract&&!x.IsGenericTypeDefinition);
        builder.RegisterTypes(services.Where(x=>x.IsDefined(typeof(SingletonAttribute))).ToArray()).AsSelf().SingleInstance();
        builder.RegisterTypes(services.Where(x => x.IsDefined(typeof(ScopedAttribute))).ToArray()).AsSelf().InstancePerLifetimeScope();
        builder.RegisterTypes(services.Where(x => x.IsDefined(typeof(TransientAttribute))).ToArray()).AsSelf().InstancePerDependency();
        //builder.RegisterAllServices(option=>option.PreserveExistingDefaults());
        DependencyResolver.Container = builder.Build();
        DependencyResolver.Container.Resolve<HubConnection>().StartAsync();
        //在启动时就链接上服务器
        Debug.Log("注入完成");
    }
}
