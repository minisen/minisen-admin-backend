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
    public class DictionaryService : IDictionaryService
    {
        public string AddNewOne(Dictionary_AddEditDTO addOne, string creater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Dictionary> commonService = new CommonService<Dictionary>(dbContext);

                bool hasExist = commonService.WhereNoMarkDeleted().Where(d => d.Name.Equals(addOne.Name)).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"A dictionary item with the same name '{addOne.Name}' already exists");
                }

                Dictionary newOne = CoffeeMapper<Dictionary_AddEditDTO, Dictionary>.AutoMap(addOne, (_out, _in) =>
                {
                    _out.Id = Utils.GetGuidStr();
                    _out.Creater = creater;
                });

                return commonService.Insert(newOne);
            }
        }

        public void EditOne(Dictionary_AddEditDTO editOne, string updater)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Dictionary> commonService = new CommonService<Dictionary>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(editOne.Id);
                if (!isExist)
                {
                    throw new PushToUserException("Current dictionary item is not exist");
                }

                bool hasExist = commonService.WhereNoMarkDeleted().Where(d => d.Name.Equals(editOne.Name) && d.Id != editOne.Id).Any();
                if (hasExist)
                {
                    throw new PushToUserException($"A dictionary item with the same name '{editOne.Name}' already exists");
                }

                Dictionary updateOne = CoffeeMapper<Dictionary_AddEditDTO, Dictionary>.AutoMap(editOne, (_out, _in) =>
                {
                    _out.Updater = updater;
                    _out.UpdateTime = DateTime.Now;
                });

                dbContext.Update<Dictionary>(d => new { d.Name, d.Value, d.Type, d.SortNumber, d.RemarkInfo, d.Updater, d.UpdateTime }, updateOne)
                         .Where(d => d.Id.Equals(updateOne.Id)).Done();
            }
        }

        public int MarkDelete(string id)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Dictionary> commonService = new CommonService<Dictionary>(dbContext);

                bool isExist = commonService.AnyByIdNoMarkDeleted(id);
                if (!isExist)
                {
                    throw new PushToUserException("Current dictionary item is not exist");
                }

                return commonService.MarkDeleteById(id);
            }
        }

        public IEnumerable<Dictionary_ShowDTO> SearchItemsPaged(int? pageNum = null, int? pageSize = null, string type = null)
        {
            using (MiniSenDbContext dbContext = new MiniSenDbContext())
            {
                CommonService<Dictionary> commonService = new CommonService<Dictionary>(dbContext);

                var search = commonService.WhereNoMarkDeleted();

                //查询条件
                if (!string.IsNullOrEmpty(type))
                {
                    search = search.Where(d => d.Type.Equals(type));
                }

                //分页
                if (pageNum.HasValue && pageSize.HasValue)
                {
                    search.OrderBy(t => t.SortNumber).Paging(pageNum.Value, pageSize.Value);
                }

                var items = search.Select().ToList();

                return null == items ? new List<Dictionary_ShowDTO>() : items.Select(d => CoffeeMapper<Dictionary, Dictionary_ShowDTO>.AutoMap(d)).ToList();

            }
        }

    }
}
