using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SearchEbook.Model
{
    class Kindlegen : Window
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
            //MessageBox.Show("目前不支持mobi");
            loadTemplates();
            //create tmp
            if (Directory.Exists("./tmp"))
            {
                // 删除制定的目录，并删除目录下的文件、目录
                Directory.Delete("./tmp", true);
            }
            Directory.CreateDirectory("./tmp");
            createCover(kindleBook);
            createChapters(kindleBook);
            createStyle(kindleBook);
            createBookToc(kindleBook);
            createNaxToc(kindleBook);
            createOpf(kindleBook);
            gen(filePath);
            MessageBox.Show("下载完成");
        }
        // 加载模板
        static void loadTemplates()
        {
            if (tplIsLoaded) return;
            tpl_cover = File.ReadAllText("workspace/tpls/tpl_cover.html");
            tpl_book_toc = File.ReadAllText("workspace/tpls/tpl_book_toc.html");
            tpl_chapter = File.ReadAllText("workspace/tpls/tpl_chapter.html");
            tpl_content = File.ReadAllText("workspace/tpls/tpl_content.opf");
            tpl_style = File.ReadAllText("workspace/tpls/tpl_style.css");
            tpl_toc = File.ReadAllText("workspace/tpls/tpl_toc.ncx");
            tplIsLoaded = true;
        }
        /// <summary>
        /// 创建封皮
        /// </summary>
        static void createCover(KindleBook kindleBook)
        {
            string path = "./tmp/cover.html";
            string content = tpl_cover;
            content = content.Replace("___BOOK_NAME___", kindleBook.name);
            content = content.Replace("___BOOK_AUTHOR___", kindleBook.author);
            File.WriteAllText(path, content);
        }
        /// <summary>
        /// 创建章节
        /// </summary>
        static void createChapters(KindleBook kindleBook)
        {
            for (int i = 0; i < kindleBook.chapters.Length; i++)
            {
                var chapter = kindleBook.chapters[i];
                string path = "./tmp/chapter" + i + ".html";
                string content = tpl_chapter;
                content = content.Replace("___CHAPTER_ID___", "Chapter " + i);
                content = content.Replace("___CHAPTER_NAME___", chapter.title);
                string chapterContent = chapter.body;
                chapterContent = chapterContent.Replace("\r", "");
                var ps = chapterContent.Split('\n');
                chapterContent = "";
                foreach (var p in ps)
                {
                    string pStr = "<p class=\"a\">";
                    pStr += "　　" + p;
                    pStr += "</p>";
                    chapterContent += pStr;
                }
                content = content.Replace("___CONTENT___", chapterContent);
                File.WriteAllText(path, content);
            }
        }
        /// <summary>
        /// 创建格式
        /// </summary>
        static void createStyle(KindleBook kindleBook)
        {
            string path = "./tmp/style.css";
            File.WriteAllText(path, tpl_style);
        }
        /// <summary>
        /// 目录内容
        /// </summary>
        static void createBookToc(KindleBook kindleBook)
        {
            string path = "./tmp/book-toc.html";
            string content = tpl_book_toc;
            string tocContent = "";
            for (int i = 0; i < kindleBook.chapters.Length; i++)
            {
                var chapter = kindleBook.chapters[i];
                string tocLine = string.Format("<dt class=\"tocl2\"><a href=\"chapter{0}.html\">{1}</a></dt>\r\n",
                    i, chapter.title);
                tocContent += tocLine;
            }
            content = content.Replace("___CONTENT___", tocContent);
            File.WriteAllText(path, content);
        }
        /// <summary>
        /// 
        /// </summary>
        static void createNaxToc(KindleBook kindleBook)
        {
            string path = "./tmp/toc.ncx";
            string content = tpl_toc;
            content = content.Replace("___BOOK_ID___", kindleBook.id);
            content = content.Replace("___BOOK_NAME___", kindleBook.name);
            string tocContent = "";
            for (int i = 0; i < kindleBook.chapters.Length; i++)
            {
                var chapter = kindleBook.chapters[i];
                string tocLine = string.Format("<navPoint id=\"chapter{0}\" playOrder=\"{1}\">\r\n", i, i + 1);
                tocLine += string.Format("<navLabel><text>{0}</text></navLabel>\r\n", chapter.title);
                tocLine += string.Format("<content src=\"chapter{0}.html\"/>\r\n</navPoint>\r\n", i);
                tocContent += tocLine;
            }
            content = content.Replace("___NAV___", tocContent);
            File.WriteAllText(path, content);
        }
        /// <summary>
        /// 
        /// </summary>
        static void createOpf(KindleBook kindleBook)
        {
            string path = "./tmp/content.opf";
            string content = tpl_content;
            content = content.Replace("___BOOK_ID___", kindleBook.id);
            content = content.Replace("___BOOK_NAME___", kindleBook.name);
            string manifest = "";
            string spine = "";
            for (int i = 0; i < kindleBook.chapters.Length; i++)
            {
                var chapter = kindleBook.chapters[i];
                // mainifest
                string tocLine = "";
                tocLine = string.Format("<item id=\"chapter{0}\" href=\"chapter{0}.html\" media-type=\"application/xhtml+xml\"/>\r\n", i);
                manifest += tocLine;
                // spine
                string spineLine = "";
                spineLine = string.Format("<itemref idref=\"chapter{0}\" linear=\"yes\"/>\r\n", i);
                spine += spineLine;


            }
            content = content.Replace("___MANIFEST___", manifest);
            content = content.Replace("___SPINE___", spine);
            File.WriteAllText(path, content);
        }
        static void gen(string savePath)
        {
            string binPath = Directory.GetCurrentDirectory() + "\\workspace\\tools\\kindlegen.exe";
            string param = "content.opf -c1 -o book.mobi";
            ProcessStartInfo p = null;
            Process Proc;
            p = new ProcessStartInfo(binPath, param);
            p.WorkingDirectory = Directory.GetCurrentDirectory() + "/tmp";
            Proc = Process.Start(p);//调用外部程序
            Proc.WaitForExit();
            if (File.Exists("./tmp/book.mobi"))
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }
                File.Move("./tmp/book.mobi", savePath);
            }
            Directory.Delete("./tmp", true);
        }
    }
}
