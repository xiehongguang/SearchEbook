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
            var bookList = (Book[])Application.Current.Properties["SearchBookList"];
            Task.Run(() =>
            {
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
                dataGrid.Dispatcher.InvokeAsync(()=> dataGrid.ItemsSource = list);
            });
          
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("idididiidididiiddiididididididi");
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var index = dataGrid.SelectedIndex;
            Application.Current.Properties["SelectedIndex"] = index;
            var newWindow = new page.BookDetial();
            newWindow.Show();
            //MessageBox.Show(index.ToString());
        }
    }
}