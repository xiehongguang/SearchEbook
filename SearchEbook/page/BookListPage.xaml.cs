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
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        SearchBook search = new SearchBook();
        Book[] book = new Book[100];
        ObservableCollection<Book> list = new ObservableCollection<Book>();
        public Window1()
        {
            InitializeComponent();
            InitDateGride();
        }
        public void InitDateGride()
        {
            var bookList=(Book[])Application.Current.Properties["SearchBookList"];
            foreach (var item in bookList)
            {
                var book = new Book();
                book.author = item.author;
                book.title = item.title;
                book.cat = item.cat;
                book.site = item.site;
                book.lastChapter = item.lastChapter;
                book.wordCount = item.wordCount;
                book.shortIntro = item.shortIntro;
                list.Add(book);
            }
            //for (int i = 0; i < 100; i++)
            //{

            //    book[i] = new Book();
            //    book[i].title = "气源车";
            //    book[i].shortIntro = "西周，诗歌与传说的时代。他持弓而立，在缤纷落英中向我走来；他浅浅莞尔，笑容如月华般似曾相识；想变得幸福，心却在摇摆，就像灰姑娘的舞鞋，明明非常合脚，12点时转身...";
            //    list.Add(book[i]);
            //}

            dataGrid.ItemsSource = list;
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}