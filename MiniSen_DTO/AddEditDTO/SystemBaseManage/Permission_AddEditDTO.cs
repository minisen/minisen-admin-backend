using CoffeeOOM.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniSen_DTO.AddEditDTO.SystemBaseManage
{
    [Mapper]
    public class Permission_AddEditDTO
    {
        public string Id { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "名称的字串长度范围为1-50")]
        [Required(ErrorMessage = "名称为必须项")]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "Api类型的字串长度范围为1-50")]
        [Required(ErrorMessage = "Api类型为必须项")]
        public string ApiType { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = "访问路径的字串长度范围为1-200")]
        [Required(ErrorMessage = "访问路径为必须项")]
        public string ApiUrl { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "访问方式的字串长度范围为1-20")]
        [Required(ErrorMessage = "访问方式为必须项")]
        public string ApiMethod { get; set; }

        [StringLength(100, ErrorMessage = "备注项字串最长为100")]
        [Mapper("RemarkInfo")]
        public string Remarks { get; set; }

        [Mapper("SortNumber")]
        public int Sort { get; set; }
    }
}
