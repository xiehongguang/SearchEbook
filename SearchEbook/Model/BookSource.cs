using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEbook.Model
{
    /*
    Host:api.zhuishushenqi.com
    Method:GET /toc?view=summary&book=书籍ID HTTP/1.1
    Example: http://api.zhuishushenqi.com/toc?view=summary&book=54914a84338fe64c65fb0030
    */

    public class BookSource
    {
        public Source[] source { get; set; }
    }

    public class Source
    {
        public string _id { get; set; }
        public string lastChapter { get; set; }
        public string link { get; set; }
        public string source { get; set; }
        public string name { get; set; }
        public bool isCharge { get; set; }
        public int chaptersCount { get; set; }
        public DateTime updated { get; set; }
        public bool starting { get; set; }
        public string host { get; set; }
    }

}
