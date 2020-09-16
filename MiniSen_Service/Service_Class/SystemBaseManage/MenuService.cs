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
    public class MenuService : IMenuService
    {
        public string AddNewOne(Menu_AddEditDTO addOne, string creater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Menu> commonService = new CommonService<Menu>(dbContext);

                bool hasExist = commonService.WhereNoMarkDeleted().Where(d => 0 == d.DelFlag && d.Name.Equals(addOne.Name)).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"A menu item with the same name '{addOne.Name}' already exists");
                }

                if (!string.IsNullOrEmpty(addOne.PagePath))
                {
                    hasExist = commonService.WhereNoMarkDeleted().Where(d => d.PagePath.Equals(addOne.PagePath)).Any();
                    if (hasExist)
                    {
                        throw new PushToUserException($"A menu item with the same page path '{addOne.PagePath}' already exists");
                    }
                }

                try
                {
                    dbContext.DBTransaction.Begin();

                    Menu newOne = CoffeeMapper<Menu_AddEditDTO, Menu>.AutoMap(addOne, (_out, _in) =>
                    {
                        _out.Id = Utils.GetGuidStr();
                        _out.Creater = creater;
                    });

                    string newMenuId = commonService.Insert(newOne);

                    //菜单项添加对应的权限项记录
                    if (2 == addOne.Type)
                    {
                        Permission menuPerm = new Permission
                        {
                            Id = Utils.GetGuidStr(),
                            Name = $"menu-({addOne.Name})",
                            PermissionType = 2,
                            MenuId = newMenuId,
                            DelFlag = 0,
                            Creater = creater
                        };

                        dbContext.Add(menuPerm);
                    }

                    dbContext.DBTransaction.Commit();

                    return newMenuId;
                }
                catch (Exception ex)
                {
                    dbContext.DBTransaction.Rollback();

                    throw ex;
                }
            }
        }

        public void EditOne(Menu_AddEditDTO editOne, string updater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Menu> commonService = new CommonService<Menu>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(editOne.Id);
                if (!isExist)
                {
                    throw new PushToUserException("Current menu item is not exist");
                }

                bool hasExist = commonService.WhereNoMarkDeleted().Where(d => 0 == d.DelFlag && d.Type != editOne.Type && d.Id == editOne.Id).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"the type of Menu item is not allow to change");
                }

                hasExist = commonService.WhereNoMarkDeleted().Where(d => 0 == d.DelFlag && d.Name.Equals(editOne.Name) && d.Id != editOne.Id).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"A menu item with the same name '{editOne.Name}' already exists");
                }

                if (!string.IsNullOrEmpty(editOne.PagePath))
                {
                    hasExist = commonService.WhereNoMarkDeleted().Where(d => d.PagePath.Equals(editOne.PagePath) && d.Id != editOne.Id).Any();
                    if (hasExist)
                    {
                        throw new PushToUserException($"A menu item with the same page path '{editOne.PagePath}' already exists");
                    }
                }

                Menu updateOne = CoffeeMapper<Menu_AddEditDTO, Menu>.AutoMap(editOne, (_out, _in) =>
                {
                    _out.Updater = updater;
                    _out.UpdateTime = DateTime.Now;
                });

                dbContext.Update<Menu>(d => new { d.Name, d.PagePath, d.ParentId, d.Icon, d.Type, d.RemarkInfo, d.SortNumber, d.Updater, d.UpdateTime }, updateOne)
                         .Where(d => d.Id.Equals(updateOne.Id)).Done();
            }
        }

        public int MarkDelete(string id)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Menu> commonService = new CommonService<Menu>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(id);
                if (!isExist)
                {
                    throw new PushToUserException("Current menu item is not exist");
                }

                bool hasChildren = commonService.GetAllNoMarkDeleted().Where(m => m.ParentId == id).Any();
                if (hasChildren)
                {
                    throw new PushToUserException("Current menu item has children, can not be deleted");
                }

                try
                {
                    dbContext.DBTransaction.Begin();

                    var perm = dbContext.Queryable<Permission>().Select().Where(p => p.MenuId == id).ToOne();

                    if (null != perm)
                    {
                        dbContext.Update<Permission>(rp => new { rp.DelFlag }, new Permission { DelFlag = 1 }).Where(p => p.MenuId.Equals(id)).Done();
                        dbContext.Update<Role_Permission>(rp => new { rp.DelFlag }, new Role_Permission { DelFlag = 1 }).Where(rp => rp.PermissionId == perm.Id).Done();
                    }

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

        public IEnumerable<Menu_ShowDTO> GetAllItems()
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Menu> commonService = new CommonService<Menu>(dbContext);

                var items = commonService.GetAllNoMarkDeleted().OrderBy(t => t.SortNumber).ToList();

                return null == items ? new List<Menu_ShowDTO>() : items.Select(d => CoffeeMapper<Menu, Menu_ShowDTO>.AutoMap(d, (tout, tin) =>
                {
                    tout.IsMenuItem = (2 == tin.Type);
                }));

            }
        }

        public IEnumerable<Menu_ShowDTO> GetAccountHasNavMenus(string accountId)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                var items = dbContext.Queryable(MenuService_SQL.SearchAccountHasNavMenus, accountId).ToList<Menu>();

                return null == items ? new List<Menu_ShowDTO>() : items.Select(d => CoffeeMapper<Menu, Menu_ShowDTO>.AutoMap(d, (tout, tin) =>
                {
                    tout.IsMenuItem = (2 == tin.Type);
                })).OrderBy(m => m.SortNumber).ToList();
            }
        }
    }
}
