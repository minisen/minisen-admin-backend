using System;
using System.Collections.Generic;
using System.Text;
using CoffeeOOM.Attributes;
using CoffeeSql.Core.Attributes;
using MiniSen_Entity.EntityExtensions;

namespace MiniSen_Entity.EntityClass.SystemBaseManage
{
    [Mapper]
    [Table("B_DICTIONARY")]
    public class Dictionary : EntityCommon
    {
        [Column]
        public string Name { get; set; }
        [Column]
        public string Value { get; set; }
        [Column]
        public string Type { get; set; }
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
