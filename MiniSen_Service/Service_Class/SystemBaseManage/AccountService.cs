using CoffeeOOM;
using MiniSen_Common.Exceptions;
using MiniSen_Common.Helpers.Utils;
using MiniSen_DTO.AddEditDTO.SystemBaseManage;
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
    public class AccountService : IAccountService
    {
        public string AddNewOne(Account_AddEditDTO addOne, string creater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<AdminUser> commonAdminUserService = new CommonService<AdminUser>(dbContext);
                CommonService<AdminUserInfo> commonAdminUserInfoService = new CommonService<AdminUserInfo>(dbContext);

                bool hasExist = commonAdminUserService.WhereNoMarkDeleted().Where(d => 0 == d.DelFlag && d.Account.Equals(addOne.Account)).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"A admin user item with the same account '{addOne.Account}' already exists");
                }

                try
                {
                    dbContext.DBTransaction.Begin();

                    AdminUser newAdminUserOne = CoffeeMapper<Account_AddEditDTO, AdminUser>.AutoMap(addOne, (_out, _in) =>
                    {
                        _out.PasswordHash = "ensky123.";  //系統默認密碼
                        _out.Id = Utils.GetGuidStr();
                        _out.Creater = creater;
                    });

                    string accountId = commonAdminUserService.Insert(newAdminUserOne);

                    AdminUserInfo newAdminUserInfoOne = CoffeeMapper<Account_AddEditDTO, AdminUserInfo>.AutoMap(addOne, (_out, _in) =>
                    {
                        _out.Id = Utils.GetGuidStr();
                        _out.Creater = creater;
                        _out.AdminUserId = accountId;
                    });

                    commonAdminUserInfoService.Insert(newAdminUserInfoOne);

                    dbContext.DBTransaction.Commit();

                    return accountId;
                }
                catch (Exception ex)
                {
                    dbContext.DBTransaction.Rollback();

                    throw ex;
                }

            }
        }

        public void EditOne(Account_AddEditDTO editOne, string updater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<AdminUser> commonService = new CommonService<AdminUser>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(editOne.Id);
                if (!isExist)
                {
                    throw new PushToUserException("Current account item is not exist");
                }

                bool hasExist = commonService.WhereNoMarkDeleted().Where(d => 0 == d.DelFlag && d.Account.Equals(editOne.Account) && d.Id != editOne.Id).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"A admin user item with the same account '{editOne.Account}' already exists");
                }

                try
                {
                    dbContext.DBTransaction.Begin();

                    AdminUser updateAdminUserOne = CoffeeMapper<Account_AddEditDTO, AdminUser>.AutoMap(editOne, (_out, _in) =>
                    {
                        _out.Updater = updater;
                        _out.UpdateTime = DateTime.Now;
                    });

                    var matchAdminUserInfo = dbContext.Queryable<AdminUserInfo>().Select().Where(a => a.AdminUserId.Equals(editOne.Id)).ToList();

                    if (matchAdminUserInfo.Count != 1)
                    {
                        throw new Exception($"TABLE 'IDSBG_ECARD.B_ADMIN_USER' record which AdminUserId = '{editOne.Id}' is not only one or not exist");
                    }

                    AdminUserInfo updateAdminUserInfoOne = matchAdminUserInfo[0];
                    updateAdminUserInfoOne.Name = editOne.Name;
                    updateAdminUserInfoOne.RemarkInfo = editOne.Remarks;
                    updateAdminUserInfoOne.Updater = updater;
                    updateAdminUserInfoOne.UpdateTime = DateTime.Now;

                    dbContext.Update<AdminUserInfo>(updateAdminUserInfoOne);
                    dbContext.Update<AdminUser>(a => new { a.Account, a.RemarkInfo, a.Updater, a.UpdateTime }, updateAdminUserOne)
                             .Where(a => a.Id.Equals(editOne.Id)).Done();

                    dbContext.DBTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContext.DBTransaction.Rollback();

                    throw ex;
                }

            }
        }

        public int MarkDelete(string id)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<AdminUser> commonService = new CommonService<AdminUser>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(id);
                if (!isExist)
                {
                    throw new PushToUserException("Current dictionary item is not exist");
                }

                try
                {
                    dbContext.DBTransaction.Begin();

                    dbContext.Update<AdminUserInfo>(a => new { a.DelFlag }, new AdminUserInfo { DelFlag = 1 }).Where(a => a.AdminUserId.Equals(id)).Done();
                    int res = commonService.MarkDeleteById(id);

                    dbContext.DBTransaction.Commit();

                    return res;
                }
                catch (Exception ex)
                {
                    dbContext.DBTransaction.Rollback();

                    throw ex;
                }
            }
        }

        public Account_ShowDTO MatchLogin(string account, string password)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<AdminUser> commonService = new CommonService<AdminUser>(dbContext);

                var matchAccount = commonService.WhereNoMarkDeleted()
                                                .Select(a => new { a.Id })
                                                .Where(a => a.Account.Equals(account) && a.PasswordHash.Equals(password))
                                                .ToList();
                if (null == matchAccount)
                {
                    throw new PushToUserException("the entered account or password is incorrect");
                }
                else if (1 == matchAccount.Count)
                {
                    var loginUser = dbContext.Queryable(AccountService_SQL.GetLoginUserInfo, matchAccount[0].Id).ToOne<AdminUser>();

                    return CoffeeMapper<AdminUser, Account_ShowDTO>.AutoMap(loginUser, (tOut, tIn) =>
                    {
                        tOut.Name = tIn["NAME"].ToString();
                        tOut.Roles = tIn["ROLESNAME"].ToString();
                    });
                }
                else if (matchAccount.Count <= 0)
                {
                    throw new PushToUserException("the entered account or password is incorrect");
                }
                else
                {
                    throw new Exception($"Duplicate Account : [account='{account}', password='{password}']");
                }
            }
        }

        public IEnumerable<Account_ShowDTO> SearchItemsPaged(int? pageNum = null, int? pageSize = null, string name = null, string account = null)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                string searchSql = AccountService_SQL.SearchAllAccountInfo;
                List<object> _sqlParams = new List<object>();

                //查询条件
                if (!string.IsNullOrEmpty(name))
                {
                    searchSql += $" and B.NAME = {{{_sqlParams.Count}}}";
                    _sqlParams.Add(name);
                }

                if (!string.IsNullOrEmpty(account))
                {
                    searchSql += $" and A.ACCOUNT = {{{_sqlParams.Count}}}";
                    _sqlParams.Add(account);
                }

                var query = dbContext.Queryable(searchSql, _sqlParams.ToArray());

                //分页
                if (pageNum.HasValue && pageSize.HasValue)
                {
                    query = query.Paging(pageNum.Value, pageSize.Value);
                }

                var items = query.ToList<AdminUser>();

                return null == items ? new List<Account_ShowDTO>() : items.Select(d => CoffeeMapper<AdminUser, Account_ShowDTO>.AutoMap(d, (tout, tin) =>
                {
                    tout.Name = tin["NAME"].ToString();
                    tout.HasRoleIds = tin["HASROLEIDS"].ToString().Split(',').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    tout.Roles = tin["ROLESNAME"].ToString();
                }));

            }
        }

        public void UpdateRolesForAccount(string accountId, string[] roleIds, string creater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<AdminUser> commonService = new CommonService<AdminUser>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(accountId);
                if (!isExist)
                {
                    throw new PushToUserException("Current account item is not exist");
                }

                try
                {
                    dbContext.DBTransaction.Begin();

                    dbContext.Update<AdminUser_Role>(ar => new { ar.DelFlag }, new AdminUser_Role { DelFlag = 1 })
                             .Where(ar => ar.AdminUserId.Equals(accountId)).Done();

                    foreach (string roleId in roleIds)
                    {
                        isExist = dbContext.Queryable<Role>().Where(r => r.DelFlag == 0 && r.Id == roleId).Any();
                        if (!isExist)
                        {
                            throw new PushToUserException("the role you choosed is not exist");
                        }

                        dbContext.Add(new AdminUser_Role
                        {
                            Id = Utils.GetGuidStr(),
                            AdminUserId = accountId,
                            RoleId = roleId,
                            Creater = creater,
                            Updater = creater
                        });
                    }

                    dbContext.DBTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContext.DBTransaction.Rollback();

                    throw ex;
                }
            }
        }

        public bool JudgeIfAccountHasPerms(string accountId, params string[] apiUrls)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                bool judgeRes = false;

                foreach (string apiUrl in apiUrls)
                {
                    var judgeData = dbContext.Queryable(AccountService_SQL.JudgeIfAccountHasApiPerm, Utils.UrlToHump(apiUrl.Trim('/')), accountId).ToData();

                    if (null == judgeData)
                    {
                        judgeRes = false;
                        break;
                    }
                    else if (1 == Convert.ToInt32(judgeData))
                    {
                        judgeRes = true;
                    }
                    else
                    {
                        judgeRes = false;
                        break;
                    }
                }

                return judgeRes;
            }
        }

        public void ChangePassword(string oldPassword, string newPassword, string repeatPassword, string updater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<AdminUser> commonService = new CommonService<AdminUser>(dbContext);

                var item = commonService.WhereNoMarkDeleted().Where(t => t.Id == updater).Select().ToOne();
                if (null == item)
                {
                    throw new PushToUserException("Current account item is not exist");
                }

                if (newPassword != repeatPassword)
                {
                    throw new PushToUserException("兩次輸入的密碼不一致");
                }

                if (newPassword.Length < 6)
                {
                    throw new PushToUserException("密碼最少6位");
                }

                if (item.PasswordHash != oldPassword)
                {
                    throw new PushToUserException("Old Password is not right");
                }

                var updateItem = item;
                updateItem.PasswordHash = newPassword;
                updateItem.Updater = updater;

                dbContext.Update(updateItem);
            }
        }

    }
}
