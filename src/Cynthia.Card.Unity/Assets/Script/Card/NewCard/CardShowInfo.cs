using System.Collections;
using System.Collections.Generic;
using Cynthia.Card;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CardShowInfo : MonoBehaviour
{
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
        SetCard(card);
    }
    public void SetGray(bool IsGray)
    {
        CardStatus.gameObject.SetActive(IsGray);
    }
    public void SetCard(GameCard card)
    {
        if (card.IsGray)
            CardStatus.gameObject.SetActive(false);
        if (!card.IsGray)
            CardStatus.gameObject.SetActive(true);
        if(card.IsCardBack)
        {
            if (card.DeckFaction == Faction.Monsters)
                CardBack.sprite = MonstersBack;
            if (card.DeckFaction == Faction.Nilfgaard)
                CardBack.sprite = NilfgaardBack;
            if (card.DeckFaction == Faction.NorthernRealms)
                CardBack.sprite = NorthernRealmsBack;
            if (card.DeckFaction == Faction.ScoiaTael)
                CardBack.sprite = ScoiaTaelBack;
            if (card.DeckFaction == Faction.Skellige)
                CardBack.sprite = SkelligeBack;
            return;
        }
        if (card.IsLock)
            LockIcon.SetActive(true);
        if (card.IsResilience)
            Resilience.SetActive(true);
        if (card.IsSpying)
            SpyingIcon.SetActive(true);
        if (card.IsReveal)
            RevealIcon.SetActive(true);
        if (card.Strength + card.HealthStatus > 0)
        {
            Strength.text = (card.Strength + card.HealthStatus).ToString();
            if (card.HealthStatus > 0)
                Strength.color = Color.green;
            if (card.HealthStatus < 0)
                Strength.color = Color.red;
        }
        if (card.CardInfo.Group == Group.Gold || card.CardInfo.Group == Group.Leader)
            CardBorder.sprite = GoldBorder;
        if (card.CardInfo.Group == Group.Silver)
            CardBorder.sprite = SilverBorder;
        if (card.CardInfo.Group == Group.Copper)
            CardBorder.sprite = CopperBorder;
        if (card.CardInfo.Faction == Faction.Monsters)
            FactionIcon.sprite = MonstersIcon;
        if (card.CardInfo.Faction == Faction.Nilfgaard)
            FactionIcon.sprite = NilfgaardIcon;
        if (card.CardInfo.Faction == Faction.NorthernRealms)
            FactionIcon.sprite = NorthernRealmsIcon;
        if (card.CardInfo.Faction == Faction.ScoiaTael)
            FactionIcon.sprite = ScoiaTaelIcon;
        if (card.CardInfo.Faction == Faction.Skellige)
            FactionIcon.sprite = SkelligeIcon;
        if (card.CardInfo.Faction == Faction.Neutral)
            FactionIcon.sprite = NeutralIcon;
        //-----------------------------------------------
    }
}
