using System.Net.WebSockets;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Project.Models;

namespace Project.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DbuniPayContext _context;

        public ChatHub(DbuniPayContext context)
        {
            _context = context;
        }


        /// <summary>
        /// 連線事件
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            string roomId = Context.GetHttpContext().Request.Query["room"]; // 獲取聊天室名稱
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId); // 將用戶加入聊天室
            var msg = new
            {
                User = "System",
                Message = $"新連線 ID: {Context.ConnectionId} 進入聊天室 {roomId}",
                Timestamp = DateTime.Now.ToString("HH:mm"),
                SystemMessage = true
            };
            await Clients.Group(roomId).SendAsync("UpdContent", msg);
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 離線事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var msg = new
            {
                User = "System",
                Message = $"已離線 ID: {Context.ConnectionId}",
                Timestamp = DateTime.Now.ToString("HH:mm"),
                SystemMessage = true
            };
            await Clients.All.SendAsync("UpdContent", msg);
            await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public async Task SendMessage(string user, string message)
        {
            string roomId = Context.GetHttpContext().Request.Query["room"];
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