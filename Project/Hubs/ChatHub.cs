using System.Net.WebSockets;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Project.Models;

namespace Project.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DbuniPayContext _context;

        public ChatHub ( DbuniPayContext context)
        {
            _context = context;
        }


        /// <summary>
        /// 連線事件
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            string roomName = Context.GetHttpContext().Request.Query["room"]; // 獲取聊天室名稱
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName); // 將用戶加入聊天室
            await Clients.Group(roomName).SendAsync("UpdContent", "新連線 ID: " + Context.ConnectionId + " 進入聊天室: " + roomName);
            await base.OnConnectedAsync();
        }
        //public override async Task OnConnectedAsync()
        //{
        //    await Clients.All.SendAsync("UpdContent", "新連線 ID: " + Context.ConnectionId);
        //    await base.OnConnectedAsync();
        //}

        /// <summary>
        /// 離線事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("UpdContent", "已離線 ID: " + Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public async Task SendMessage(string user, string message)
        {
            //for SQL
            var chatMessage = new Tmessage
            {
                ChatId = 1, //測試用先寫死
                MessageSendId = int.Parse(user),
                MessageContent = message,
                MessageTime = DateTime.Now
            };

            _context.Tmessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            //for 前端顯示
            var msg = new
            {
                User = user,
                Message = message,
                Timestamp = DateTime.Now.ToString("HH:mm")
            };
            await Clients.All.SendAsync("UpdContent", msg);
        }
    }
}
