using System.Collections;
using System.Collections.Generic;
using Cynthia.Card;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using Alsein.Utilities;
using Autofac;
using Cynthia.Card.Client;

public class EditorInfo : MonoBehaviour
{
    public InputField Search;
    public RectTransform CardsContent;
    public GameObject UICardPrefab;
    public Scrollbar Scroll;
    public Toggle AllButton;
    public Transform DecksContext;
    public GameObject DeckPrefab;
    private IList<CardStatus> _cards { get => GwentMap.GetCards().ToList(); }
    private Faction _faction = Faction.All;
    private string _searchMessage = "";
    private GwentClientService _clientService;

    void Start()
    {
        Search.onValueChanged.RemoveAllListeners();
        Search.onValueChanged.AddListener(x => SearchChange(x));
        _clientService = DependencyResolver.Container.Resolve<GwentClientService>();
        SetDeckList(_clientService.User.Decks);
        ResetCard();
    }
    public void SetCardInfo(IList<CardStatus> cards)
    {
        var count = cards.Count;
        RemoveAllChild();
        for (var i = 0; i < count; i++)
        {
            var card = Instantiate(UICardPrefab).GetComponent<CardShowInfo>();
            card.CurrentCore = cards[i];
            card.transform.SetParent(CardsContent, false);
        }
        //------------------------------------------------------------------------//276
        var height = (50f + 267f * (count % 6 > 0 ? count / 6 + 1 : count / 6));//count <= 16 ? 780f : 
        CardsContent.sizeDelta = new Vector2(500, height);
        CardsContent.GetComponent<GridLayoutGroup>().padding.top = 104;
        Scroll.value = 1;
    }
    public void AutoSetCards()
    {
        SetCardInfo
        (
            _cards
            .Where(x => ((_faction == Faction.All) ? true : (x.Faction == _faction)))
            .Where(x => ((_searchMessage == "") ? true :
                (x.CardInfo.Name.Contains(_searchMessage) ||
                x.CardInfo.Info.Contains(_searchMessage) ||
                x.CardInfo.Strength.ToString().Contains(_searchMessage))))
            .ToList()
        );
    }
    public void ResetCard()
    {
        Search.text = "";
        AllButton.isOn = true;
    }
    public void GorpClick(int change)
    {
        _faction = (Faction)change;
        AutoSetCards();
        Debug.Log($"点击了{_faction}按钮");
    }
    public void SearchChange(string value)
    {
        _searchMessage = value;
        AutoSetCards();
        Debug.Log($"变成了:{value == ""}");
    }
    public void RemoveAllChild()
    {
        for (var i = CardsContent.childCount - 1; i >= 0; i--)
        {
            Destroy(CardsContent.GetChild(i).gameObject);
        }
        CardsContent.DetachChildren();
    }
    public void SetDeckList(IList<DeckModel> decks)
    {
        var count = DecksContext.childCount;
        for (var i = count - 1; i >= 0; i--)
        {
            Destroy(DecksContext.GetChild(i).gameObject);
        }
        DecksContext.DetachChildren();
        decks.ForAll(x =>
        {
            var deck = Instantiate(DeckPrefab);
            deck.GetComponent<DeckShowInfo>().SetDeckInfo(x.Name, GwentMap.CardMap[x.Leader].Faction);
            deck.GetComponent<SwitchMatchDeck>().SetId(DecksContext.childCount);
            deck.transform.SetParent(DecksContext, false);
        });
    }
}
