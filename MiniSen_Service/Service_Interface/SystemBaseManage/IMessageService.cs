using MiniSen_DTO.ShowDTO.SystemBaseManage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Service.Service_Interface.SystemBaseManage
{
    public interface IMessageService : IServiceSupport
    {
        /// <summary>
        /// 獲取搜索結果
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="type">消息類型</param>
        /// <param name="status">消息狀態</param>
        /// <param name="receiver">接收者</param>
        /// <param name="theTermDays">天數期限</param>
        /// <returns></returns>
        IEnumerable<Message_ShowDTO> SearchItemsPaged(int? pageNum = null, int? pageSize = null, int? type = null, int? status = null, string receiver = null, int? theTermDays = null);

        /// <summary>
        /// 改變消息狀態
        /// </summary>
        /// <param name="status">目標狀態</param>
        /// <param name="messageIds">消息的id集合</param>
        void ChangeMessageStatus(int status, params string[] messageIds);
    }
}
