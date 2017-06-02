using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEbook.Model
{
    //Host:api.zhuishushenqi.com
    //Method:GET /toc/书源ID?view=chapters
    //http://api.zhuishushenqi.com/toc/54914a84338fe64c65fb0035?view=chapters
    public class ChapterList
    {
        public string _id { get; set; }
        public string link { get; set; }
        public string name { get; set; }
        public Chapter[] chapters { get; set; }
        public DateTime updated { get; set; }
        public string host { get; set; }
    }

    public class Chapter
    {
        public string title { get; set; }
        public string link { get; set; }
        public int currency { get; set; }
        public bool unreadble { get; set; }
        public bool isVip { get; set; }
    }
}
