using MiniSen_DTO.AddEditDTO.SystemBaseManage;
using MiniSen_DTO.ShowDTO.SystemBaseManage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Service.Service_Interface.SystemBaseManage
{
    public interface IAccountService : IServiceSupport
    {
        /// <summary>
        /// 匹配登录信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns>返回匹配成功的用户信息</returns>
        Account_ShowDTO MatchLogin(string account, string password);

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="repeatPassword"></param>
        void ChangePassword(string oldPassword, string newPassword, string repeatPassword, string updater);

        /// <summary>
        /// 软删除用户项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int MarkDelete(string id);

        /// <summary>
        /// 新增字典项
        /// </summary>
        /// <returns></returns>
        string AddNewOne(Account_AddEditDTO addOne, string creater);

        /// <summary>
        /// 编辑字典项
        /// </summary>
        /// <param name="editOne"></param>
        void EditOne(Account_AddEditDTO editOne, string updater);

        /// <summary>
        /// 为账户更新角色
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="roleIds"></param>
        void UpdateRolesForAccount(string accountId, string[] roleIds, string creater);

        /// <summary>
        /// 获取搜索结果
        /// </summary>
        /// <param name="pageNum">查询分页数据的页码</param>
        /// <param name="pageSize">每页的数据量</param>
        /// <param name="name">管理员姓名（作为查询项）</param>
        /// <param name="account">用户账号（作为查询项）</param>
        /// <returns></returns>
        IEnumerable<Account_ShowDTO> SearchItemsPaged(int? pageNum = null, int? pageSize = null, string name = null, string account = null);

        /// <summary>
        /// 判断账号是否有权限
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="apiUrls"></param>
        /// <returns></returns>
        bool JudgeIfAccountHasPerms(string accountId, params string[] apiUrls);

    }
}
