using System.Collections;
using System.Collections.Generic;
using Cynthia.Card;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CardShowInfo : MonoBehaviour
{
    public CardMoveInfo CardMoveInfo;
    public CardStatus CurrentCore { get=>_currentCore; set
        {
            _currentCore = value;
            if (_currentCore.IsCardBack) return;
            _currentCore.CardInfo = GwentMap.CardMap[_currentCore.CardId];
        }
    }
    public bool IsGray { get=>_isGray;
        set
        {
            if (_isGray == value)
                return;
            _isGray = value;
            CardStatus.SetActive(IsGray);
        }
    }
    private bool _isGray = false;
    private CardStatus _currentCore;
    //---------------------------
    public Text Strength;
    public Text Armor;
    public Text Countdown;
    public GameObject StrengthShow;
    public GameObject ArmorShow;
    public GameObject CountdownShow;
    //---------------------------
    public Image FactionIcon;
    public Image CardBorder;
    public Image CardImg;
    public Image CardBack;
    public GameObject CardStatus;
    public GameObject LockIcon;//锁定
    public GameObject SpyingIcon;//间谍
    public GameObject Resilience;//坚韧
    public GameObject RevealIcon;//揭示
    //-----------------------------
    public Sprite CopperBorder;
    public Sprite SilverBorder;
    public Sprite GoldBorder;
    //--------
    public Sprite NorthernRealmsIcon;//北方
    public Sprite ScoiaTaelIcon;//松鼠党
    public Sprite MonstersIcon;//怪物
    public Sprite SkelligeIcon;//群岛
    public Sprite NilfgaardIcon;//帝国
    public Sprite NeutralIcon;//中立
    //--------
    public Sprite NorthernRealmsBack;//北方
    public Sprite ScoiaTaelBack;//松鼠党
    public Sprite MonstersBack;//怪物
    public Sprite SkelligeBack;//群岛
    public Sprite NilfgaardBack;//帝国
    //----------------------------------
    //客户端相关
    public Image SelectCenter;
    public Image SelectMargin;
    public GameObject SelectIcon;

    public CardShowInfo(CardStatus card)
    {
        CurrentCore = card;
    }/*
    public void SetGray(bool IsGray)
    {
        if(CurrentCore!=null)
            CurrentCore.IsGray = IsGray;
        CardStatus.SetActive(IsGray);
    }*/
    public void SetSelect(bool center,bool margin,bool isLight = false)
    {
        SelectCenter.gameObject.SetActive(center);
        SelectMargin.gameObject.SetActive(margin);
        SelectIcon.SetActive(center || margin);
        if (isLight)
        {
            SelectCenter.color = new Color(0, 160f/255f, 1);
            SelectMargin.color = new Color(0, 160f/255f, 1);
        }
        else
        {
            SelectCenter.color = new Color(0, 180f/255f, 1);
            SelectMargin.color = new Color(0, 180f/255f, 1);
        }
    }
    public void SetCard()
    {
        var iconCount = 0;
        var use = this.GetComponent<CardMoveInfo>();
        if (use != null&&!CurrentCore.IsCardBack)
            use.CardUseInfo = CurrentCore.CardInfo.CardUseInfo;
        CardImg.sprite = Resources.Load<Sprite>("Sprites/Cards/"+CurrentCore.CardArtsId);
        //设置卡牌是否灰(转移到属性)
        //如果卡牌是背面,设置背面并结束
        CardBack.gameObject.SetActive(false);
        if (CurrentCore.IsCardBack)
        {
            if (CurrentCore.DeckFaction == Faction.Monsters)
                CardBack.sprite = MonstersBack;
            if (CurrentCore.DeckFaction == Faction.Nilfgaard)
                CardBack.sprite = NilfgaardBack;
            if (CurrentCore.DeckFaction == Faction.NorthernRealms)
                CardBack.sprite = NorthernRealmsBack;
            if (CurrentCore.DeckFaction == Faction.ScoiaTael)
                CardBack.sprite = ScoiaTaelBack;
            if (CurrentCore.DeckFaction == Faction.Skellige)
                CardBack.sprite = SkelligeBack;
            CardBack.gameObject.SetActive(true);
            return;
        }
        if (CurrentCore.CardInfo.Group == Group.Gold || CurrentCore.CardInfo.Group == Group.Leader)
            CardBorder.sprite = GoldBorder;
        if (CurrentCore.CardInfo.Group == Group.Silver)
            CardBorder.sprite = SilverBorder;
        if (CurrentCore.CardInfo.Group == Group.Copper)
            CardBorder.sprite = CopperBorder;
        if (CurrentCore.CardInfo.Faction == Faction.Monsters)
            FactionIcon.sprite = MonstersIcon;
        if (CurrentCore.CardInfo.Faction == Faction.Nilfgaard)
            FactionIcon.sprite = NilfgaardIcon;
        if (CurrentCore.CardInfo.Faction == Faction.NorthernRealms)
            FactionIcon.sprite = NorthernRealmsIcon;
        if (CurrentCore.CardInfo.Faction == Faction.ScoiaTael)
            FactionIcon.sprite = ScoiaTaelIcon;
        if (CurrentCore.CardInfo.Faction == Faction.Skellige)
            FactionIcon.sprite = SkelligeIcon;
        if (CurrentCore.CardInfo.Faction == Faction.Neutral)
            FactionIcon.sprite = NeutralIcon;
        if(CurrentCore.IsCountdown)
        {
            CountdownShow.SetActive(true);
            Countdown.text = CurrentCore.Countdown.ToString();
            iconCount++;
        }
        else
            CountdownShow.SetActive(false);
        if (CurrentCore.CardInfo.CardType == CardType.Special)
        {
            Strength.gameObject.SetActive(false);
            FactionIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(50,100);
            return;
        }
        Strength.gameObject.SetActive(true);
        iconCount++;
        //根据状态进行设置
        Armor.text = CurrentCore.Armor.ToString();
        if (CurrentCore.Armor > 0)
        {
            ArmorShow.SetActive(true);
            iconCount++;
        }
        else
            ArmorShow.SetActive(false);
        if (CurrentCore.IsLock)
            LockIcon.SetActive(true);
        if (CurrentCore.IsResilience)
            Resilience.SetActive(true);
        if (CurrentCore.IsSpying)
            SpyingIcon.SetActive(true);
        if (CurrentCore.IsReveal)
            RevealIcon.SetActive(true);
        if (CurrentCore.Strength + CurrentCore.HealthStatus > 0)
        {
            Strength.text = (CurrentCore.Strength + CurrentCore.HealthStatus).ToString();
            if (CurrentCore.HealthStatus > 0)
                Strength.color = ConstInfo.GreenColor;
            if (CurrentCore.HealthStatus < 0)
                Strength.color = ConstInfo.RedColor;
        }
        FactionIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50+(iconCount==0?1:iconCount)*50);
        //-----------------------------------------------
    }
}
