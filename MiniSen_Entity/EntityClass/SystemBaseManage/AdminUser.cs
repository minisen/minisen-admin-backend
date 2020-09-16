using CoffeeOOM.Attributes;
using CoffeeSql.Core.Attributes;
using MiniSen_Entity.EntityExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Entity.EntityClass.SystemBaseManage
{
    [Mapper]
    [Table("B_ADMIN_USER")]
    public class AdminUser : EntityCommon
    {
        [Column]
        public string Account { get; set; }
        [Column]
        public string PasswordSalt { get; set; }
        [Column]
        public string PasswordHash { get; set; }
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
