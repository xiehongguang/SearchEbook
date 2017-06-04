using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEbook.Model
{
    /// <summary>
    /// 生成Kindle书籍信息
    /// </summary>
    class KindleBook
    {
        public string id;
        public string name;
        public string author;
        public ChapterDetial[] chapters;
    }
}
