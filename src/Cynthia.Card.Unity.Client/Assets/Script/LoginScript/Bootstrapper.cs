using UnityEngine;
using Autofac;
using Alsein.Utilities;
using Microsoft.AspNetCore.SignalR.Client;

public class Bootstrapper : MonoBehaviour {

    public void Awake()
    {
        if (DependencyResolver.Container != null)
            return;
        var builder = new ContainerBuilder();
        builder.Register(x => DependencyResolver.Container).SingleInstance();
        builder.RegisterType<HubConnectionBuilder>().SingleInstance();
        builder.Register(x => DependencyResolver.Container.Resolve<HubConnectionBuilder>().WithUrl("http://cynthia.ovyno.com/hub/gwent").Build()).SingleInstance();
        builder.RegisterAllServices(option=>option.PreserveExistingDefaults());
        DependencyResolver.Container = builder.Build();
    }
}
