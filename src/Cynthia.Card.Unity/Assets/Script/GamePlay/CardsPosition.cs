using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cynthia.Card;
using Alsein.Utilities;
using System.Linq;
using System.Threading.Tasks;

public class CardsPosition : MonoBehaviour
{
    public GameObject CardPrefab;
    public float XSize;
    public float YSize;
    public float Width;
    public bool IsCanDrag;
    public bool IsCanSelect;
    public bool IsLock;//是否锁定住一端
    public bool IsCoverage;//是否靠前的卡牌覆盖靠后的卡牌
    public bool IsStayRow;//是否是特殊排(待出现)
    public int MaxCards;
    public RowPosition Id;
    private int _temCardIndex;

    private void Start()
    {
        ResetCards();
        _temCardIndex = -1;
    }
    public bool IsTem()
    {
        return _temCardIndex >= 0;
    }
    public void AddTemCard(CardStatus cardInfo,int index)
    {
        if (index == _temCardIndex)//如果临时卡存在
        {
            return;//返回
        }
        if (IsTem())
        {
            RemoveCard(_temCardIndex);//删除现有临时卡
        }
        if (cardInfo == null)
        {
            _temCardIndex = -1;
            return;
        }
        _temCardIndex = index;
        var newCard = Instantiate(CardPrefab);
        newCard.GetComponent<CardShowInfo>().CurrentCore = cardInfo;
        newCard.GetComponent<CardShowInfo>().CurrentCore.IsGray = true;
        newCard.GetComponent<CardMoveInfo>().IsCanSelect = false;
        newCard.GetComponent<CardShowInfo>().SetCard();
        newCard.transform.SetParent(transform);
        newCard.transform.SetSiblingIndex(_temCardIndex);
        newCard.transform.localPosition = new Vector3((IsLock?0:(-(transform.childCount - 1f) * XSize / 2f)) + index * XSize, -YSize * index, -0.1f - 0.01f * index);
        ResetCards();
    }

    public void ResetCards()//将所有卡牌定位到应有的位置
    {
        var size = XSize;
        var count = transform.childCount;
        if ((count - 1f) * size > Width)
        {
            size = Width / (count - 1f);
        }
        for (var i = 0; i < count; i++)
        {
            var item = transform.GetChild(i).gameObject.GetComponent<CardMoveInfo>();
            item.IsCanDrag = IsCanDrag;
            item.IsCanSelect = IsCanSelect;
            if(!item.IsOn)//如果没使用的话,恢复
                item.transform.localScale = Vector3.one;
            item.Speed = 5f;
            item.SetResetPoint(new Vector3((IsLock?0:(-(count - 1f) * size / 2f)) + i * size, -YSize*i,IsCoverage?(-0.1f-0.01f*(count-i-1)):(-0.1f - 0.01f * i)));
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
    public void AddCard(CardMoveInfo card, int cardIndex)//,bool isAwait = true)
    {
        if (IsTem())
        {//添加卡牌的时候删除临时卡
            AddTemCard(null, -1);
        }
        card.IsCanDrag = IsCanDrag;
        var source = card.transform.parent.gameObject.GetComponent<CardsPosition>();
        var leader = card.transform.parent.gameObject.GetComponent<LeaderCard>();
        card.transform.SetParent(transform);
        card.transform.SetSiblingIndex(cardIndex == -1 ? transform.childCount : cardIndex);
        card.IsCanDrag = IsCanDrag;
        if (source != null)
            source.ResetCards();
        if (leader != null)
            leader.TrueCard = null;
        ResetCards();
        /*
        if (isAwait)
        {
            card.IsCanSelect = false;
            card.IsOn = true;
            if (IsStayRow)
                return;
            card.transform.localScale = Vector3.one * 1.2f;
            card.ZPosition -= 2f;
        }
        else
        {
            card.transform.localScale = Vector3.one;
            card.IsCanSelect = IsCanSelect;
            card.IsOn = false;
        }*/
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
    public int GetCardCount()
    {
        return transform.childCount;
    }
    public void SetCards(IEnumerable<CardStatus> Cards)
    {
        Cards.Select(x=> 
        {
            var newCard = Instantiate(CardPrefab);
            newCard.GetComponent<CardShowInfo>().CurrentCore = x;
            if (x.IsCardBack == false)
            {
                newCard.GetComponent<CardMoveInfo>().CardUseInfo = GwentMap.CardMap[x.CardIndex].CardUseInfo;
            }
            newCard.GetComponent<CardShowInfo>().SetCard();
            return newCard.GetComponent<CardMoveInfo>();
        }).To(SetCards);
    }
}
