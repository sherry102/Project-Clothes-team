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
        private static readonly Dictionary<string, string> _userRooms = new Dictionary<string, string>();
        private static readonly Dictionary<string, HashSet<string>> _roomConnections = new Dictionary<string, HashSet<string>>();


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

        /// <summary>
        /// 加入特定聊天室
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        public async Task JoinRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                await Clients.Caller.SendAsync("Error", "聊天室ID不能為空");
                return;
            }

            // 先離開當前聊天室
            string currentRoomId = _userRooms.ContainsKey(Context.ConnectionId) ? _userRooms[Context.ConnectionId] : null;
            if (!string.IsNullOrEmpty(currentRoomId))
            {
                await LeaveRoom(currentRoomId);
            }

            // 加入新聊天室
            if (!_roomConnections.ContainsKey(roomId))
            {
                _roomConnections[roomId] = new HashSet<string>();
            }

            _roomConnections[roomId].Add(Context.ConnectionId);
            _userRooms[Context.ConnectionId] = roomId;

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            // 通知聊天室有新成員加入
            var msg = new
            {
                User = "System",
                Message = $"用戶 ID: {Context.ConnectionId} 加入聊天室",
                Timestamp = DateTime.Now.ToString("HH:mm"),
                SystemMessage = true
            };

            await Clients.Group(roomId).SendAsync("UpdContent", msg);

            // 更新聊天室成員數量
            var memberCount = _roomConnections[roomId].Count;
            await Clients.Group(roomId).SendAsync("UpdateMemberCount", memberCount);

            // 讀取聊天室歷史消息
            var history = await GetChatHistory(roomId);
            await Clients.Caller.SendAsync("ChatHistory", history);
        }

        /// <summary>
        /// 離開特定聊天室
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        public async Task LeaveRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                await Clients.Caller.SendAsync("Error", "聊天室ID不能為空");
                return;
            }

            // 檢查用戶是否在該聊天室中
            if (!_roomConnections.ContainsKey(roomId) || !_roomConnections[roomId].Contains(Context.ConnectionId))
            {
                await Clients.Caller.SendAsync("Error", "您不在該聊天室中");
                return;
            }

            // 移除用戶與聊天室的關聯
            _roomConnections[roomId].Remove(Context.ConnectionId);

            // 如果聊天室為空，移除聊天室
            if (_roomConnections[roomId].Count == 0)
            {
                _roomConnections.Remove(roomId);
            }
            else
            {
                // 更新聊天室成員數量
                var memberCount = _roomConnections[roomId].Count;
                await Clients.Group(roomId).SendAsync("UpdateMemberCount", memberCount);
            }

            // 從用戶的聊天室記錄中移除
            if (_userRooms.ContainsKey(Context.ConnectionId) && _userRooms[Context.ConnectionId] == roomId)
            {
                _userRooms.Remove(Context.ConnectionId);
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

            // 通知聊天室有成員離開
            var msg = new
            {
                User = "System",
                Message = $"用戶 ID: {Context.ConnectionId} 離開聊天室",
                Timestamp = DateTime.Now.ToString("HH:mm"),
                SystemMessage = true
            };

            await Clients.Group(roomId).SendAsync("UpdContent", msg);
        }

        /// <summary>
        /// 獲取聊天室成員列表
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        public async Task GetRoomMembers(string roomId)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                await Clients.Caller.SendAsync("Error", "聊天室ID不能為空");
                return;
            }

            if (!_roomConnections.ContainsKey(roomId))
            {
                await Clients.Caller.SendAsync("RoomMembers", roomId, new List<string>());
                return;
            }

            var members = _roomConnections[roomId].ToList();
            await Clients.Caller.SendAsync("RoomMembers", roomId, members);
        }

        /// <summary>
        /// 獲取聊天室歷史消息
        /// </summary>
        /// <param name="roomId">聊天室ID</param>
        private async Task<List<object>> GetChatHistory(string roomId)
        {
            // 從資料庫讀取聊天歷史
            var chatId = 1; // 這裡要根據 roomId 取得正確的 chatId，目前先寫死

            var history = await _context.Tmessages
                .Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.MessageTime)
                .Take(50) // 取最近的50條消息
                .OrderBy(m => m.MessageTime) // 按時間順序排序
                .Select(m => new
                {
                    MessageId = m.MessageId,
                    User = m.MessageSendId.ToString(), // 這裡可能需要從用戶表獲取用戶名稱
                    Message = m.MessageContent,
                    Timestamp = m.MessageTime.ToString("HH:mm")
                })
                .ToListAsync();

            return history.Cast<object>().ToList();
        }

        /// <summary>
        /// 刪除訊息
        /// </summary>
        /// <param name="messageId">消息ID</param>
        public async Task DeleteMessage(int messageId)
        {
            // 檢查消息是否存在
            var message = await _context.Tmessages.FindAsync(messageId);
            if (message == null)
            {
                await Clients.Caller.SendAsync("Error", "消息不存在");
                return;
            }

            // 檢查是否為消息發送者
            if (message.MessageSendId.ToString() != Context.GetHttpContext().Request.Query["user"])
            {
                await Clients.Caller.SendAsync("Error", "您沒有權限刪除此消息");
                return;
            }

            // 從資料庫刪除消息
            _context.Tmessages.Remove(message);
            await _context.SaveChangesAsync();

            // 通知聊天室成員刪除該消息
            string roomId = Context.GetHttpContext().Request.Query["room"];
            await Clients.Group(roomId).SendAsync("MessageDeleted", messageId);
        }

        /// <summary>
        /// 發送文件訊息
        /// </summary>
        /// <param name="user">用戶ID</param>
        /// <param name="fileName">文件名稱</param>
        /// <param name="fileUrl">文件URL</param>
        public async Task SendFileMessage(string user, string fileName, string fileUrl)
        {
            string roomId = Context.GetHttpContext().Request.Query["room"];

            // 確保用戶已經加入聊天室
            if (string.IsNullOrEmpty(roomId) || !_roomConnections.ContainsKey(roomId) || !_roomConnections[roomId].Contains(Context.ConnectionId))
            {
                await Clients.Caller.SendAsync("Error", "您尚未加入聊天室或聊天室不存在");
                return;
            }

            // 檢查文件數據是否為空
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileUrl))
            {
                await Clients.Caller.SendAsync("Error", "文件數據不完整");
                return;
            }

            // 構建文件消息內容 (JSON格式存儲)
            var fileData = JsonConvert.SerializeObject(new { FileName = fileName, FileUrl = fileUrl });

            // 存入資料庫
            var chatMessage = new Tmessage
            {
                ChatId = 1, // 測試用先寫死
                MessageSendId = int.Parse(user),
                MessageContent = fileData,
                MessageTime = DateTime.Now
            };

            _context.Tmessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            // 發送給聊天室成員
            var msg = new
            {
                User = user,
                Message = fileData,
                Timestamp = DateTime.Now.ToString("HH:mm"),
                MessageId = chatMessage.MessageId,
                IsFile = true,
                FileName = fileName,
                FileUrl = fileUrl
            };

            await Clients.Group(roomId).SendAsync("UpdContent", msg);
        }
    }
}