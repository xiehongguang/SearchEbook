using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEbook.Model
{

    /*
     Host:api.zhuishushenqi.com
     Method:GET /book/书籍ID
     Example: http://api.zhuishushenqi.com/book/54914a84338fe64c65fb0030
    */
    class BookDetialInfo
    {
        public string _id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string longIntro { get; set; }
        public string cover { get; set; }
        public string cat { get; set; }
        public string majorCate { get; set; }
        public string minorCate { get; set; }
        public bool _le { get; set; }
        public bool allowMonthly { get; set; }
        public bool allowVoucher { get; set; }
        public bool allowBeanVoucher { get; set; }
        public bool hasCp { get; set; }
        public int postCount { get; set; }
        public int latelyFollower { get; set; }
        public int followerCount { get; set; }
        public int wordCount { get; set; }
        public int serializeWordCount { get; set; }
        public int retentionRatio { get; set; }
        public DateTime updated { get; set; }
        public bool isSerial { get; set; }
        public int chaptersCount { get; set; }
        public string lastChapter { get; set; }
        public string[] gender { get; set; }
        public object[] tags { get; set; }
        public bool donate { get; set; }
    }
}
