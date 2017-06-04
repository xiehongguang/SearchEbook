using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEbook.Model
{
    class Kindlegen
    {
        /// <summary>
        /// 封面
        /// </summary>
        private static string tpl_cover;
        /// <summary>
        /// 书源
        /// </summary>
        private static string tpl_book_toc;
        /// <summary>
        /// 章节
        /// </summary>
        private static string tpl_chapter;
        /// <summary>
        /// 章节内容
        /// </summary>
        private static string tpl_content;
        /// <summary>
        /// 章节样式
        /// </summary>
        private static string tpl_style;
        private static string tpl_toc;
        private static bool tplIsLoaded = false;
        KindleBook kindleBook = new KindleBook();
        /// <summary>
        /// 转化成Txt格式
        /// </summary>
        /// <param name="kindleBook">书籍信息</param>
        /// <param name="filePath">路径</param>
        public static void book2Txt(KindleBook kindleBook,string filePath)
        {
            string txt = "";
            for (int i = 0; i < kindleBook.chapters.Length; i++)
            {
                var chapterInfo = kindleBook.chapters[i];
                txt += chapterInfo.title + "\r\n";
                txt += chapterInfo.body + "\r\n";
            }

            string fileDir = Path.GetDirectoryName(filePath);
            if(!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }
            File.WriteAllText(filePath,txt);
        }
        public static void book2Mobi(KindleBook kindleBook, string filePath)
        {
            loadTemplates();
        }
        static void loadTemplates()
        {
            if (tplIsLoaded) return;
            tpl_cover = File.ReadAllText("/SearchEbook/workspace/tpls/tpl_cover.html");
            tpl_book_toc = File.ReadAllText("/SearchEbook/workspace/tpls/tpl_book_toc.html");
            tpl_chapter = File.ReadAllText("/SearchEbook/workspace/tpls/tpl_chapter.html");
            tpl_content = File.ReadAllText("/SearchEbook/workspace/tpls/tpl_content.opf");
            tpl_style = File.ReadAllText("/SearchEbook/workspace/tpls/tpl_style.css");
            tpl_toc = File.ReadAllText("/SearchEbook/workspace/tpls/tpl_toc.ncx");
            tplIsLoaded = true;
        }

    }
}
