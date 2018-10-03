using System;
using System.Threading.Tasks;
using Alsein.Utilities.LifetimeAnnotations;
using Autofac;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using UnityEngine.Audio;
using Alsein.Utilities;
using System.Collections.Generic;

namespace Cynthia.Card.Client
{
    [Transient]
    public class MainCodeService
    {
        private GameObject _code;
        public MainCodeService()
        {
            _code = GameObject.Find("Code");
        }

        public void SetDeckList(IList<DeckModel> decks)
        {
            _code.GetComponent<MainCode>().MatchUI.GetComponent<MatchInfo>().SetDeckList(decks);
        }

        public void SetDeck(DeckModel deck,int index)
        {
            _code.GetComponent<MainCode>().MatchUI.GetComponent<MatchInfo>().SetDeck(deck,index);
        }
        public void SwitchDeckOpen()
        {
            _code.GetComponent<MainCode>().MatchUI.GetComponent<MatchInfo>().SwitchDeckOpen();
        }
        public void MatchReset()
        {
            _code.GetComponent<MainCode>().MatchUI.GetComponent<MatchInfo>().MatchReset();
        }
        public void SwitchDeckClose()
        {
            _code.GetComponent<MainCode>().MatchUI.GetComponent<MatchInfo>().SwitchDeckClose();
        }
    }
}
