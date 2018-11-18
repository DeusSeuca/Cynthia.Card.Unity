using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Alsein.Utilities.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cynthia.Card.Client
{
    public class LocalPlayer : Player
    {
        public IList<string> idList { get; set; }
        public LocalPlayer(HubConnection hubConnection)
        {
            idList = new List<string>();
            ((Player)this).Receive += async x =>
            {
                x.IsAsyncReceived = false;
                await hubConnection.SendAsync("GameOperation", x.Result);
                //Debug.Log($"+++++++++++++发送指令后:{((Operation<UserOperationType>)x.Result).OperationType}");
            };
            //接收到来自下游的消息,发送到通讯层↑
            hubConnection.On<Operation<ServerOperationType>>("GameOperation",async x =>
            {
                //接收到通讯层消息,发送到下游
                x.Arguments = x.Arguments.Select(item => item.ToType<string>());
                //Debug.Log($"--------------收到指令{x.OperationType},id:{x.Id}");
                var o = Operation.Create(UserOperationType.OK, new object[0]);
                o.Id = x.Id;
                await SendAsync(o);
                if (!idList.Contains(x.Id))
                {
                    idList.Add(x.Id);
                    await SendAsync(x);   
                }
            });
        }
        public Task SendAsync(Operation<UserOperationType> operation) => _downstream.SendAsync(operation);
        public Task SendAsync(UserOperationType type, params object[] objs) => _downstream.SendAsync(Operation.Create(type, objs));
        public new Task<Operation<ServerOperationType>> ReceiveAsync() => _downstream.ReceiveAsync<Operation<ServerOperationType>>();
        public new event Func<ReceiveEventArgs, Task> Receive { add => _downstream.Receive += value; remove => _downstream.Receive -= value; }
    }
}
