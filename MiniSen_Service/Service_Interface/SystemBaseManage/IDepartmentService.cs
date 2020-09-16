using MiniSen_DTO.AddEditDTO.SystemBaseManage;
using MiniSen_DTO.ShowDTO.SystemBaseManage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Service.Service_Interface.SystemBaseManage
{
    public interface IDepartmentService : IServiceSupport
    {
        /// <summary>
        /// 软删除部門项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int MarkDelete(string id);

        /// <summary>
        /// 新增部門项
        /// </summary>
        /// <returns></returns>
        string AddNewOne(Department_AddEditDTO addOne, string creater);

        /// <summary>
        /// 编辑部門项
        /// </summary>
        /// <param name="editOne"></param>
        void EditOne(Department_AddEditDTO editOne, string updater);

        /// <summary>
        /// 获取所有部門项
        /// </summary>
        /// <returns></returns>
        IEnumerable<Department_ShowDTO> GetAllItems();
    }
}
