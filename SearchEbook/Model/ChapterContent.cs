using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEbook.Model
{
    /*  Host:chapter2.zhuishushenqi.com
        Method:GET /chapter/章节link(从章节列表中获得)?k=2124b73d7e2e1945&t=1468223717
       Params:
             link:章节link(从章节列表中获得)
             t:当前时间戳
         */
    //http://chapter2.zhuishushenqi.com/chapter/http://book.kanunu.org/files/yqxs/201103/2233/51781.html?k=2124b73d7e2e1945&t=1491895445
    class ChapterContent
    {

        public bool ok { get; set; }
        public Content chapter { get; set; }
    }

    public class Content
    {
        public string title { get; set; }
        public string body { get; set; }
    }
}
