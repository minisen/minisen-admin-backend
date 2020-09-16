using CoffeeOOM;
using MiniSen_DTO.ShowDTO.SystemBaseManage;
using MiniSen_Entity;
using MiniSen_Entity.EntityClass.SystemBaseManage;
using MiniSen_Service.Service_Interface.SystemBaseManage;
using MiniSen_Service.Service_SQL.SystemBaseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniSen_Service.Service_Class.SystemBaseManage
{
    public class MessageService : IMessageService
    {
        public void ChangeMessageStatus(int status, params string[] messageIds)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                string updateSql = MessageService_SQL.UpdateMessageStatus;
                List<object> _sqlParams = new List<object>();

                _sqlParams.Add(status);

                string messageIdsJoin = messageIds.Length == 0 ? string.Empty : string.Join(",", messageIds);
                _sqlParams.Add(messageIdsJoin);

                dbContext.ExecuteSql(updateSql, _sqlParams.ToArray());
            }
        }

        public IEnumerable<Message_ShowDTO> SearchItemsPaged(int? pageNum = null, int? pageSize = null, int? type = null, int? status = null, string receiver = null, int? theTermDays = null)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Message> commonService = new CommonService<Message>(dbContext);

                var search = commonService.WhereNoMarkDeleted();

                //查询条件
                if (type.HasValue)
                {
                    search = search.Where(d => d.Type == type.Value);
                }

                if (status.HasValue)
                {
                    search = search.Where(d => d.Status == status.Value);
                }

                if (!string.IsNullOrEmpty(receiver))
                {
                    search = search.Where(d => d.Receiver == receiver);
                }

                if (theTermDays.HasValue)
                {
                    DateTime theEarliestDateTime = DateTime.Now.AddDays(-theTermDays.Value);
                    search = search.Where(d => d.CreateTime >= theEarliestDateTime);
                }

                //分页
                if (pageNum.HasValue && pageSize.HasValue)
                {
                    search.OrderBy(t => t.SortNumber).Paging(pageNum.Value, pageSize.Value);
                }

                var items = search.Select().ToList();

                return null == items ? new List<Message_ShowDTO>() : items.Select(d => CoffeeMapper<Message, Message_ShowDTO>.AutoMap(d, (tout, tin) =>
                {
                    switch (tin.Type)
                    {
                        case 1:
                            tout.TypeText = "系統通知";
                            break;
                        case 2:
                            tout.TypeText = "個人消息";
                            break;
                        default:
                            tout.TypeText = "未知消息類型";
                            break;
                    }
                    switch (tin.Status)
                    {
                        case -1:
                            tout.TypeText = "發送失敗";
                            break;
                        case 0:
                            tout.TypeText = "未發送";
                            break;
                        case 1:
                            tout.TypeText = "已發送";
                            break;
                        case 2:
                            tout.TypeText = "未閱讀";
                            break;
                        case 3:
                            tout.TypeText = "已閱讀";
                            break;
                        default:
                            tout.TypeText = "未知消息狀態";
                            break;
                    }
                }));

            }
        }
    }
}
