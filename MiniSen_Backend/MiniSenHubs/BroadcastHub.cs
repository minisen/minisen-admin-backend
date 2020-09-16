using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MiniSen_Common.Helpers.Log;
using MiniSen_Common.Helpers.Utils;
using MiniSen_MVC_Common.Helper;
using MiniSen_Service.Service_Class.SystemBaseManage;
using MiniSen_Service.Service_Interface.SystemBaseManage;

namespace MiniSen_Backend.MiniSenHubs
{

    public class BroadcastHub : Hub
    {

        //private static bool timerCreated = false;
        //public static Timer timer;

        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}

        //public void CreateTimer()
        //{

        //    if (!timerCreated)
        //    {
        //        timer = new Timer(new TimerCallback(sayHello), this.Clients.All, 0, 1000);
        //        timerCreated = true;
        //    }

        //}

        //public void sayHello(object a)
        //{
        //    IClientProxy all = a as IClientProxy;
        //    all.SendAsync("ReceiveMessage", "system", "timer--hello");
        //}

        private void DebugInfo()
        {
            //LogHelper.debug(string.Join("\r\n", _onlineUserDic.Keys.Select(k => $"Account:{((HubOnlineUser)_onlineUserDic[k]).Account}-ConnectionId:{((HubOnlineUser)_onlineUserDic[k]).ConnectionId}-OnlineLastTime:{((HubOnlineUser)_onlineUserDic[k]).OnlineLastTime.ToString()}")) + "\r\n");
        }

        public static MessageService messageService = new MessageService();
        private static ConcurrentDictionary<string, HubOnlineUser> _onlineUserDic = new ConcurrentDictionary<string, HubOnlineUser>();
        protected static Timer DetectHubOnlineTimer;
        protected static Timer CleanOfflineUserTimer;
        protected static Timer SendSysMessageTimer;
        protected static bool SendSysMessageTimer_HasCreated = false;
        protected static bool DetectHubOnlineTimer_HasCreated = false;
        protected static bool CleanOfflineUserTimer_HasCreated = false;

        public void UpdateCurrentOnlineUser(string currentBroadcastHubConnectionId)
        {
            if (this.Context.ConnectionId != currentBroadcastHubConnectionId)
            {
                this.Context.Abort();
                LogHelper.debug($"abort connectionId:{this.Context.ConnectionId}");
                return;
            }

            HubOnlineUser currentOnineUser = _onlineUserDic.GetOrAdd(this.Context.ConnectionId, new HubOnlineUser());

            //update OnlineUser's Info
            currentOnineUser.ConnectionId = this.Context.ConnectionId;
            currentOnineUser.Account = this.Context.GetHttpContext().GetSessionStr("LoginUserAccount");
            currentOnineUser.OnlineLastTime = DateTime.Now;

            DebugInfo();
        }

        protected void DetectOnlineUser(object a)
        {
            var hubClients = a as IHubCallerClients;

            hubClients.All.SendAsync("HubEcho");

            DebugInfo();

        }

        /// <summary>
        /// remove user which offline over one minute
        /// </summary>
        protected void CleanOfflineUser(object a)
        {
            DateTime nowTime = DateTime.Now;
            var lastOnlineUsers = BroadcastHub._onlineUserDic;
            List<string> removeConnectionIds = new List<string>();
            HubOnlineUser outRemoveUser;

            foreach (var keyValuePair in lastOnlineUsers)
            {
                DateTime lastOnlineTime = keyValuePair.Value.OnlineLastTime;

                if ((nowTime - lastOnlineTime).TotalSeconds > 60)
                {
                    removeConnectionIds.Add(keyValuePair.Key);
                }
            }

            removeConnectionIds.ForEach(connectionId => {
                lastOnlineUsers.Remove(connectionId, out outRemoveUser);
                LogHelper.debug($"remove=>Account:{outRemoveUser.Account}-ConnectionId:{outRemoveUser.ConnectionId}\r\n");
            });

            DebugInfo();

        }

        protected void SendSysMessageToHubUsers(object a)
        {
            var hubClients = (IHubCallerClients)a as IHubCallerClients;

            var lastOnlineUsers = BroadcastHub._onlineUserDic;

            var allNonSendMessages = BroadcastHub.messageService.SearchItemsPaged(null, null, null, 0);

            #region 發送給指定用戶
            foreach (var keyValuePair in lastOnlineUsers)
            {
                //1、發送當前用戶的未發送消息
                string hubConnectionId = keyValuePair.Key;
                var currentNeedSendMessages = allNonSendMessages.Where(m => m.Receiver == keyValuePair.Value.Account && m.ExpectedSendTime <= DateTime.Now);

                if (null == currentNeedSendMessages || 0 == currentNeedSendMessages.Count()) continue;

                var sendMessages = currentNeedSendMessages.Select(m => new { 
                                                                id = m.Id, 
                                                                title = m.Title, 
                                                                content = m.Content, 
                                                                sendTime = DateTime.Now,
                                                                sender = m.Sender,
                                                                senderName = m.SenderName,
                                                                type = m.Type,
                                                                status = 2
                                                            })
                                                          .ToArray();

                string sendMessageJson = Utils.ObjectToJson(sendMessages);

                hubClients.Clients(hubConnectionId).SendAsync("ReceiveMessage", sendMessageJson);

                //2、回寫消息的發送狀態（未閱讀）
                BroadcastHub.messageService.ChangeMessageStatus(2, currentNeedSendMessages.Select(m => m.Id).ToArray());
            }
            #endregion

            //發送給所有在線用戶
            DebugInfo();

        }

        public void CheckTimersStatus()
        {
            //15S 進行一次 系統消息發送
            if (!SendSysMessageTimer_HasCreated)
            {
                SendSysMessageTimer = new Timer(new TimerCallback(SendSysMessageToHubUsers), this.Clients, 0, 15120);
                SendSysMessageTimer_HasCreated = true;
            }

            //30S 進行一次 偵測在線用戶
            if (!DetectHubOnlineTimer_HasCreated)
            {
                DetectHubOnlineTimer = new Timer(new TimerCallback(DetectOnlineUser), this.Clients, 0, 30110);
                DetectHubOnlineTimer_HasCreated = true;
            }

            //90S 進行一次 清除掉線用戶
            if (!CleanOfflineUserTimer_HasCreated)
            {
                CleanOfflineUserTimer = new Timer(new TimerCallback(CleanOfflineUser), null, 0, 90690);
                CleanOfflineUserTimer_HasCreated = true;
            }
        }
    }

}
