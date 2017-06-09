using SearchEbook.Controller;
using SearchEbook.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
using static SearchEbook.Model.AutoComplete;

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
            //string file = Directory.GetCurrentDirectory() + "\\workspace\\tools\\kindlegen.exe";
            //MessageBox.Show(file);
            InitializeComponent();
            //Task t = new Task(() =>
            //  {
            bookName.TextChanged += BookName_TextChanged;
            //  }
            //);
            //t.Start();
            bookListBox.SelectionChanged += BookListBox_SelectionChanged;
        }
        public void useInFileSer()
        {
        }
        private void BookListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (bookListBox.SelectedItem == null) return;

                bookName.Text = bookListBox.SelectedItem.ToString();
                bookListBox.Visibility = Visibility.Hidden;// false;
                searchBook_Click(sender, e);
                string name = bookListBox.SelectedItem.ToString();
                string bookid;
                string id = "0";
                if (bookTitleAndId.TryGetValue(name, out bookid))
                {
                    id = bookid;
                    // 点击自动搜索框，数据的id
                    Application.Current.Properties["SelectAutoSearchId"] = id;
                    var newWindow = new page.BookDetial();
                    newWindow.Show();
                }
                else
                {
                    MessageBox.Show("error");
                }
                //string url = @" http://api.zhuishushenqi.com/book/" + id;
                //MessageBox.Show(url);
                //string json = common.GetPage(url);
                //var bookDetialInfo = new BookDetialInfo();
                //bookDetialInfo = (BookDetialInfo)common.FromJson("BookDetialInfo", json);

                //MessageBox.Show(bookDetialInfo.longIntro);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void BookName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //  Task task = new Task(() =>
            // {
            if (string.IsNullOrWhiteSpace(bookName.Text)) return;
            bookListBox.Items.Clear();
            bookListBox.Visibility = Visibility.Visible;
            bookTitleAndId.Clear();
            string name = bookName.Text.ToString().Trim();
            string url = string.Format("http://api.zhuishushenqi.com/book/auto-complete?query={0}", name);
            string json = common.GetPage(url);
            if (String.IsNullOrEmpty(json))
            {
                MessageBox.Show("网络错误");
                return;
            }
            var bookComplete = new CompleteTitle();
            bookComplete = (CompleteTitle)common.FromJson("CompleteTitle", json);
            List<string> list = new List<string>();

            var ok = bookComplete.ok;
            if (ok)
            {
                if (bookComplete.keywords.Length <= 0)
                {
                    bookListBox.Items.Add("没有此图书");
                }
                else
                {
                    foreach (var item in bookComplete.keywords)
                    {
                        list.Add(item);
                        bookListBox.Items.Add(item);
                    }
                }
            }
            //  });

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
            if (String.IsNullOrEmpty(json))
            {
                MessageBox.Show("网络错误");
                return;
            }
            book = (SearchBook)common.FromJson("SearchBook", json);
            if (book.books == null)
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

        void RunAsync(Action action)
        {
            ((Action)(delegate ()
            {
                action?.Invoke();
            })).BeginInvoke(null, null);
        }

        //void RunInMainthread(Action action)
        //{
        //    this.BeginInvoke((Action)(delegate () {
        //        action?.Invoke();
        //    }));
        //}

    }
}
