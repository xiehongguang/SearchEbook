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
            DownloadButton.IsEnabled = false;
            //if (dataGrid==null)
            //{
            //    DownloadButton.Dispatcher.InvokeAsync(() => DownloadButton.IsEnabled = false);
            //}
        }
        /// <summary>
        /// 初始化详情界面，填充数据
        /// </summary>
        public void initData()
        {
            try
            {
                // 点击自动搜索框，数据的id
                var selectedIndex = Int32.Parse(Application.Current.Properties["SelectedIndex"].ToString());
                var bookList = (Book[])Application.Current.Properties["SearchBookList"];
                downloadBook = bookList[selectedIndex];
                bookTitle.Content = bookList[selectedIndex].title;
                bookAuthor.Content = bookList[selectedIndex].author;
                bookCat.Content = bookList[selectedIndex].cat;
                bookWordCount.Content = bookList[selectedIndex].wordCount + "字";
                bookLatelyFollower.Content = bookList[selectedIndex].latelyFollower + " 人";
                booklatestChapter.Content = bookList[selectedIndex].lastChapter;
                bookRetentionRatio.Content = bookList[selectedIndex].retentionRatio + " %";
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
                //Task<string> getValue = common.GetPage(url);
                //var json = await getValue;
                var getValue = Task.Run(() =>
                {
                    string json = common.GetPage(url);
                    //jsonArry
                    var bookSource = JArray.Parse(json);
                    if (String.IsNullOrEmpty(json))
                    {
                        MessageBox.Show("网络错误");
                        return;
                    }
                    Dictionary<string, string> sourceList = new Dictionary<string, string>();
                    List<Source> sourceDetialList = new List<Source>();
                    foreach (var sourceItem in bookSource)
                    {
                        var info = sourceItem.ToObject<Source>();
                        sourceDetialList.Add(info);
                        // 书源的名字 书源内书的id
                        sourceList.Add(info.name, info._id);
                    }
                    // 章节列表
                    url = "http://api.zhuishushenqi.com/toc/" + sourceList.First().Value + "?view=chapters";
                    json = common.GetPage(url);
                    chapter = (ChapterList)common.FromJson("ChapterList", json);

                    if (String.IsNullOrEmpty(json))
                    {
                        MessageBox.Show("网络错误");
                        return;
                    }
                    if (chapter.name == "优质书源")
                    {
                        MessageBox.Show("当前电子书不支持下载");
                        return;
                    }
                    Application.Current.Properties["chapterList"] = chapter;

                    ObservableCollection<ChapterList> list = new ObservableCollection<ChapterList>();

                    foreach (var item in chapter.chapters)
                    {
                            chapter = new ChapterList();
                            chapter.name = item.title;
                            chapter.link = item.link;
                            list.Add(chapter);
                    }
                    DownloadButton.Dispatcher.InvokeAsync(() => DownloadButton.IsEnabled = true);
                    dataGrid.Dispatcher.InvokeAsync(() => dataGrid.ItemsSource = list);
                });
            }catch(Exception ex)
            {
                MessageBox.Show("网络错误");
                return;
            }
            

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

        // 下载完成
        private void downloadComplate()
        {
            downloadInfo.Visibility = Visibility.Hidden;
            downloadProcess.Visibility = Visibility.Hidden;
            DownloadButton.Content = "全本下载";
        }

        // 后台下载完成
        private void BackgroundDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            downloadComplate();
        }

        // 进度条的变化
        private void BackgroundDownload_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            downloadProcess.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                // 获取用户状态
                downloadInfo.Content = e.UserState.ToString();
            }
        }

        // 进行下载
        private void BackgroundDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            // Chapter[] chapterList = new Chapter();
            ChapterList _chapter = (ChapterList)Application.Current.Properties["chapterList"];
            var chapterList = _chapter.chapters;
            var progessBar = downloadProcess;
            var lable = downloadInfo;
            var filePath = e.Argument.ToString();
            // 章节具体信息
            List<ChapterDetial> chaperInfoList = new List<ChapterDetial>();

            for(int i = 0; i < chapterList.Length;i++)
            {
                if(backgroundDownload.CancellationPending)
                {
                    return;
                }
                var chapter = chapterList[i];
                double progressBarRat=(double)(i + 1) / (double)chapterList.Length;
                string info = string.Format("正在下载:{0} {1}/{2} {3:F2}%", chapter.title, i + 1, chapterList.Length,
                  progressBarRat * 100);
                backgroundDownload.ReportProgress(i, info);
                
                while(true)
                {
                    var downloadSuccess = false;
                    var error = "";
                    // 每个源尝试下载3次
                    for (int j = 0; j < 3; j++)
                    {
                        try
                        {
                            var charterInfo = getChapter(chapter.link);
                            if (charterInfo != null)
                            {
                                chaperInfoList.Add(charterInfo);
                                downloadSuccess = true;
                            }
                        }catch (Exception ex)
                        {
                            error = ex.ToString();
                        }
                      
                    }
                    if(!downloadSuccess)
                    {
                        var result = MessageBox.Show(error, "章节 " + chapter.title + " 下载失败", MessageBoxButton.OKCancel);
                        if (result == MessageBoxResult.OK)
                        {
                            return;
                        }
                        else if (result == MessageBoxResult.Cancel)
                        {
                            var emptyChaper = new ChapterDetial();
                            emptyChaper.title = chapter.title;
                            emptyChaper.body = "本章下载失败了，失败原因:\n " + error;
                            chaperInfoList.Add(emptyChaper);
                            downloadSuccess = true;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            backgroundDownload.ReportProgress(chapterList.Length, "正在生成电子书请稍后....");
            // 获取文件拓展名
            var extName = System.IO.Path.GetExtension(filePath);
            var selectedIndex = Int32.Parse(Application.Current.Properties["SelectedIndex"].ToString());
            var bookList = (Book[])Application.Current.Properties["SearchBookList"];
            downloadBook = bookList[selectedIndex];
            KindleBook kindleBook = new KindleBook();
            kindleBook.name = downloadBook.title;
            kindleBook.author = downloadBook.author;
            kindleBook.id = downloadBook._id;
            kindleBook.chapters = chaperInfoList.ToArray();
            if (extName.ToLower() == ".txt")
            {
                Kindlegen.book2Txt(kindleBook, filePath);
            }
            else if (extName.ToLower() == ".mobi")
            {
                Kindlegen.book2Mobi(kindleBook, filePath);
            }
        }

        // 下载章节信息
        private ChapterDetial getChapter(string link)
        {
            try
            {
                ChapterInfo chapterCon = new ChapterInfo();
                // 获取当前时间戳
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                int timestamp = (int)(DateTime.Now - startTime).TotalSeconds;
                string url = "http://chapter2.zhuishushenqi.com/chapter/" + link + "?k=2124b73d7e2e1945&t=" + timestamp + "";
              
                    string json = common.GetPage(url);
                    //var json = common.GetPage(url);
                    if (String.IsNullOrEmpty(json))
                    {
                        MessageBox.Show("网络错误");
                        return null;
                    }
                    chapterCon = (ChapterInfo)common.FromJson("ChapterInfo", json);
                    if (chapterCon.ok)
                    {
                        return chapterCon.chapter;
                    }
                    else
                    {
                        return null;
                    }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
           
        }
    }
}
