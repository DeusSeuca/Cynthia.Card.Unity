using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cynthia.Card.Client;
using Cynthia.Card;
using System.Linq;

public class GameUIControl : MonoBehaviour
{
    public Text MyHandCount;//
    public Text MyCemeteryCount;//
    public Text MyDeckCount;//
    public Text EnemyHandCount;//
    public Text EnemyCemeteryCount;//
    public Text EnemyDeckCount;//
    //-----------------------------------
    public Text MyRow1Point;
    public Text MyRow2Point;
    public Text MyRow3Point;
    public Text MyAllPoint;
    public Text EnemyRow1Point;
    public Text EnemyRow2Point;
    public Text EnemyRow3Point;
    public Text EnemyAllPoint;
    //-----------------------------------
    public Text MyName;//
    public Text EnemyName;//
    //-----------------------------------
    public GameObject MyCrownLeft;//
    public GameObject MyCrownRight;//
    public GameObject EnemyCrownLeft;//
    public GameObject EnemyCrownRight;//
    //----------------------------------

    public void SetGameInfo(GameInfomation gameInfomation)
    {
        //****关于卡牌相关还没写
        //------------------------------------
        //各种数量
        MyHandCount.text = gameInfomation.MyHandCard.Count().ToString();
        EnemyHandCount.text = gameInfomation.EnemyHandCard.Count().ToString();
        MyCemeteryCount.text = gameInfomation.MyCemetery.Count().ToString();
        EnemyCemeteryCount.text = gameInfomation.EnemyCemetery.Count().ToString();
        MyDeckCount.text = gameInfomation.MyDeckCount.ToString();
        EnemyDeckCount.text = gameInfomation.EnemyDeckCount.ToString();
        //------------------------------------
        //名称
        EnemyName.text = gameInfomation.EnemyName;
        MyName.text = gameInfomation.MyName;
        //-------------------------------------
        //皇冠图标
        if (gameInfomation.MyWinCount == 0)
        {
            MyCrownLeft.SetActive(false);
            MyCrownRight.SetActive(false);
        }
        if (gameInfomation.MyWinCount == 1)
        {
            MyCrownLeft.SetActive(true);
            MyCrownRight.SetActive(false);
        }
        if (gameInfomation.MyWinCount == 2)
        {
            MyCrownLeft.SetActive(true);
            MyCrownRight.SetActive(true);
        }
        if (gameInfomation.EnemyWinCount == 0)
        {
            EnemyCrownLeft.SetActive(false);
            EnemyCrownRight.SetActive(false);
        }
        if (gameInfomation.EnemyWinCount == 1)
        {
            EnemyCrownLeft.SetActive(true);
            EnemyCrownRight.SetActive(false);
        }
        if (gameInfomation.EnemyWinCount == 2)
        {
            EnemyCrownLeft.SetActive(true);
            EnemyCrownRight.SetActive(true);
        }
    }
}
