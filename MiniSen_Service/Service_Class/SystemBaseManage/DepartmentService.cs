using CoffeeOOM;
using MiniSen_Common.Exceptions;
using MiniSen_Common.Helpers.Utils;
using MiniSen_DTO.AddEditDTO.SystemBaseManage;
using MiniSen_DTO.ShowDTO.SystemBaseManage;
using MiniSen_Entity;
using MiniSen_Entity.EntityClass.SystemBaseManage;
using MiniSen_Service.Service_Interface.SystemBaseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniSen_Service.Service_Class.SystemBaseManage
{
    public class DepartmentService : IDepartmentService
    {
        public string AddNewOne(Department_AddEditDTO addOne, string creater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Department> commonService = new CommonService<Department>(dbContext);

                bool hasExist = commonService.WhereNoMarkDeleted().Where(d => 0 == d.DelFlag && d.Name.Equals(addOne.Name)).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"A Department item with the same name '{addOne.Name}' already exists");
                }

                Department newOne = CoffeeMapper<Department_AddEditDTO, Department>.AutoMap(addOne, (_out, _in) =>
                {
                    _out.Id = Utils.GetGuidStr();
                    _out.Creater = creater;
                });

                return commonService.Insert(newOne);
            }
        }

        public void EditOne(Department_AddEditDTO editOne, string updater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Department> commonService = new CommonService<Department>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(editOne.Id);
                if (!isExist)
                {
                    throw new PushToUserException("Current Department item is not exist");
                }

                bool hasExist = commonService.WhereNoMarkDeleted().Where(d => 0 == d.DelFlag && d.Name.Equals(editOne.Name) && d.Id != editOne.Id).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"A Department item with the same name '{editOne.Name}' already exists");
                }
                

                Department updateOne = CoffeeMapper<Department_AddEditDTO, Department>.AutoMap(editOne, (_out, _in) =>
                {
                    _out.Updater = updater;
                    _out.UpdateTime = DateTime.Now;
                });

                dbContext.Update<Department>(d => new { d.Name, d.ParentId, d.RemarkInfo, d.SortNumber, d.Updater, d.UpdateTime }, updateOne)
                         .Where(d => d.Id.Equals(updateOne.Id)).Done();
            }
        }

        public int MarkDelete(string id)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Department> commonService = new CommonService<Department>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(id);
                if (!isExist)
                {
                    throw new PushToUserException("Current Department item is not exist");
                }

                bool hasChildren = commonService.GetAllNoMarkDeleted().Where(m => m.ParentId == id).Any();
                if (hasChildren)
                {
                    throw new PushToUserException("Current Department item has children, can not be deleted");
                }

                try
                {
                    dbContext.DBTransaction.Begin();

                    dbContext.Update<AdminUser_Department>(ad => new { ad.DelFlag }, new AdminUser_Department { DelFlag = 1 }).Where(rp => rp.DepartmentId == id).Done();

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

        public IEnumerable<Department_ShowDTO> GetAllItems()
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Department> commonService = new CommonService<Department>(dbContext);

                var items = commonService.GetAllNoMarkDeleted().OrderBy(t => t.SortNumber).ToList();

                return null == items ? new List<Department_ShowDTO>() : items.Select(d => CoffeeMapper<Department, Department_ShowDTO>.AutoMap(d));

            }
        }

    }
}
