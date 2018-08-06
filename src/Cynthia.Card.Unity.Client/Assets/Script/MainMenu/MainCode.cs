using Cynthia.Card.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autofac;

public class MainCode : MonoBehaviour
{
    private GlobalUIService _globalUIService;

    void Start()
    {
        _globalUIService = DependencyResolver.Container.Resolve<GlobalUIService>();
    }
    public async void ExitGameClick()
    {
        if (await _globalUIService.YNMessageBox("退出游戏?", "是否退出游戏"))
        {
            Application.Quit();
            Debug.Log("退出了..?");
            return;
        }
    }
}
