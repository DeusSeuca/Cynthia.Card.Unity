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
        //-------------------------------------------------------------------------------------------
        //更新数据的方法们
        public void SetAllInfo(GameInfomation gameInfomation)//更新全部数据
        {
            _code.GetComponent<GameCode>().GameUIControl.SetGameInfo(gameInfomation);
            _code.GetComponent<GameCode>().GameCardsControl.SetCardsInfo(gameInfomation);
        }
        public void SetMyCemeteryInfo(IList<GameCard> myCemetery)
        {
            _code.GetComponent<GameCode>().GameCardShowControl.MyCemetery = myCemetery;
        }
        public void SetEnemyCemeteryInfo(IList<GameCard> enemyCemetery)
        {
            _code.GetComponent<GameCode>().GameCardShowControl.EnemyCemetery = enemyCemetery;
        }
        //--
        public void SetGameInfo(GameInfomation gameInfomation)//更新数值+胜场数据
        {
            _code.GetComponent<GameCode>().GameUIControl.SetGameInfo(gameInfomation);
        }
        public void SetCardsInfo(GameInfomation gameInfomation)//更新卡牌类型数据
        {
            _code.GetComponent<GameCode>().GameCardsControl.SetCardsInfo(gameInfomation);
        }
        //
        public void SetCoinInfo(bool isBlueCoin)
        {
            _code.GetComponent<GameCode>().GameEvent.SetCoinInfo(isBlueCoin);
        }
        public void SetPointInfo(GameInfomation gameInfomation)
        {
            _code.GetComponent<GameCode>().GameUIControl.SetPointInfo(gameInfomation);
        }
        public void SetCountInfo(GameInfomation gameInfomation)
        {
            _code.GetComponent<GameCode>().GameUIControl.SetCountInfo(gameInfomation);
        }
        public void SetPassInfo(GameInfomation gameInfomation)
        {
            _code.GetComponent<GameCode>().GameUIControl.SetPassInfo(gameInfomation);
        }
        public void SetWinCountInfo(GameInfomation gameInfomation)
        {
            _code.GetComponent<GameCode>().GameUIControl.SetWinCountInfo(gameInfomation);
        }
        public void SetNameInfo(GameInfomation gameInfomation)
        {
            _code.GetComponent<GameCode>().GameUIControl.SetNameInfo(gameInfomation);
        }
        //-------------------------------------------------------------------------------------------
        public void LeaveGame()//立刻离开游戏,进入主菜单
        {
            _code.GetComponent<GameCode>().LeaveGame();
        }
        public void ShowCardsToCemetery(GameCardsPart cards)
        {
            _code.GetComponent<GameCode>().GameEvent.ShowCardsToCemetery(cards);
        }
        public void ShowGameResult(GameResultInfomation gameResult)//设定并展示游戏结束画面
        {
            _code.GetComponent<GameCode>().GameResultControl.ShowGameResult(gameResult);
        }
        public Task<RoundInfo> GetPlayerDrag()//玩家的回合到了
        {
            return _code.GetComponent<GameCode>().GameEvent.GetPlayerDrag();
        }
        public void MyCardEffectEnd()//结束卡牌效果
        {
            _code.GetComponent<GameCode>().GameEvent.MyCardEffectEnd();
        }
        public void RoundEnd()
        {
            _code.GetComponent<GameCode>().GameEvent.RoundEnd();
        }
        public void EnemyDrag(RoundInfo enemyRoundInfo,GameCard cardInfo)
        {
            _code.GetComponent<GameCode>().GameEvent.EnemyDrag(enemyRoundInfo,cardInfo);
        }
        public void EnemyCardEffectEnd()//结束卡牌效果
        {
            _code.GetComponent<GameCode>().GameEvent.EnemyCardEffectEnd();
        }
        public void SetCardTo(RowPosition rowIndex,int cardIndex,RowPosition tagetRowIndex,int tagetCardIndex)
        {
            _code.GetComponent<GameEvent>().SetCardTo(rowIndex, cardIndex, tagetRowIndex, tagetCardIndex);
        }
        public void GetCardFrom(RowPosition getPosition,RowPosition tagetPosition,int tagetCardIndex,GameCard cardInfo)
        {
            _code.GetComponent<GameEvent>().GetCardFrom(getPosition, tagetPosition, tagetCardIndex, cardInfo);
        }
        //-------------------------------------------------
        public Transform GetGameScale()
        {
            return _code.GetComponent<GameCode>().GameScale;
        }
    }
}
