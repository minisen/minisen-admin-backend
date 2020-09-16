using CoffeeOOM.Attributes;
using CoffeeSql.Core.Attributes;
using MiniSen_Entity.EntityExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Entity.EntityClass.SystemBaseManage
{
    [Mapper]
    [Table("B_MENU")]
    public class Menu : EntityCommon
    {
        [Column]
        public string Name { get; set; }
        [Column]
        public int SortNumber { get; set; }
        [Column]
        public string ParentId { get; set; }
        [Column("Level0")]
        public int Level { get; set; }
        [Column]
        public int IsLeaf { get; set; }
        [Column]
        public string PagePath { get; set; }
        [Column]
        public int Type { get; set; }
        [Column]
        public string Icon { get; set; }
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
