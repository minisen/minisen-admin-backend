using CoffeeOOM.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniSen_DTO.AddEditDTO.SystemBaseManage
{
    [Mapper]
    public class Department_AddEditDTO
    {
        public string Id { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "部門名称的字串长度范围为1-100")]
        [Required(ErrorMessage = "部門名称为必须项")]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "父节点Id的字串长度范围为1-50")]
        [Required(ErrorMessage = "没有父节点记录")]
        public string ParentId { get; set; }

        [Required(ErrorMessage = "排序数字为必须项")]
        [Mapper("SortNumber")]
        public int Sort { get; set; }

        [StringLength(100, ErrorMessage = "备注的字串长度最长为100")]
        [Mapper("RemarkInfo")]
        public string Remarks { get; set; }
    }
}
