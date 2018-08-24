using System.Collections;
using System.Collections.Generic;
using Cynthia.Card;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CardShowInfo : MonoBehaviour
{
    public GameCard CurrentCore { get=>_currentCore; set
        {
            _currentCore = value;
            if (_currentCore.IsCardBack) return;
            if (_currentCore.CardInfo == null)
            {
                _currentCore.CardInfo = GwentMap.CardMap[_currentCore.CardIndex];
            }
        }
    }
    private GameCard _currentCore;
    //---------------------------
    public Text Strength;
    public Image FactionIcon;
    public Image CardBorder;
    public Image CardImg;
    public Image CardBack;
    public Image CardStatus;
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

    public CardShowInfo(GameCard card)
    {
        CurrentCore = card;
    }
    public void SetGray(bool IsGray)
    {
        CardStatus.gameObject.SetActive(IsGray);
    }
    public void SetCard()
    {
        //设置卡牌是否灰
        if (CurrentCore.IsGray)
            CardStatus.gameObject.SetActive(false);
        if (!CurrentCore.IsGray)
            CardStatus.gameObject.SetActive(true);
        //如果卡牌是背面,设置背面并结束
        if(CurrentCore.IsCardBack)
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
        if (CurrentCore.CardInfo.CardType == CardType.Special)
        {
            Strength.gameObject.SetActive(false);
            return;
        }
        Strength.gameObject.SetActive(true);
        //根据状态进行设置
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
                Strength.color = Color.green;
            if (CurrentCore.HealthStatus < 0)
                Strength.color = Color.red;
        }
        //-----------------------------------------------
    }
}
