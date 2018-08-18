using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cynthia.Card.Client;
using Autofac;

public class SetMatchDeck : MonoBehaviour
{
    public Text DeckText;
    private int _index;
    private GwentClientService _client;
    private MainCodeService _codeService;
    public void SetDeckInfo(string name,int index)
    {
        DeckText.text = name;
        _index = index;
        _client = DependencyResolver.Container.Resolve<GwentClientService>();
        _codeService = DependencyResolver.Container.Resolve<MainCodeService>();
    }
    public void OnClick()
    {
        _codeService.SetDeck(_client.User.Decks[_index],_index);
        _codeService.SwitchDeckClose();
    }
}
