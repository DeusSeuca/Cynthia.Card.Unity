using Cynthia.Card.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autofac;

public class MainCode : MonoBehaviour
{
    private GlobalUIService _globalUIService;
    private GwentClientService _client;
    public GameObject Context;
    public GameObject MatchUI;

    void Start()
    {
        _globalUIService = DependencyResolver.Container.Resolve<GlobalUIService>();
        _client = DependencyResolver.Container.Resolve<GwentClientService>();
    }
    public async void ExitGameClick()
    {
        if (await _globalUIService.YNMessageBox("退出游戏?", "是否退出游戏"))
        {
            //进行一些处理
            Application.Quit();
            return;
        }
    }
}
