using System.Collections;
using System.Collections.Generic;
using Autofac;
using UnityEngine;
using Cynthia.Card.Client;
using Cynthia.Card;

public class TestButton : MonoBehaviour
{
    public GameObject TestItem;
    public void Click()
    {
        var item = TestItem.GetComponent<GameEvent>();
        item.GetCardFrom(RowPosition.EnemyDeck, RowPosition.EnemyStay, 0, new CardStatus() { IsCardBack = true });
        item.SetCardTo(RowPosition.EnemyStay,0,RowPosition.EnemyRow1,0);
    }
}
