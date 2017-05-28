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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SearchEbook
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, string> bookTitleAndId = new Dictionary<string, string>();
        CommonController common = new CommonController();
        public MainWindow()
        {
            InitializeComponent();
            bookName.TextChanged += BookName_TextChanged;
            bookListBox.SelectionChanged += BookListBox_SelectionChanged;
        }

        private void BookListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string name = bookListBox.SelectedItem.ToString();
                string bookid;
                string id = "0";
                if (bookTitleAndId.TryGetValue(name, out bookid))
                {
                    id = bookid;
                    // 点击自动搜索框，数据的id
                    Application.Current.Properties["SecectAutoSearchId"] =id;
                }

                string url = @" http://api.zhuishushenqi.com/book/" + id;
                MessageBox.Show(url);
                string json = common.GetPage(url);
                var bookDetialInfo = new BookDetialInfo();
                bookDetialInfo = (BookDetialInfo)common.FromJson("BookDetialInfo", json);

                MessageBox.Show(bookDetialInfo.longIntro);
            }catch(Exception ex)
            {
                ex.ToString();
            }
           

        }

        private void BookName_TextChanged(object sender, TextChangedEventArgs e)
        {
            bookListBox.Items.Clear();
            bookListBox.Visibility = Visibility.Visible;
            bookTitleAndId.Clear();
            string name = bookName.Text.ToString().Trim();
            string url = " http://api.zhuishushenqi.com/book/fuzzy-search?query='" + name + "'&start=0&limit=5";
            string json = common.GetPage(url);
            var book = new SearchBook();
            book = (SearchBook)common.FromJson("SearchBook", json);
            List<string> bookList = new List<string>();
            if (book.books == null)
            {
                bookListBox.Items.Add("没有此图书");
                return;
            }
            else
            {
                foreach (var item in book.books)
                {
                    bookTitleAndId.Add(item.title, item._id);
                    bookList.Add(item.title);
                    bookListBox.Items.Add(item.title);
                }
            }
            if (bookList.Count() != 0)
            {
                bookListBox.Visibility = Visibility.Visible;
                bookListBox.Items.Add("没有此图书");
                //bookListBox.Items.Add( bookList);
            }
        }
        /// <summary>
        /// 搜索图书
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBook_Click(object sender, RoutedEventArgs e)
        {
            bookListBox.Items.Clear();
            bookListBox.Visibility = Visibility.Visible;
            string name = bookName.Text.ToString().Trim();
            string url = " http://api.zhuishushenqi.com/book/fuzzy-search?query='" + name + "'&start=0&limit=5";
            string json = common.GetPage(url);
            var book = new SearchBook();
            book =(SearchBook) common.FromJson("SearchBook", json);
            if( book.books == null)
            {
                bookListBox.Items.Add("没有此图书");
                return;
            }
            List<string> bookList = new List<string>();
            foreach (var item in book.books)
            {
                bookList.Add(item.title);
                bookListBox.Items.Add(item.title);
            }
            if (bookList.Count() != 0)
            {
                bookListBox.Visibility = Visibility.Visible;
                // 点击搜索，查询到的书籍列表
                Application.Current.Properties["SearchBookList"] = book.books;
                // 书籍列表
                var newWindow = new page.Window1();
                newWindow.Show();
                //bookListBox.Items.Add( bookList);
            }
            else
            {
                bookListBox.Items.Add("没有此图书");
            }
        }

        private void donate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
