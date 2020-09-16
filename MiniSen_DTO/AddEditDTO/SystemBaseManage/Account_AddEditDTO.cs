using CoffeeOOM.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniSen_DTO.AddEditDTO.SystemBaseManage
{
    [Mapper]
    public class Account_AddEditDTO
    {
        public string Id { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "姓名的字串长度范围为1-50")]
        [Required(ErrorMessage = "姓名为必须项")]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "账号的字串长度范围为1-100")]
        [Required(ErrorMessage = "账号为必须项")]
        public string Account { get; set; }

        [StringLength(100, ErrorMessage = "备注项字串最长为100")]
        [Mapper("RemarkInfo")]
        public string Remarks { get; set; }

        public string[] HasRoleIds { get; set; }
    }
}
