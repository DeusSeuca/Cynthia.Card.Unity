using System.Collections;
using System.Collections.Generic;
using Cynthia.Card.Client;
using Cynthia.Card;
using UnityEngine;
using System.Linq;
using Alsein.Utilities;

public class GameCardsControl : MonoBehaviour
{
    public CardsPosition MyHand;
    public CardsPosition MyRow1;
    public CardsPosition MyRow2;
    public CardsPosition MyRow3;
    public LeaderCard MyLeader;
    //
    public CardsPosition EnemyHand;
    public CardsPosition EnemyRow1;
    public CardsPosition EnemyRow2;
    public CardsPosition EnemyRow3;
    public LeaderCard EnemyLeader;
    //暂时用不到
    public CardsPosition MyCemtery;
    public CardsPosition MyDeck;
    public CardsPosition EnemyCemtery;
    public CardsPosition EnemyDeck;
    //预制体
    public GameObject CardObj;
    //---------------------------
    public void SetCardsInfo(GameInfomation gameInfomation)
    {
        MyHand.SetCards(gameInfomation.MyHandCard);
        MyRow1.SetCards(gameInfomation.MyPlace[0]);
        MyRow2.SetCards(gameInfomation.MyPlace[1]);
        MyRow3.SetCards(gameInfomation.MyPlace[2]);
        EnemyHand.SetCards(gameInfomation.EnemyHandCard);
        EnemyRow1.SetCards(gameInfomation.EnemyPlace[0]);
        EnemyRow2.SetCards(gameInfomation.EnemyPlace[1]);
        EnemyRow3.SetCards(gameInfomation.EnemyPlace[2]);
        MyLeader.SetLeader(gameInfomation.MyLeader,gameInfomation.IsMyLeader);
        EnemyLeader.SetLeader(gameInfomation.EnemyLeader,gameInfomation.IsEnemyLeader);
    }
}
