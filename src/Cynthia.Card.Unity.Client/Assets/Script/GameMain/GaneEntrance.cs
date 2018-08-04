using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autofac;
using Microsoft.AspNetCore.SignalR.Client;

public class GaneEntrance : MonoBehaviour
{
    public GameObject GlobalUI;
    public void Start()
    {
        if (DependencyResolver.Container == null)
        {
            Debug.Log("??????");
        }
        DependencyResolver.Container.Resolve<HubConnection>().StartAsync().Wait();
    }
}
