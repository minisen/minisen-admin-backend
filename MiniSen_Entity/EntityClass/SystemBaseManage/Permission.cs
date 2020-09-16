using CoffeeOOM.Attributes;
using CoffeeSql.Core.Attributes;
using MiniSen_Entity.EntityExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Entity.EntityClass.SystemBaseManage
{
    [Mapper]
    [Table("B_PERMISSION")]
    public class Permission : EntityCommon
    {
        [Column]
        public string Name { get; set; }
        [Column]
        public int PermissionType { get; set; }
        [Column]
        public string MenuId { get; set; }
        [Column]
        public int SortNumber { get; set; }
        [Column]
        public string ApiUrl { get; set; }
        [Column]
        public string ApiType { get; set; }
        [Column]
        public string ApiMethod { get; set; }
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
