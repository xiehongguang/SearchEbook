using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEbook.Model
{
    class SearchBook
    {

        /*
         Host:api.zhuishushenqi.com
         Method:GET /book/fuzzy-search
         Params:
             query:关键词
             start:结果开始位置    匹配字
             limit:结果最大数量    匹配字
         example:  http://api.zhuishushenqi.com/book/fuzzy-search?query="双阙"&start=1&limit=5

        */
        public Book[] books { get; set; }
        public bool ok { get; set; }
    }
    public class Book
    {
        public string _id { get; set; }
        public bool hasCp { get; set; }
        public string title { get; set; }
        public string cat { get; set; }
        public string author { get; set; }
        public string site { get; set; }
        public string cover { get; set; }
        public string shortIntro { get; set; }
        public string lastChapter { get; set; }
        public float? retentionRatio { get; set; }
        public int latelyFollower { get; set; }
        public int wordCount { get; set; }
    }
}
