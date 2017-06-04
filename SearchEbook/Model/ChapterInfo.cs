using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEbook.Model
{
    class ChapterInfo
    {
        public bool ok { get; set; }
        public ChapterDetial chapter { get; set; }
    }
    public class ChapterDetial
    {
        public string title { get; set; }
        public string body { get; set; }
        public bool isVip { get; set; }
        public string cpContent { get; set; }
        public int currency { get; set; }
        public string id { get; set; }
    }
}
