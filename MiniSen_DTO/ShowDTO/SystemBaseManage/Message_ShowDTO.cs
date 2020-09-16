using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_DTO.ShowDTO.SystemBaseManage
{
    public class Message_ShowDTO : BaseDTO
    {
        public int SortNumber { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public string TypeText { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public DateTime ExpectedSendTime { get; set; }
        public DateTime LastSendTime { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
    }
}
