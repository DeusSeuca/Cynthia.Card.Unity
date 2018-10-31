using System.Collections;
using System.Collections.Generic;
using Autofac;
using UnityEngine;
using Cynthia.Card.Client;
using Cynthia.Card;

public class TestButton : MonoBehaviour
{
    public GameObject TestItem;
    public int Number;
    public int a = -9;
    public void Click()
    {
        //var item = TestItem.GetComponent<GameEvent>();
        //item.ShowCardBreakEffect(new CardLocation() { RowPosition = RowPosition.MyRow1, CardIndex = 0 }, CardBreakEffectType.Banish);
    }
    private void Update()
    {
    }
}
