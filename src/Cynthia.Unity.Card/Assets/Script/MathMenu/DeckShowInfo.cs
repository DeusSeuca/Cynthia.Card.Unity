using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cynthia.Card.Client;
using Autofac;
using Cynthia.Card;

public class DeckShowInfo : MonoBehaviour
{
    public Text DeckText;
    public Sprite MonsterIcon;
    public Sprite NilfgaardIcon;
    public Sprite ScoiataelIcon;
    public Sprite NorthernreaIcon;
    public Sprite SkelligeIcon;
    public Sprite NeutralIcon;
    public Image HeadIcon;
    private IDictionary<Faction, Sprite> _groupIconMap;
    public void Start()
    {
        _groupIconMap = new Dictionary<Faction, Sprite>
         {
             {Faction.NorthernRealms,NorthernreaIcon},
             {Faction.ScoiaTael,ScoiataelIcon},
             {Faction.Monsters,MonsterIcon},
             {Faction.Skellige,SkelligeIcon},
             {Faction.Nilfgaard,NilfgaardIcon},
         };
    }
    public void SetDeckInfo(string name,Faction faction)
    {
        if (_groupIconMap == null) Start();
        //Debug.Log(faction);
        HeadIcon.sprite = _groupIconMap[faction];
        DeckText.text = name;
    }
}
