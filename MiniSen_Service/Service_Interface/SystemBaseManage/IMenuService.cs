using MiniSen_DTO.AddEditDTO.SystemBaseManage;
using MiniSen_DTO.ShowDTO.SystemBaseManage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Service.Service_Interface.SystemBaseManage
{
    public interface IMenuService : IServiceSupport
    {
        /// <summary>
        /// 软删除菜单项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int MarkDelete(string id);

        /// <summary>
        /// 新增菜单项
        /// </summary>
        /// <returns></returns>
        string AddNewOne(Menu_AddEditDTO addOne, string creater);

        /// <summary>
        /// 编辑菜单项
        /// </summary>
        /// <param name="editOne"></param>
        void EditOne(Menu_AddEditDTO editOne, string updater);

        /// <summary>
        /// 获取所有菜单项
        /// </summary>
        /// <returns></returns>
        IEnumerable<Menu_ShowDTO> GetAllItems();

        /// <summary>
        /// 获得账号的所有导航菜单项
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IEnumerable<Menu_ShowDTO> GetAccountHasNavMenus(string accountId);
    }
}
