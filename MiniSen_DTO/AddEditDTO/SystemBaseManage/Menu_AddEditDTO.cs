using CoffeeOOM.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniSen_DTO.AddEditDTO.SystemBaseManage
{
    [Mapper]
    public class Menu_AddEditDTO
    {
        public string Id { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "菜单项名称的字串长度范围为1-50")]
        [Required(ErrorMessage = "菜单项名称为必须项")]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "页面路径的字串最长为100")]
        public string PagePath { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "父节点Id的字串长度范围为1-50")]
        [Required(ErrorMessage = "没有父节点记录")]
        public string ParentId { get; set; }

        [StringLength(50, ErrorMessage = "图标名称的字串长度最长为50")]
        public string Icon { get; set; }

        [Required(ErrorMessage = "菜单项类型为必须项")]
        public int Type { get; set; }

        [Required(ErrorMessage = "排序数字为必须项")]
        [Mapper("SortNumber")]
        public int Sort { get; set; }

        [StringLength(100, ErrorMessage = "备注的字串长度最长为100")]
        [Mapper("RemarkInfo")]
        public string Remarks { get; set; }
    }
}
