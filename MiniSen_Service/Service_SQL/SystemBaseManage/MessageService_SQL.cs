using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Service.Service_SQL.SystemBaseManage
{
    /// <summary>
    /// 存放MessageService中的所有sql
    /// </summary>
    public static class MessageService_SQL
    {
        public static string UpdateMessageStatus = @"

            update 
	            B_MESSAGE 
            set 
	            STATUS = {0},
                UpdateTime = NOW(),
	            Updater = 'system'
            where 
	            Find_IN_SET(ID, {1})
        ";
    }
}
