using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace CoreMVC_SignalR_Chat.Hubs
{
    public class ChatHub : Hub
    {
        // 用戶連線 ID 列表
        public static List<string> ConnIDList = new List<string>();

        /// <summary>
        /// 連線事件
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {

            if (ConnIDList.Where(p => p == Context.ConnectionId).FirstOrDefault() == null)
            {
                ConnIDList.Add(Context.ConnectionId);
            }
            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(ConnIDList);
            await Clients.All.SendAsync("UpdList", jsonString);

            // 更新個人 ID
            await Clients.Client(Context.ConnectionId).SendAsync("UpdSelfID", Context.ConnectionId);

            // 更新聊天內容
            await Clients.All.SendAsync("UpdContent", "新連線 ID: " + Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 離線事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            string id = ConnIDList.Where(p => p == Context.ConnectionId).FirstOrDefault();
            if (id != null)
            {
                ConnIDList.Remove(id);
            }
            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(ConnIDList);
            await Clients.All.SendAsync("UpdList", jsonString);

            // 更新聊天內容
            await Clients.All.SendAsync("UpdContent", "已離線 ID: " + Context.ConnectionId);

            await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SendMessage(string selfID, string message, string sendToID)
        {
            if (string.IsNullOrEmpty(sendToID))
            {
                await Clients.All.SendAsync("UpdContent", selfID + " 說: " + message);
            }
            else
            {
                // 接收人
                await Clients.Client(sendToID).SendAsync("UpdContent", selfID + " 私訊向你說: " + message);

                // 發送人
                await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "你向 " + sendToID + " 私訊說: " + message);
            }
        }
    }
}
//using Microsoft.AspNetCore.SignalR;
//using Project.Models;
//using System.Threading.Tasks;

//public class ChatHub : Hub
//{
//    private readonly DbuniPayContext _context;

//    public ChatHub(DbuniPayContext context)
//    {
//        _context = context;
//    }

//    public async Task SendMessage(string selfID, string message, string sendToID)
//    {
//        // 將訊息保存到資料庫
//        var newMessage = new Tcservice
//        {
//            Csid = int.Parse(selfID),
//            Mid = int.Parse(sendToID),
//            Cstexts = message,
//            Csmtimes = DateTime.Now,
//            Csgmtimes = DateTime.Now,
//            Csstatus = "Active"
//        };

//        _context.Tcservices.Add(newMessage);
//        await _context.SaveChangesAsync();

//        // 發送訊息給指定的用戶
//        await Clients.User(sendToID).SendAsync("ReceiveMessage", selfID, message);
//    }

//    public override async Task OnConnectedAsync()
//    {
//        var selfID = Context.ConnectionId;
//        await Clients.Caller.SendAsync("UpdSelfID", selfID);
//        await base.OnConnectedAsync();
//    }
//}