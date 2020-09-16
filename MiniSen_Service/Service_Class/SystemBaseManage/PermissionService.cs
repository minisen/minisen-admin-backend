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
    public class PermissionService : IPermissionService
    {
        public string AddNewOne(Permission_AddEditDTO addOne, string creater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Permission> commonService = new CommonService<Permission>(dbContext);

                bool hasExist = commonService.WhereNoMarkDeleted().Where(p => 0 == p.DelFlag && p.ApiUrl.Equals(addOne.ApiUrl)).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"A api permission item with the same api url '{addOne.ApiUrl}' already exists");
                }

                Permission newOne = CoffeeMapper<Permission_AddEditDTO, Permission>.AutoMap(addOne, (_out, _in) =>
                {
                    _out.Id = Utils.GetGuidStr();
                    _out.Creater = creater;
                    _out.PermissionType = 1;
                });

                return commonService.Insert(newOne);
            }
        }

        public void EditOne(Permission_AddEditDTO editOne, string updater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Permission> commonService = new CommonService<Permission>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(editOne.Id);
                if (!isExist)
                {
                    throw new PushToUserException("Current api permission item is not exist");
                }

                bool hasExist = commonService.WhereNoMarkDeleted().Where(p => 0 == p.DelFlag && p.Id != editOne.Id && p.ApiUrl.Equals(editOne.ApiUrl)).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"A api permission item with the same api url '{editOne.ApiUrl}' already exists");
                }

                Permission updateOne = CoffeeMapper<Permission_AddEditDTO, Permission>.AutoMap(editOne, (_out, _in) =>
                {
                    _out.Updater = updater;
                    _out.UpdateTime = DateTime.Now;
                });

                dbContext.Update<Permission>(p => new { p.Name, p.ApiType, p.ApiUrl, p.ApiMethod, p.RemarkInfo, p.SortNumber, p.Updater, p.UpdateTime }, updateOne)
                         .Where(d => d.Id.Equals(updateOne.Id)).Done();
            }
        }

        public int MarkDelete(string id)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Permission> commonService = new CommonService<Permission>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(id);
                if (!isExist)
                {
                    throw new PushToUserException("Current api permission item is not exist");
                }

                try
                {
                    dbContext.DBTransaction.Begin();

                    dbContext.Update<Role_Permission>(rp => new { rp.DelFlag }, new Role_Permission { DelFlag = 1 }).Where(rp => rp.PermissionId.Equals(id)).Done();
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

        public IEnumerable<Permission_ShowDTO> SearchItemsPaged(int? pageNum = null, int? pageSize = null, string apiType = null, string nameKey = null)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Permission> commonService = new CommonService<Permission>(dbContext);

                var search = commonService.WhereNoMarkDeleted().Where(p => p.PermissionType == 1);

                //查询条件
                if (!string.IsNullOrEmpty(apiType))
                {
                    search = search.Where(p => p.ApiType.Equals(apiType));
                }

                if (!string.IsNullOrEmpty(nameKey))
                {
                    search = search.Where(p => p.Name.Contains(nameKey));
                }

                //分页
                if (pageNum.HasValue && pageSize.HasValue)
                {
                    search.OrderBy(t => t.SortNumber).Paging(pageNum.Value, pageSize.Value);
                }

                var items = search.Select().ToList();

                return null == items ? new List<Permission_ShowDTO>() : items.Select(d => CoffeeMapper<Permission, Permission_ShowDTO>.AutoMap(d)).ToList();

            }
        }

        public IEnumerable<Permission_ShowDTO> GetAccountHasApiPerms(string accountId)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                var items = dbContext.Queryable(PermissionService_SQL.SearchAccountHasApiPerms, accountId).ToList<Permission>();

                return null == items ? new List<Permission_ShowDTO>() : items.Select(d => CoffeeMapper<Permission, Permission_ShowDTO>.AutoMap(d));
            }
        }
    }
}
