using System.Collections;
using System.Collections.Generic;
using Cynthia.Card.Client;
using Cynthia.Card;
using UnityEngine;

public class GameCardsControl : MonoBehaviour
{
    public GameObject MyHand;
    public GameObject MyRow1;
    public GameObject MyRow2;
    public GameObject MyRow3;
    public GameObject MyLeader;
    //
    public GameObject EnemyHand;
    public GameObject EnemyRow1;
    public GameObject EnemyRow2;
    public GameObject EnemyRow3;
    public GameObject EnemyLeader;
    //暂时用不到
    public GameObject MyCemtery;
    public GameObject MyDeck;
    public GameObject EnemyCemtery;
    public GameObject EnemyDeck;
    //预制体
    public GameObject CardObj;
    //---------------------------
    public void SetCardsInfo(GameInfomation gameInfomation)
    {
        MyHand.GetComponent<CardsModify>();
        MyRow1.GetComponent<CardsModify>();
        MyRow2.GetComponent<CardsModify>();
        MyRow3.GetComponent<CardsModify>();
        EnemyHand.GetComponent<CardsModify>();
        EnemyRow1.GetComponent<CardsModify>();
        EnemyRow2.GetComponent<CardsModify>();
        EnemyRow3.GetComponent<CardsModify>();
    }
}
