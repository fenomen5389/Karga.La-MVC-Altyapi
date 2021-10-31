using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.Hubs
{
    public class ChatHub:Hub
    {
        IChatMessageService _chatMessageService;
        public ChatHub(IChatMessageService chatMessageService)
        {
            _chatMessageService = chatMessageService;
        }
        public async Task SendMessage(string userName,string message)
        {
            _chatMessageService.Add(new ChatMessage() { Message = message, Username = userName, DateTime = DateTime.Now });
            await Clients.Others.SendAsync("ReceiveMessage",userName,message);
        }
    }
}
