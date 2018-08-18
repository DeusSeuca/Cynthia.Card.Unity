using System.Collections;
using System.Collections.Generic;
using Cynthia.Card.Client;
using Cynthia.Card;
using UnityEngine;
using Alsein.Utilities;
using System.Linq;
using Autofac;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchInfo : MonoBehaviour
{
    public GameObject LaderPrefab;
    public GameObject CardPrefab;
    public GameObject DeckPrefab;
    //-------------------------------------------
    public Transform CardsContext;
    public Transform DecksContext;
    public Text GC;
    public Text SC;
    public Text CC;

    public Text DeckName;
    public Transform DeckNameBackground;
    public Transform DeckIcon;
    //-------------------------------------------
    public Sprite NorthernRealmsIcon;
    public Sprite ScoiaTaelIcon;
    public Sprite MonstersIcon;
    public Sprite SkelligeIcon;
    public Sprite NilfgaardIcon;
    //-------------------------------------------
    public GameObject ReturnButton;
    public GameObject SwitchButton;
    public GameObject CloseButton;
    public GameObject MatchButton;
    public GameObject DeckSwitch;
    public GameObject CardsScrollbar;
    public GameObject DecksScrollbar;
    public Text MatchButtonText;
    public Text MatchMessage;
    //-------------------------------------------
    public int CurrentDeckIndex { get; private set; }
    public bool IsDoingMatch { get; private set; }

    private GwentClientService _client;
    private IDictionary<Faction, Sprite> _groupIconMap;

    void Start()
    {
        IsDoingMatch = false;
        _client = DependencyResolver.Container.Resolve<GwentClientService>();
        _groupIconMap = new Dictionary<Faction, Sprite>
         {
             {Faction.NorthernRealms,NorthernRealmsIcon},
             {Faction.ScoiaTael,ScoiaTaelIcon},
             {Faction.Monsters,MonstersIcon},
             {Faction.Skellige,SkelligeIcon},
             {Faction.Nilfgaard,NilfgaardIcon},
         };

        SetDeck(_client.User.Decks[0],0);
        SetDeckList(_client.User.Decks);
    }
    public void Match()/////待编辑
    {
        ReturnButton.SetActive(false);
        SwitchButton.SetActive(false);
        MatchMessage.text = "寻找对手中";
        MatchButtonText.text = "停止匹配";
    }
    public void StopMatch()/////待编辑
    {
        ReturnButton.SetActive(true);
        SwitchButton.SetActive(true);
        MatchMessage.text = "牌组就绪";
        MatchButtonText.text = "开始战斗";
    }
    public async void MatchButtonClick()/////待编辑
    {
        if (IsDoingMatch)
        {
            await _client.StopMatch();
            return;
        }
        //开始匹配
        if (!await _client.Match(CurrentDeckIndex))
        {
            Debug.Log("发送未知错误,匹配失败");
        }
        IsDoingMatch = true;
        Match();
        if (await _client.MatchResult())
        {
            //Debug.Log("成功匹配,进入游戏");
            SceneManager.LoadScene("GamePlay");
            return;
        }
        Debug.Log("成功停止匹配");
        IsDoingMatch = false;
        StopMatch();
    }
    public void SwitchDeckOpen()
    {
        ReturnButton.SetActive(false);
        SwitchButton.SetActive(false);
        MatchButton.SetActive(false);
        CloseButton.SetActive(true);
        MatchMessage.text = "选择牌组";
        DeckNameBackground.gameObject.SetActive(false);
        DecksScrollbar.GetComponent<Scrollbar>().value = 1;
        DeckSwitch.GetComponent<Animator>().Play("SwitchDeckOpen");
    }
    public void MatchReset()
    {
        ReturnButton.SetActive(true);
        SwitchButton.SetActive(true);
        MatchButton.SetActive(true);
        CloseButton.SetActive(false);
        MatchMessage.text = "牌组就绪!";
        CardsScrollbar.GetComponent<Scrollbar>().value = 1;
        DeckNameBackground.gameObject.SetActive(true);
    }
    public void SwitchDeckClose()
    {
        MatchReset();
        DeckSwitch.GetComponent<Animator>().Play("SwitchDeckClose");
    }
    public void SetDeckList(IList<DeckModel> decks)
    {
        var count = DecksContext.childCount;
        for (var i = count - 1; i >= 0; i--)
        {
            Destroy(DecksContext.GetChild(i).gameObject);
        }
        DecksContext.DetachChildren();
        decks.Select(x => x.Name).ForAll(x => 
        {
            var deck = Instantiate(DeckPrefab);
            deck.GetComponent<SetMatchDeck>().SetDeckInfo(x, DecksContext.childCount);
            deck.transform.SetParent(DecksContext, false);
        });
    }

    public void SetDeck(DeckModel deck,int index)
    {
        CurrentDeckIndex = index;
        var count = CardsContext.childCount;
        for (var i = count - 1; i >= 0; i--)
        {
            Destroy(CardsContext.GetChild(i).gameObject);
        }
        CardsContext.DetachChildren();
        //////////////////////////////////////////////////
        DeckName.text = deck.Name;
        DeckNameBackground.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(25* DeckName.text.Length+ 150, 71);
        DeckName.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(25 * DeckName.text.Length, 40);
        DeckName.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-25 * DeckName.text.Length/2 - 50, 0);
        DeckIcon.GetComponent<Image>().sprite = _groupIconMap[GwentMap.CardMap[deck.Leader].Faction];
        //////////////////////////////////////////////////
        var leader = Instantiate(LaderPrefab);
        leader.GetComponent<SetMatchCard>().SetCardInfo(GwentMap.CardMap[deck.Leader].Strength, GwentMap.CardMap[deck.Leader].Name);
        leader.transform.SetParent(CardsContext,false);
        var cards = deck.Deck.Select(x=> GwentMap.CardMap[x]);
        cards.OrderByDescending(x => x.Group).ThenByDescending(x=>x.Strength).GroupBy(x=>x.Name).ForAll(x =>
        {
            var card = Instantiate(CardPrefab);
            card.GetComponent<SetMatchCard>().SetCardInfo(x.First().Strength, x.Key,x.Count(),x.First().Group);
            card.transform.SetParent(CardsContext, false);
        });
        CC.text = cards.Where(x => x.Group == Group.Copper).Count().ToString();
        SC.text = $"{cards.Where(x => x.Group == Group.Silver).Count().ToString()}/6";
        GC.text = $"{cards.Where(x => x.Group == Group.Gold).Count().ToString()}/4";
        //////////////////////////////////////////////////
        var height = ((41.5f+3f)*CardsContext.childCount)+8f+38f;
        CardsContext.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, height);
    }
}

