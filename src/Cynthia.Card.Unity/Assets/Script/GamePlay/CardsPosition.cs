using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cynthia.Card;
using Alsein.Utilities;
using System.Linq;

public class CardsPosition : MonoBehaviour
{
    public static GameObject DragCard = null;
    public GameObject CardPrefab;
    public float Size = 1.75f;
    public float Width = 14f;
    public bool IsCanDrag;//其中卡牌是否可拖动
    public bool IsCanSelect;//其中卡牌是否可被选中
    public int MaxCards = 9;
    private int temCardIndex;

    private void Start()
    {
        ResetCards();
        temCardIndex = -1;
    }
    public bool IsTem()
    {
        return temCardIndex >= 0;
    }
    public void AddTemCard(GameCard cardInfo,int index)
    {
        if (index == temCardIndex)//如果临时卡存在
        {
            return;//返回
        }
        if (IsTem())
        {
            RemoveCard(temCardIndex);//删除现有临时卡
        }
        if (cardInfo == null)
        {
            temCardIndex = -1;
            return;
        }
        temCardIndex = index;
        var newCard = Instantiate(CardPrefab);
        newCard.GetComponent<CardShowInfo>().CurrentCore = cardInfo;
        newCard.GetComponent<CardShowInfo>().SetCard();
        newCard.GetComponent<CardMoveInfo>().IsCanSelect = false;
        newCard.GetComponent<CardShowInfo>().SetGray(true);
        newCard.transform.SetParent(transform);
        newCard.transform.SetSiblingIndex(temCardIndex);
        ResetCards();
    }

    public void ResetCards()//将所有卡牌定位到应有的位置
    {
        var size = Size;
        var count = transform.childCount;
        if ((count - 1f) * size > Width)
        {
            size = Width / (count - 1f);
        }
        for (var i = 0; i < count; i++)
        {
            var item = transform.GetChild(i).gameObject.GetComponent<CardMoveInfo>();
            item.IsCanDrag = IsCanDrag;
            item.transform.localScale = Vector3.one;
            item.SetResetPoint(new Vector3(-(count - 1f) * size / 2f + i * size, 0, -0.1f - 0.01f * i));
        }
    }
    public void CardsCanDrag(bool isCanDrag)
    {//疑点---?设定子物体中所有的卡牌无法拖动
        IsCanDrag = isCanDrag;
        var count = transform.childCount;
        for (var i = 0; i < count; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<CardMoveInfo>().IsCanDrag = IsCanDrag;
        }
    }
    public void CardsCanSelect(bool isCanSelect)
    {//疑点---?设定子物体中所有的卡牌无法拖动
        IsCanSelect = isCanSelect;
        var count = transform.childCount;
        for (var i = 0; i < count; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<CardMoveInfo>().IsCanSelect = IsCanSelect;
        }
    }
    public void AddCard(CardMoveInfo card, int cardIndex)
    {
        if (IsTem())
        {//添加卡牌的时候删除临时卡
            AddTemCard(null, -1);
        }
        card.IsCanDrag = IsCanDrag;
        var source = card.transform.parent.gameObject.GetComponent<CardsPosition>();
        card.transform.SetParent(transform);
        card.transform.SetSiblingIndex(cardIndex == -1 ? transform.childCount : cardIndex);
        card.transform.localScale = Vector3.one;
        if (source != null)
            source.ResetCards();
        ResetCards();
    }
    public void RemoveCard(int cardIndex)
    {
        var card = transform.GetChild(cardIndex).gameObject;
        card.transform.SetParent(null);
        //card.transform = false;
        Destroy(card);
        ResetCards();
    }
    public void CreateCard(CardMoveInfo card, int cardIndex)
    {
        card.IsCanDrag = IsCanDrag;
        card.transform.SetParent(transform);
        card.transform.SetSiblingIndex(cardIndex == -1 ? transform.childCount : cardIndex);
        card.transform.localScale = Vector3.one;
        ResetCards();
    }
    public void SetCards(IEnumerable<CardMoveInfo> Cards)
    {
        Cards.ForAll(x=>CreateCard(x,-1));
    }
    public void SetCards(IEnumerable<GameCard> Cards)
    {
        Cards.Select(x=> 
        {
            var newCard = Instantiate(CardPrefab);
            newCard.GetComponent<CardShowInfo>().CurrentCore = x;
            newCard.GetComponent<CardMoveInfo>().CardUseInfo = GwentMap.CardMap[x.CardIndex].CardUseInfo;
            newCard.GetComponent<CardShowInfo>().SetCard();
            return newCard.GetComponent<CardMoveInfo>();
        }).To(SetCards);
    }
}
