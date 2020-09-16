using MiniSen_DTO.AddEditDTO.SystemBaseManage;
using MiniSen_DTO.ShowDTO.SystemBaseManage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Service.Service_Interface.SystemBaseManage
{
    public interface IPermissionService : IServiceSupport
    {
        /// <summary>
        /// 软删除Api权限项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int MarkDelete(string id);

        /// <summary>
        /// 新增Api权限项
        /// </summary>
        /// <returns></returns>
        string AddNewOne(Permission_AddEditDTO addOne, string creater);

        /// <summary>
        /// 编辑Api权限项
        /// </summary>
        /// <param name="editOne"></param>
        void EditOne(Permission_AddEditDTO editOne, string updater);


        /// <summary>
        /// 获取搜索结果
        /// </summary>
        /// <param name="pageNum">查询分页数据的页码</param>
        /// <param name="pageSize">每页的数据量</param>
        /// <param name="type">Api权限项类型（作为查询项）</param>
        /// <param name="nameKey">名称（作为查询项<模糊匹配>）</param>
        /// <returns></returns>
        IEnumerable<Permission_ShowDTO> SearchItemsPaged(int? pageNum = null, int? pageSize = null, string apiType = null, string nameKey = null);

        /// <summary>
        /// 获得账号所拥有的Api权限项
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IEnumerable<Permission_ShowDTO> GetAccountHasApiPerms(string accountId);
    }
}
