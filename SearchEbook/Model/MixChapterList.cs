using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEbook.Model
{
    class MixChapterList
    {
            public Mixtoc mixToc { get; set; }
            public bool ok { get; set; }
    }
    public class Mixtoc
    {
        public string _id { get; set; }
        public string book { get; set; }
        public DateTime chaptersUpdated { get; set; }
        public MixChapter[] chapters { get; set; }
        public DateTime updated { get; set; }
    }

    public class MixChapter
    {
        public string title { get; set; }
        public string link { get; set; }
        public bool unreadble { get; set; }
    }
}
