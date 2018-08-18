using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alsein.Utilities;
using UnityEngine;
using Alsein.Utilities.LifetimeAnnotations;
using System.Threading;

namespace Cynthia.Card.Client
{
    [Transient]
    public class GwentClientGameService
    {
        private LocalPlayer _player;
        //--------------------------------
        //手牌
        private IList<GameCard> _myHand;
        private IList<GameCard> _enemyHand;
        //墓地
        private IList<GameCard> _myCemetery = new List<GameCard>();
        private IList<GameCard> _enemyCemetery = new List<GameCard>();
        private IList<GameCard>[] _myPlace = new IList<GameCard>[3];
        private IList<GameCard>[] _enemyPlace = new IList<GameCard>[3];
        //--------------------------------
        public GameCodeService GameCodeService { get; set; }
        //--------------------------------
        private string _myName;
        private string _enemyName;

        public async Task Play(LocalPlayer player)
        {
            _player = player;
            while(ResponseOperation(await _player.ReceiveAsync()));
        }

        //-----------------------------------------------------------------------
        //响应指令
        private bool ResponseOperation(Operation<ServerOperationType> operation)
        {
            switch (operation.OperationType)
            {
                case ServerOperationType.GameStart:
                    var gameInformation = operation.Arguments.ToArray()[0].ToType<GameInfomation>();
                    _myName = _player.PlayerName;
                    _enemyName = gameInformation.EnemyName;
                    GameCodeService.SetGameInfo(gameInformation);
                    break;
                case ServerOperationType.GameInfomation:
                    break;
                case ServerOperationType.GameEnd:
                    GameCodeService.ShowGameResult(operation.Arguments.ToArray()[0].ToType<GameResultInfomation>());
                    return false;
            }
            return true;
        }
    }
}