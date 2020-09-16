using CoffeeOOM.Attributes;
using CoffeeSql.Core.Attributes;
using CoffeeSql.Core.EntityDesign;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Entity.EntityExtensions
{
    [Mapper]
    public class EntityCommon : EntityBase
    {
        [PrimaryKey]
        [Column]
        public string Id { get; set; }
        [Column]
        public string RemarkInfo { get; set; }
        [Column]
        public int DelFlag { get; set; }   //oracle: 不支持bool，使用int；mysql中使用bool；
        [Column]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [Column]
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        [Column]
        public string Creater { get; set; }
        [Column]
        public string Updater { get; set; }

    }
}
