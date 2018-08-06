using UnityEngine;
using Autofac;
using Microsoft.AspNetCore.SignalR.Client;
using System.Reflection;
using System.Linq;
using Alsein.Utilities.LifetimeAnnotations;
using Cynthia.Card.Client;
using Autofac.Extensions.DependencyInjection;
using Alsein.Utilities;
using UnityEngine.UI;
using System;

public class Bootstrapper : MonoBehaviour {
    public InputField TestText;
    public void Awake()
    {
        try
        {
            if (DependencyResolver.Container != null)
                return;
            var builder = new ContainerBuilder();
            builder.Register(x => DependencyResolver.Container).SingleInstance();
            //builder.RegisterType<HubConnectionBuilder>().SingleInstance();
            //builder.Register(x => DependencyResolver.Container.Resolve<HubConnectionBuilder>().WithUrl("http://cynthia.ovyno.com/hub/gwent").Build()).SingleInstance();
            //builder.Populate(DependencyResolver.Container.Resolve<HubConnectionBuilder>().WithUrl("http://cynthia.ovyno.com/hub/gwent").Services);
            var hubConnectionBuilder = new HubConnectionBuilder().WithUrl("http://cynthia.ovyno.com/hub/gwent");
            builder.Populate(hubConnectionBuilder.Services);
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            var services = types.Where(x => x.Name.EndsWith("Service") && x.IsClass && !x.IsAbstract && !x.IsGenericTypeDefinition);
            //services.Select(x => x.Name).ForAll(Debug.Log);
            builder.RegisterTypes(services.Where(x => x.IsDefined(typeof(SingletonAttribute))).ToArray()).PropertiesAutowired().AsSelf().SingleInstance();
            builder.RegisterTypes(services.Where(x => x.IsDefined(typeof(ScopedAttribute))).ToArray()).PropertiesAutowired().AsSelf().InstancePerLifetimeScope();
            builder.RegisterTypes(services.Where(x => x.IsDefined(typeof(TransientAttribute))).ToArray()).PropertiesAutowired().AsSelf().InstancePerDependency();
            //builder.RegisterAllServices(option=>option.PreserveExistingDefaults());
            builder.RegisterType<LocalPlayer>().PropertiesAutowired().AsSelf();
            DependencyResolver.Container = builder.Build();
            DependencyResolver.Container.Resolve<HubConnection>().StartAsync();
        }
        catch (Exception e)
        {
            TestText.text = e.Message;
        }
        //在启动时就链接上服务器
    }
}
