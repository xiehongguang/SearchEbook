using Newtonsoft.Json.Linq;
using SearchEbook.Controller;
using SearchEbook.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SearchEbook.page
{
    /// <summary>
    /// BookDetial.xaml 的交互逻辑
    /// </summary>
    public partial class BookDetial : Window
    {
        CommonController common = new CommonController();

        public BookDetial()
        {
            InitializeComponent();
            initData();
        }

        public void initData()
        {
            // 点击自动搜索框，数据的id
            var selectedIndex = Int32.Parse( Application.Current.Properties["SelectedIndex"].ToString());
            var bookList = (Book[])Application.Current.Properties["SearchBookList"];
            bookTitle.Content = bookList[selectedIndex].title;
            bookAuthor.Content = bookList[selectedIndex].author;
            bookCat.Content = bookList[selectedIndex].cat;
            bookWordCount.Content = bookList[selectedIndex].wordCount+ "字";
            bookLatelyFollower.Content = bookList[selectedIndex].latelyFollower+" 人";
            booklatestChapter.Content = bookList[selectedIndex].lastChapter;
            bookRetentionRatio.Content = bookList[selectedIndex].retentionRatio+" %";
            bookShortInfo.Text = bookList[selectedIndex].shortIntro;

            string url = bookList[selectedIndex].cover;
            int urlStartIndex = url.IndexOf("http:");
            if (urlStartIndex >= 0)
            {
                url = url.Substring(urlStartIndex);
                bookImage.Source = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
                // picturebox_cover.ImageLocation = url;
                //MessageBox.Show(bookList[bookId].cover);
            }


            // 获取书源
            url = "http://api.zhuishushenqi.com/toc?view=summary&book=" + bookList[selectedIndex]._id + "";
            string json = common.GetPage(url);
            //jsonArry
            var bookSource =  JArray.Parse(json);
            Dictionary<string,string> sourceList = new Dictionary<string, string>();
            List<Source> sourceDetialList = new List<Source>();
            foreach( var sourceItem in bookSource)
            {
                var info = sourceItem.ToObject<Source>();
                sourceDetialList.Add(info);
                // 书源的名字 书源内书的id
                sourceList.Add(info.name, info._id);
            }
            // 章节列表
            url = "http://api.zhuishushenqi.com/toc/"+ sourceList.First().Value+ "?view=chapters";
            json = common.GetPage(url);
            var chapter = new ChapterList();
            chapter = (ChapterList)common.FromJson("ChapterList", json);

            ObservableCollection<ChapterList> list = new ObservableCollection<ChapterList>();
            
            foreach (var item in chapter.chapters)
            {
                chapter = new ChapterList();
                chapter.name = item.title;
                chapter.link = item.link;
                list.Add(chapter);
            }
            dataGrid.ItemsSource = list;

        }
      
    }
}
