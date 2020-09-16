using MiniSen_DTO.AddEditDTO.SystemBaseManage;
using MiniSen_DTO.ShowDTO.SystemBaseManage;
using System;
using System.Collections.Generic;
using System.Text;


namespace MiniSen_Service.Service_Interface.SystemBaseManage
{
    public interface IDictionaryService : IServiceSupport
    {
        /// <summary>
        /// 软删除字典项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int MarkDelete(string id);

        /// <summary>
        /// 新增字典项
        /// </summary>
        /// <returns></returns>
        string AddNewOne(Dictionary_AddEditDTO addOne, string creater);

        /// <summary>
        /// 编辑字典项
        /// </summary>
        /// <param name="editOne"></param>
        void EditOne(Dictionary_AddEditDTO editOne, string updater);

        /// <summary>
        /// 获取搜索结果
        /// </summary>
        /// <param name="pageNum">查询分页数据的页码</param>
        /// <param name="pageSize">每页的数据量</param>
        /// <param name="type">字典项类型（作为查询项）</param>
        /// <returns></returns>
        IEnumerable<Dictionary_ShowDTO> SearchItemsPaged(int? pageNum = null, int? pageSize = null, string type = null);

    }
}
