using CoffeeSql.Core.Attributes;
using MiniSen_Entity.EntityExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Entity.EntityClass.SystemBaseManage
{
    [Table("R_ADMIN_USER_ROLE")]
    public class AdminUser_Role : EntityCommon
    {
        [Column]
        public string AdminUserId { get; set; }
        [Column]
        public string RoleId { get; set; }
    }
}
