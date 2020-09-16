using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_DTO.ShowDTO.MiniSenBlog
{
    public class Article_ShowDTO : BaseDTO
    {
        public string Category { get; set; }
        public string Title { get; set; }
        public string FullTitle { get; set; }
        public string SubHead { get; set; }
        public string Intro { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string CopyFrom { get; set; }
        public string Inputer { get; set; }
        public string Keyword { get; set; }
        public int HitNum { get; set; }
        public int IsOnTop { get; set; }
        public string IsOnTopText { get; set; }
        public DateTime ReleaseTime { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public int SortNumber { get; set; }
    }
}
