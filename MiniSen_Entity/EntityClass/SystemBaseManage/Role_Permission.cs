using System;
using System.Collections.Generic;
using System.Text;
using CoffeeOOM.Attributes;
using CoffeeSql.Core.Attributes;
using MiniSen_Entity.EntityExtensions;

namespace MiniSen_Entity.EntityClass.SystemBaseManage
{
    [Table("R_ROLE_PERMISSION")]
    public class Role_Permission : EntityCommon
    {
        [Column]
        public string RoleId { get; set; }
        [Column]
        public string PermissionId { get; set; }
    }
}
