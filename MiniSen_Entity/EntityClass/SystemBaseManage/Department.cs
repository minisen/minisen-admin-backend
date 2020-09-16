using CoffeeOOM.Attributes;
using CoffeeSql.Core.Attributes;
using MiniSen_Entity.EntityExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Entity.EntityClass.SystemBaseManage
{
    [Mapper]
    [Table("B_DEPARTMENT")]
    public class Department : EntityCommon
    {
        [Column]
        public string Name { get; set; }
        [Column]
        public string ParentId { get; set; }
        [Column]
        public int Grade { get; set; }
        [Column]
        public int IsLeaf { get; set; }
        [Column]
        public int SortNumber { get; set; }
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
