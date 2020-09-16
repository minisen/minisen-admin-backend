using CoffeeOOM.Attributes;
using CoffeeSql.Core.Attributes;
using MiniSen_Entity.EntityExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Entity.EntityClass.SystemBaseManage
{
    [Mapper]
    [Table("B_MESSAGE")]
    public class Message : EntityCommon
    {
        [Column]
        public int SortNumber { get; set; }
        [Column]
        public string Sender { get; set; }
        [Column]
        public string Receiver { get; set; }
        [Column]
        public string Title { get; set; }
        [Column]
        public string Content { get; set; }
        [Column]
        public int Type { get; set; }
        [Column]
        public int Status { get; set; }
        [Column]
        public DateTime ExpectedSendTime { get; set; } = DateTime.Now;
        [Column]
        public string SenderName { get; set; }
        [Column]
        public string ReceiverName { get; set; }
    }
}
