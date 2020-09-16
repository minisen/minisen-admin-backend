using System;
using System.Collections.Generic;
using System.Text;
using CoffeeOOM.Attributes;
using CoffeeSql.Core.Attributes;
using MiniSen_Entity.EntityExtensions;

namespace MiniSen_Entity.EntityClass.SystemBaseManage
{
    [Mapper]
    [Table("B_ADMIN_USER_INFO")]
    public class AdminUserInfo : EntityCommon
    {
        [Column]
        public string Name { get; set; }
        [Column]
        public string AdminUserId { get; set; }
        [Column]
        public string BackupField_1 { get; set; }
        [Column]
        public string BackupField_2 { get; set; }
        [Column]
        public string BackupField_3 { get; set; }
        [Column]
        public string BackupField_4 { get; set; }
        [Column]
        public string BackupField_5 { get; set; }
    }
}
