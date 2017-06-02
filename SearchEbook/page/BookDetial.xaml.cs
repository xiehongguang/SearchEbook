using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using SearchEbook.Controller;
using SearchEbook.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        // 所下载书籍的信息
        Book downloadBook = new Book();
        // 章节列表
        ChapterList chapter = new ChapterList();
        // 后台工作
        BackgroundWorker backgroundDownload = new BackgroundWorker();

        public BookDetial()
        {
            InitializeComponent();
            backgroundDownload.WorkerReportsProgress = true;
            backgroundDownload.WorkerSupportsCancellation = true;
            backgroundDownload.DoWork += BackgroundDownload_DoWork;
            backgroundDownload.ProgressChanged += BackgroundDownload_ProgressChanged;
            backgroundDownload.RunWorkerCompleted += BackgroundDownload_RunWorkerCompleted;
            initData();
        }

        private void BackgroundDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //downloadComplate();
        }

        private void BackgroundDownload_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            downloadProcess.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                downloadInfo.Content = e.UserState.ToString();
            }
        }

        private void BackgroundDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 初始化详情界面，填充数据
        /// </summary>
        public void initData()
        {
            // 点击自动搜索框，数据的id
            var selectedIndex = Int32.Parse( Application.Current.Properties["SelectedIndex"].ToString());
            var bookList = (Book[])Application.Current.Properties["SearchBookList"];
            downloadBook = bookList[selectedIndex];
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

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            // 正在执行耗时任务  &&  正在取消任务
            if (backgroundDownload.IsBusy && backgroundDownload.CancellationPending)
            {
                return;
            }
            // 正在执行耗时任务
            if (backgroundDownload.IsBusy)
            {
                StopDownload();
                return;
            }

            // 保存文件
            SaveFileDialog file = new SaveFileDialog();
            // 筛选文件类型
            file.Filter = "Kindel（*.mobi）|*.mobi|文本文件（*.txt）|*.txt";
            // 默认筛选文件按的类型索引
            file.FilterIndex = 0;
            // 文件名字  包含路径
            file.FileName = downloadBook.title;
            // 关闭窗口，将目录还原
            file.RestoreDirectory = true;
            MessageBox.Show(file.FileName);
            // 打开文件夹
            if(file.ShowDialog()==true)
            {
                StartDownload(file.FileName);
            }

        }
        private void StartDownload(string filePath)
        {
            DownloadButton.Content = "取消";
            downloadInfo.Content = "即将开始下载...";
            downloadProcess.Value = 0;
            downloadProcess.Maximum = dataGrid.Items.Count;
            downloadInfo.Visibility = Visibility.Visible;
            downloadProcess.Visibility = Visibility.Visible;
            // 开始下载
            backgroundDownload.RunWorkerAsync(filePath);
        }
        // 停止下载
        private void StopDownload()
        {
            if (backgroundDownload.IsBusy )
            {
                // 取消任务
                backgroundDownload.CancelAsync();
                DownloadButton.Content = "正在取消";
            }
        }
    }
}
