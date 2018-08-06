using System.Collections;
using System.Collections.Generic;
using Autofac;
using UnityEngine;
using Cynthia.Card.Client;

public class TestButton : MonoBehaviour
{
    public async void Click()
    {
        if(await DependencyResolver.Container.Resolve<GlobalUIService>().YNMessageBox("退出游戏?","是否退出游戏"))
        {
            Debug.Log("退出了");
            return;
        }
        Debug.Log("取消了");
    }
}
