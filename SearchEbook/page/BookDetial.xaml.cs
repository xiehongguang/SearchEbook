using SearchEbook.Controller;
using SearchEbook.Model;
using System;
using System.Collections.Generic;
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
            var bookId=Int32.Parse( Application.Current.Properties["SelectedIndex"].ToString());
            var bookList = (Book[])Application.Current.Properties["SearchBookList"];

            //string url = @" http://api.zhuishushenqi.com/book/" + bookId;
            //MessageBox.Show(url);
            //string json = common.GetPage(url);
            //var bookDetialInfo = new BookDetialInfo();
            //bookDetialInfo = (BookDetialInfo)common.FromJson("BookDetialInfo", json);
            bookTitle.Content = bookList[bookId].title;
            bookAuthor.Content = bookList[bookId].author;
            bookCat.Content = bookList[bookId].cat;
            bookWordCount.Content = bookList[bookId].wordCount+ "字";
            bookLatelyFollower.Content = bookList[bookId].latelyFollower+" 人";
            booklatestChapter.Content = bookList[bookId].lastChapter;
            bookRetentionRatio.Content = bookList[bookId].retentionRatio+" %";
            bookShortInfo.Text = bookList[bookId].shortIntro;
            bookImage.Source = new BitmapImage(new Uri(bookList[bookId].cover, UriKind.Relative));
            MessageBox.Show(bookList[bookId].shortIntro);
        }
      
    }
}
