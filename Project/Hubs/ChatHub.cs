using System;
using System.Net.WebSockets;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                string roomId = Context.GetHttpContext().Request.Query["room"]; // 獲取聊天室名稱
                int memberId = int.Parse(roomId); // room 參數是會員 ID

                // 創建或更新聊天室記錄
                var chatRoom = new Tchat
                {
                    ChatConnectId = Context.ConnectionId,
                    Mid = memberId,
                    ChatCreateTime = DateTime.Now
                };

                _context.Tchats.Add(chatRoom);
                await _context.SaveChangesAsync();

                // 將用戶加入聊天室群組
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

                // 取得會員名稱
                var member = await _context.Tmembers.FindAsync(memberId);
                string memberName = member?.Mname ?? "會員";

                var msg = new
                {
                    User = "System",
                    Message = $"{memberName} 已進入聊天室",
                    Timestamp = DateTime.Now.ToString("HH:mm"),
                    SystemMessage = true
                };

                // 發送連線訊息給群組
                await Clients.Group(roomId).SendAsync("UpdContent", msg);

                // 載入歷史訊息
                var historicalMessages = await LoadHistoricalMessages(chatRoom.ChatId);
                foreach (var message in historicalMessages)
                {
                    var historicalMsg = new
                    {
                        User = message.MessageSendId.ToString(),
                        Message = message.MessageContent,
                        Timestamp = message.MessageTime.ToString("HH:mm")
                    };
                    await Clients.Caller.SendAsync("UpdContent", historicalMsg);
                }

                await base.OnConnectedAsync();
            }
            catch (Exception ex) {
                // 記錄錯誤
                System.Diagnostics.Debug.WriteLine($"連線錯誤: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 離線事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            try
            {
                // 找到當前的聊天室記錄
                var chatRoom = await _context.Tchats
                    .FirstOrDefaultAsync(c => c.ChatConnectId == Context.ConnectionId);

                if (chatRoom != null)
                {
                    string roomId = chatRoom.Mid.ToString();

                    // 從群組中移除用戶
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

                    var member = await _context.Tmembers.FindAsync(chatRoom.Mid);
                    string memberName = member?.Mname ?? "會員";

                    var msg = new
                    {
                        User = "System",
                        Message = $"{memberName} 已離開聊天室",
                        Timestamp = DateTime.Now.ToString("HH:mm"),
                        SystemMessage = true
                    };

                    await Clients.Group(roomId).SendAsync("UpdContent", msg);
                }

                await base.OnDisconnectedAsync(ex);
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"離線錯誤: {error.Message}");
                throw;
            }
        }

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public async Task SendMessage(string user, string message)
        {
            try
            {
                // 找到當前用戶的聊天室
                var chatRoom = await _context.Tchats
                    .FirstOrDefaultAsync(c => c.ChatConnectId == Context.ConnectionId);

                if (chatRoom != null)
                {
                    // 儲存訊息到SQL資料庫
                    var chatMessage = new Tmessage
                    {
                        ChatId = 1,//chatRoom.ChatId, //測試用先寫死
                        MessageSendId = int.Parse(user),
                        MessageContent = message,
                        MessageTime = DateTime.Now
                    };

                    _context.Tmessages.Add(chatMessage);
                    await _context.SaveChangesAsync();

                    // 取得會員名稱
                    var member = await _context.Tmembers.FindAsync(int.Parse(user));
                    string memberName = member?.Mname ?? "未知會員";

                    // 發送訊息給View、Razor顯示
                    var msg = new
                    {
                        User = user,
                        Message = message,
                        Timestamp = DateTime.Now.ToString("HH:mm"),
                        //ChatID = chatRoom.ChatId
                    };
                    // 發送訊息給聊天室群組
                    //string roomId = chatRoom.Mid.ToString();
                    //await Clients.Group(roomId).SendAsync("UpdContent", msg);
                    await Clients.All.SendAsync("UpdContent", msg);
                }
                else
                {
                    throw new Exception("找不到聊天室記錄");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"發送訊息錯誤: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 載入歷史訊息
        /// </summary>
        private async Task<List<Tmessage>> LoadHistoricalMessages(int chatId)
        {
            return await _context.Tmessages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.MessageTime)
                .Take(100) // 限制載入最近的 100 條訊息
                .ToListAsync();
        }
    }
}
