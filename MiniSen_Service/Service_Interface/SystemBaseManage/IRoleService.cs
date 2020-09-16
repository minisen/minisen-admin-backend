using MiniSen_DTO.AddEditDTO.SystemBaseManage;
using MiniSen_DTO.ShowDTO.SystemBaseManage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Service.Service_Interface.SystemBaseManage
{
    public interface IRoleService : IServiceSupport
    {
        /// <summary>
        /// 软删除角色项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int MarkDelete(string id);

        /// <summary>
        /// 新增角色项
        /// </summary>
        /// <returns></returns>
        string AddNewOne(Role_AddEditDTO addOne, string creater);

        /// <summary>
        /// 编辑角色项
        /// </summary>
        /// <param name="editOne"></param>
        void EditOne(Role_AddEditDTO editOne, string updater);


        /// <summary>
        /// 获取搜索结果
        /// </summary>
        /// <param name="pageNum">查询分页数据的页码</param>
        /// <param name="pageSize">每页的数据量</param>
        /// <param name="name">角色名（作为查询项）</param>
        /// <returns></returns>
        IEnumerable<Role_ShowDTO> SearchItemsPaged(int? pageNum = null, int? pageSize = null, string name = null);


        /// <summary>
        /// 为角色编辑Api权限信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="hasApiPermissionIds"></param>
        /// <param name="updater"></param>
        void EditApiPermForRole(string roleId, string[] hasApiPermissionIds, string updater);


        /// <summary>
        /// 为角色编辑菜单信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="hasMenuIds"></param>
        /// <param name="updater"></param>
        void EditMenuForRole(string roleId, string[] hasMenuIds, string updater);
    }
}
