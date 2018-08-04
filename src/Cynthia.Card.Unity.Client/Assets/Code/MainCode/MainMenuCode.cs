using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cynthia.Card.Client;
using Autofac;
using Microsoft.AspNetCore.SignalR.Client;

public class MainMenuCode : MonoBehaviour {

	public GwentClientService Client { get; set; }
    void Start()
    {
        var a = DependencyResolver.Container.Resolve<HubConnection>();
        a.SendAsync("Register","Test","Test","Test");
    }
}
