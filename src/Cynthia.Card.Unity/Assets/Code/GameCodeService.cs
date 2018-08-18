using System;
using System.Threading.Tasks;
using Alsein.Utilities.LifetimeAnnotations;
using Autofac;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using UnityEngine.Audio;
using Alsein.Utilities;
using System.Collections.Generic;
using System.Threading;

namespace Cynthia.Card.Client
{
    [Transient]
    public class GameCodeService
    {
        private GameObject _code;
        public GameCodeService()
        {
            _code = GameObject.Find("Code");
        }

        public void SetGameInfo(GameInfomation gameInfomation)//设定场上信息
        {
            _code.GetComponent<GameCode>().GameUIControl.SetGameInfo(gameInfomation);
            _code.GetComponent<GameCode>().GameCardsControl.SetCardsInfo(gameInfomation);
        }
        public void LeaveGame()//立刻离开游戏,进入主菜单
        {
            _code.GetComponent<GameCode>().LeaveGame();
        }
        public void ShowGameResult(GameResultInfomation gameResult)//设定并展示游戏结束画面
        {
            _code.GetComponent<GameCode>().GameResultControl.ShowGameResult(gameResult);
        }
    }
}
