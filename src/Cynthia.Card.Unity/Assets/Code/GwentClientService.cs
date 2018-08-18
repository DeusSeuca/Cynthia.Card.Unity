using System;
using System.Threading.Tasks;
using Alsein.Utilities.IO;
using Alsein.Utilities.LifetimeAnnotations;
using Autofac;
using Microsoft.AspNetCore.SignalR.Client;

namespace Cynthia.Card.Client
{
    [Singleton]
    public class GwentClientService
    {
        public HubConnection HubConnection { get; set; }
        public LocalPlayer Player { get; set; }
        public UserInfo User { get; set; }
        private IAsyncDataSender sender;/*待修改*/
        private IAsyncDataReceiver receiver;/*待修改*/
        /*待修改*/
        public Task<bool> MatchResult()
        {
            return receiver.ReceiveAsync<bool>();
        }
        public GwentClientService(HubConnection hubConnection)
        {
            /*待修改*/
            (sender, receiver) = AsyncDataEndPoint.CreateSimplex();
            /*待修改*/
            hubConnection.On<bool>("MatchResult",x=> 
            {
                sender.SendAsync<bool>(x);
            });
            //////////////////////////////
            HubConnection = hubConnection;
            Player = new LocalPlayer(HubConnection);
        }
        public Task<bool> Register(string username, string password, string playername) => HubConnection.InvokeAsync<bool>("Register", username, password, playername);
        public async Task<UserInfo> Login(string username, string password)
        {
            //登录,如果成功保存登录信息
            User = await HubConnection.InvokeAsync<UserInfo>("Login", username, password);
            if (User != null)
                Player.PlayerName = User.PlayerName;
            return User;
        }

        //开始匹配与停止匹配
        public Task<bool> Match(int cardIndex)
        {
            Player.Deck = User.Decks[cardIndex];
            return HubConnection.InvokeAsync<bool>("Match", cardIndex);
        }
        public Task<bool> StopMatch()
        {
            return HubConnection.InvokeAsync<bool>("StopMatch");
        }

        //新建卡组,删除卡组,修改卡组
        public Task<bool> AddDeck(DeckModel deck) => HubConnection.InvokeAsync<bool>("AddDeck", deck);
        public Task<bool> RemoveDeck(int cardIndex) => HubConnection.InvokeAsync<bool>("AddDeck", cardIndex);
        public Task<bool> ModifyDeck(int cardIndex, DeckModel deck) => HubConnection.InvokeAsync<bool>("AddDeck", cardIndex, deck);
        public Task SendOperation(Task<Operation<UserOperationType>> operation) => HubConnection.SendAsync("GameOperation", operation);

        //开启连接,断开连接
        public Task StartAsync() => HubConnection.StartAsync();
        public Task StopAsync() => HubConnection.StopAsync();
    }
}