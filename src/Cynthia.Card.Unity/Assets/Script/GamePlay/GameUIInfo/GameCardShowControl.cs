using Cynthia.Card;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameCardShowControl : MonoBehaviour
{
    public GameObject UICardPrefab;
    public GameObject NullCardPrefab;
    public RectTransform CardsContent;
    public Text ShowCardMessage;
    public Scrollbar Scroll;
    public GameObject CardSelectUI;

    public IList<GameCard> MyCemetery;
    public IList<GameCard> EnemyCemetery;

    private void Update()
    {
        //ShowCardMessage.text = (int.Parse(ShowCardMessage.text)+1).ToString();
    }
    public void ShowMyCemetery()
    {
        if (MyCemetery == null || MyCemetery.Count == 0)
            return;
        ShowCardMessage.text = "我方墓地";
        SetCardInfo(MyCemetery);
        CardSelectUI.SetActive(true);
    }
    public void ShowEnemyCemetery()
    {
        if (EnemyCemetery == null || EnemyCemetery.Count == 0)
            return;
        ShowCardMessage.text = "敌方墓地";
        SetCardInfo(EnemyCemetery);
        CardSelectUI.SetActive(true);
    }
    public void SetCardInfo(IList<GameCard> cards)
    {
        var count = cards.Count;
        RemoveAllChild();
        for (var i = 0; i < count; i++)
        {
            var card = Instantiate(UICardPrefab).GetComponent<CardShowInfo>();
            card.CurrentCore = cards[i];
            if (card.CurrentCore.CardInfo == null && !card.CurrentCore.IsCardBack)
                card.CurrentCore.CardInfo = GwentMap.CardMap[card.CurrentCore.CardIndex];
            card.SetCard();
            card.transform.SetParent(CardsContent, false);
        }
        var nullcount = count <= 10 ? 10 - count : (count % 5 == 0 ? 0 : 5 - count % 5);
        for (var i = 0; i < nullcount; i++)
        {
            var card = Instantiate(NullCardPrefab);
            card.transform.SetParent(CardsContent, false);
        }
        //------------------------------------------------------------------------
        var height = count <= 10 ? 780f : (108f + 276 * (count % 5 > 0 ? count / 5 + 1 : count / 5));
        CardsContent.sizeDelta = new Vector2(0, height);
        if (count <= 10)
            CardsContent.GetComponent<GridLayoutGroup>().padding.top = 190;
        else
            CardsContent.GetComponent<GridLayoutGroup>().padding.top = 130;
        Scroll.value = 1;
    }
    public void RemoveAllChild()
    {
        for (var i = CardsContent.childCount - 1; i >= 0; i--)
        {
            Destroy(CardsContent.GetChild(i).gameObject);
        }
        CardsContent.DetachChildren();
    }
}
