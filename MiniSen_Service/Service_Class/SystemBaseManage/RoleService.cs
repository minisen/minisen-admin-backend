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
    public class RoleService : IRoleService
    {
        public string AddNewOne(Role_AddEditDTO addOne, string creater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Role> commonService = new CommonService<Role>(dbContext);

                bool hasExist = commonService.WhereNoMarkDeleted().Where(d => 0 == d.DelFlag && d.Name.Equals(addOne.Name)).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"A dictionary item with the same name '{addOne.Name}' already exists");
                }

                Role newOne = CoffeeMapper<Role_AddEditDTO, Role>.AutoMap(addOne, (_out, _in) =>
                {
                    _out.Id = Utils.GetGuidStr();
                    _out.Creater = creater;
                });

                return commonService.Insert(newOne);
            }
        }

        public void EditApiPermForRole(string roleId, string[] hasApiPermissionIds, string updater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Role> commonService = new CommonService<Role>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(roleId);
                if (!isExist)
                {
                    throw new PushToUserException("Current role item is not exist");
                }

                try
                {
                    dbContext.DBTransaction.Begin();

                    dbContext.ExecuteSql(RoleService_SQL.MarkDeleteAllApiPermsOfRole, roleId);

                    foreach (string apiPermissionId in hasApiPermissionIds)
                    {
                        dbContext.Add(new Role_Permission
                        {
                            Id = Utils.GetGuidStr(),
                            RoleId = roleId,
                            PermissionId = apiPermissionId,
                            Creater = updater,
                            Updater = updater
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

        public void EditMenuForRole(string roleId, string[] hasMenuIds, string updater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Role> commonService = new CommonService<Role>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(roleId);
                if (!isExist)
                {
                    throw new PushToUserException("Current role item is not exist");
                }

                try
                {
                    dbContext.DBTransaction.Begin();

                    dbContext.ExecuteSql(RoleService_SQL.MarkDeleteAllMenuPermsOfRole, roleId);

                    foreach (string menuId in hasMenuIds)
                    {
                        var permItem = dbContext.Queryable<Permission>().Select(p => new { p.Id }).Where(p => p.DelFlag == 0 && p.MenuId == menuId).ToOne();

                        if (null == permItem)
                        {
                            throw new PushToUserException("menu item is not exist");
                        }

                        dbContext.Add(new Role_Permission
                        {
                            Id = Utils.GetGuidStr(),
                            RoleId = roleId,
                            PermissionId = permItem.Id,
                            Creater = updater,
                            Updater = updater
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

        public void EditOne(Role_AddEditDTO editOne, string updater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Role> commonService = new CommonService<Role>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(editOne.Id);
                if (!isExist)
                {
                    throw new PushToUserException("Current dictionary item is not exist");
                }

                bool hasExist = commonService.WhereNoMarkDeleted().Where(d => 0 == d.DelFlag && d.Id != editOne.Id && d.Name.Equals(editOne.Name)).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"A dictionary item with the same name '{editOne.Name}' already exists");
                }

                Role updateOne = CoffeeMapper<Role_AddEditDTO, Role>.AutoMap(editOne, (_out, _in) =>
                {
                    _out.Updater = updater;
                    _out.UpdateTime = DateTime.Now;
                });

                dbContext.Update<Role>(d => new { d.Name, d.SortNumber, d.RemarkInfo, d.Updater, d.UpdateTime }, updateOne)
                         .Where(d => d.Id.Equals(updateOne.Id)).Done();
            }
        }

        public int MarkDelete(string id)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Role> commonService = new CommonService<Role>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(id);
                if (!isExist)
                {
                    throw new PushToUserException("Current dictionary item is not exist");
                }

                try
                {
                    dbContext.DBTransaction.Begin();

                    dbContext.Update<Role_Permission>(rp => new { rp.DelFlag }, new Role_Permission { DelFlag = 1 }).Where(rp => rp.RoleId.Equals(id)).Done();
                    dbContext.Update<AdminUser_Role>(ar => new { ar.DelFlag }, new AdminUser_Role { DelFlag = 1 }).Where(ar => ar.RoleId.Equals(id)).Done();
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

        public IEnumerable<Role_ShowDTO> SearchItemsPaged(int? pageNum = null, int? pageSize = null, string name = null)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                string searchSql = RoleService_SQL.SearchAllRoleInfo;

                //查询条件
                if (!string.IsNullOrEmpty(name))
                {
                    searchSql += " and A.NAME = {0}";
                }

                var query = dbContext.Queryable(searchSql, name);

                //分页
                if (pageNum.HasValue && pageSize.HasValue)
                {
                    query = query.Paging(pageNum.Value, pageSize.Value);
                }

                var items = query.ToList<Role>();

                return null == items ? new List<Role_ShowDTO>() : items.Select(d => CoffeeMapper<Role, Role_ShowDTO>.AutoMap(d, (tout, tin) =>
                {
                    tout.HasApiPermissionIds = tin["HAS_API_PERMIDS"].ToString().Split(',').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    tout.HasMenuIds = tin["HAS_MENUIDS"].ToString().Split(',').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                }));

            }
        }
    }
}
