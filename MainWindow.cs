using HtmlAgilityPack;
using MaterialDesignThemes.Wpf;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using Xceed.Words.NET;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sony_giftcard_generator
{
    public partial class MainWindow : Window, IComponentConnector
    {
        private List<string> sha1 = new List<string>();

        private const string sonypage2 = "sony\\2page.docx";

        private const string sonypage1 = "sony\\1page.docx";

        private const string sonyregex = "sony\\regex.json";

        private const string s3page2 = "sony\\saudi\\3month\\2page.docx";

        private const string s3page1 = "sony\\saudi\\3month\\1page.docx";

        private const string s3regex = "sony\\saudi\\3month\\regex.json";

        private const string s12page2 = "sony\\saudi\\12month\\2page.docx";

        private const string s12page1 = "sony\\saudi\\12month\\1page.docx";

        private const string s12regex = "sony\\saudi\\12month\\regex.json";

        private const string s1page2 = "sony\\saudi\\1month\\2page.docx";

        private const string s1page1 = "sony\\saudi\\1month\\1page.docx";

        private const string s1regex = "sony\\saudi\\1month\\regex.json";

        private const string situnespage2 = "itunes-saudi\\2page.docx";

        private const string situnespage1 = "itunes-saudi\\1page.docx";

        private const string situnesregex = "itunes-saudi\\regex.json";

        private const string itunespage2 = "itunes\\2page.docx";

        private const string itunespage1 = "itunes\\1page.docx";

        private const string itunesregex = "itunes\\regex.json";

        public MainWindow()
        {
            InitializeComponent();
            sha1.AddRange(LogWriter.ReadLogs());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (CommonOpenFileDialog folderBrowserDialog = new CommonOpenFileDialog())
            {
                folderBrowserDialog.IsFolderPicker = true;
                htmlfile.Text = ((folderBrowserDialog.ShowDialog() == CommonFileDialogResult.Ok) ? folderBrowserDialog.FileName : "");
                htmlfile.Background = Brushes.White;
            }
        }

        // Get the respective Regular expression for a certain card type
        public string[] LoadRegex(byte cardtype)
        {

        }

        private void VerySimpleReplaceText(string ResultFile, List<string> origText, List<string> replaceText, int pages, int cards, byte cardtype)
        {
            DocX docX = DocX.Create(ResultFile);
            DocX docX2 = null;
            DocX docX3 = null;
            switch (cardtype)
            {
                case 0:
                    docX2 = DocX.Load("sony\\1page.docx");
                    docX3 = DocX.Load("sony\\2page.docx");
                    break;
                case 1:
                    docX2 = DocX.Load("itunes\\1page.docx");
                    docX3 = DocX.Load("itunes\\2page.docx");
                    break;
                case 2:
                    docX2 = DocX.Load("itunes-saudi\\1page.docx");
                    docX3 = DocX.Load("itunes-saudi\\2page.docx");
                    break;
                case 3:
                    docX2 = DocX.Load("sony\\saudi\\3month\\1page.docx");
                    docX3 = DocX.Load("sony\\saudi\\3month\\2page.docx");
                    break;
                case 4:
                    docX2 = DocX.Load("sony\\saudi\\12month\\1page.docx");
                    docX3 = DocX.Load("sony\\saudi\\12month\\2page.docx");
                    break;
                case 5:
                    docX2 = DocX.Load("sony\\saudi\\1month\\1page.docx");
                    docX3 = DocX.Load("sony\\saudi\\1month\\2page.docx");
                    break;
            }
            int i;
            for (i = 0; i < pages; i++)
            {
                if (i == pages - 1 && cards % 2 == 1)
                {
                    docX.InsertDocument(docX2);
                }
                else
                {
                    docX.InsertDocument(docX3);
                }
                if (cardtype != 3 && cardtype != 4 && cardtype != 5)
                {
                    for (int num = origText.Count - 1; num >= ((origText.Count == 5) ? (origText.Count - 5) : (origText.Count - 10)); num--)
                    {
                        docX.ReplaceText(origText[num], replaceText[num]);
                    }
                }
                else
                {
                    for (int num2 = origText.Count - 1; num2 >= ((origText.Count == 3) ? (origText.Count - 3) : (origText.Count - 6)); num2--)
                    {
                        docX.ReplaceText(origText[num2], replaceText[num2]);
                    }
                }
                if (cardtype != 3 && cardtype != 4 && cardtype != 5)
                {
                    if (origText.Count >= 10)
                    {
                        origText.RemoveRange(origText.Count - 10, 10);
                    }
                }
                else if (origText.Count >= 6)
                {
                    origText.RemoveRange(origText.Count - 6, 6);
                }
            }
            docX.Save();
            docX2.Dispose();
            docX3.Dispose();
            docX.Dispose();
            LogWriter.WriteLog(sha1.ToArray());
        }

        public static string[] SearchGet(string source, string startMatch, string endMatch, int maxChar, bool includeEnds)
        {
            int num = source.IndexOf(startMatch);
            int num2 = 0;
            List<string> list = new List<string>();
            while (num != -1)
            {
                num += startMatch.Length;
                num2 = source.IndexOf(endMatch, num);
                if (num2 != -1)
                {
                    if (includeEnds)
                    {
                        num2 += endMatch.Length;
                    }
                    string text = source.Substring(num, num2 - num);
                    if (text.Length <= maxChar)
                    {
                        list.Add(text);
                    }
                    num = num2;
                    num = source.IndexOf(startMatch, num);
                }
                else
                {
                    num = -1;
                }
            }
            return list.ToArray();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (htmlfile.Text != "")
            {
                htmlfile.Background = Brushes.White;
                status.Foreground = Brushes.Black;
                status.ToolTip = "";
                printbtn.IsEnabled = false;
                progress.IsIndeterminate = true;
                Thread thread = new Thread(() => sonyCards(false))
                {
                    IsBackground = true
                };
                thread.Start();
            }
            else
            {
                errorMessage.Text = "Fill in the necessary information before proceeding to the print operation";
                dialError.IsOpen = true;
                htmlfile.Background = new SolidColorBrush(Color.FromArgb(30, Colors.Red.R, Colors.Red.G, Colors.Red.B));
                htmlfile.Focus();
            }
        }

        private async void sonyCards(bool oneFile)
        {
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            bool flag = false;
            htmlDocument.OptionFixNestedTags = true;
            string htmlfilepath = "";
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
            {
                htmlfilepath = htmlfile.Text;
            });

            List<string> rcodes = new List<string>();
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            List<string> list4 = new List<string>();

            byte b = 0;
            string[] array = null;
            bool flag2 = false;


            if (!oneFile)
            {
                array = Directory.GetFiles(htmlfilepath, "*.html");
            }
            else
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Filter = "Html file (*.html)|*.html";
                if (openFileDialog.ShowDialog() == true)
                {
                    array = new string[1]
                    {
                        openFileDialog.FileName
                    };
                }
                else
                {
                    flag2 = true;
                }
            }
            string[] array2 = array;
            foreach (string file in array2)
            {
                htmlDocument.Load(file);
                StreamReader streamReader = new StreamReader(file, Encoding.UTF8);
                if (htmlDocument.DocumentNode == null)
                {
                    continue;
                }
                HtmlNode htmlNode = htmlDocument.DocumentNode.SelectSingleNode("//body");
                string text = streamReader.ReadToEnd();
                int num = 0;
                if (htmlNode == null)
                {
                    continue;
                }
                string text2 = string.Empty;
                Match match = null;
                if (text.Contains("ايتونز"))
                {
                    b = (byte)((!text.Contains("ريال")) ? 1 : 2);
                }
                else if (text.Contains(" أشهر "))
                {
                    b = 3;
                }
                else if (text.Contains("12شهر"))
                {
                    b = 4;
                }
                else if (text.Contains(" شهر "))
                {
                    b = 5;
                }
                else
                {

                }



                switch (b)
                {
                    case 0:
                    case 3:
                    case 4:
                    case 5:
                        text2 = "[A-Z0-9]{4}[-][A-Z0-9]{4}[-][A-Z0-9]{4}";
                        break;
                    case 1:
                    case 2:
                        text2 = "[A-Z0-9]{16}";
                        break;
                }
                match = Regex.Match(htmlNode.InnerText, text2);
                while (match.Success)
                {
                    string sHA1Hash = DTHasher.GetSHA1Hash(match.Value);
                    if (!sha1.Contains(sHA1Hash))
                    {
                        if (b != 1 || !Regex.IsMatch(match.Value, "[A-Z]{3}\\d{13}"))
                        {
                            switch (b)
                            {
                                case 0:
                                case 3:
                                case 4:
                                case 5:
                                    Trace.Assert(match.Value.Length == 14, "FATAL ERROR: Sony cards must be 14 characters");
                                    break;
                                case 1:
                                case 2:
                                    Trace.Assert(match.Value.Length == 16, "FATAL ERROR: Itunes cards must be 16 characters");
                                    break;
                            }
                            rcodes.Add(match.Value);
                            num++;
                            sha1.Add(sHA1Hash);
                        }
                        match = match.NextMatch();
                    }
                    else
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            status.Foreground = Brushes.Red;
                            status.Content = "Error: \"" + Path.GetFileName(file) + "\" contains duplicate code.";
                            status.ToolTip = status.Content;
                        }, new object[0]);
                        flag = true;
                        streamReader.Close();
                        streamReader.Dispose();
                    }
                }
                if (flag)
                {
                    break;
                }
                switch (b)
                {
                    case 0:
                    case 3:
                    case 4:
                    case 5:
                        text2 = "(?>)\\d{8}(-)\\d{6}";
                        break;
                    case 1:
                        text2 = "[A-Z]{3}\\d{13}";
                        break;
                    case 2:
                        text2 = "[A-Z]{3}\\d{11}";
                        break;
                }
                bool flag3 = false;
                while (true)
                {
                    match = Regex.Match(htmlNode.InnerText, text2);
                    int num2 = 0;
                    while (match.Success)
                    {
                        if (b == 0 || b == 3 || b == 4 || b == 5)
                        {
                            string[] array3 = match.Value.Split('-');
                            string item = array3[1].Trim() + "-" + array3[0].Trim();
                            list2.Add(item);
                        }
                        else
                        {
                            list2.Add(match.Value);
                        }
                        num2++;
                        match = match.NextMatch();
                    }
                    if (num2 == num)
                    {
                        break;
                    }
                    if (b == 1)
                    {
                        text2 = "[A-z]{4}_\\$\\d{1,3}_\\d{10}";
                    }
                    else
                    {
                        flag3 = true;
                    }
                    if (flag3)
                    {
                        AutoResetEvent manualResetEvent = new AutoResetEvent(false);
                        this.Dispatcher.Invoke((Action)async delegate
                        {

                            var viewmsg = new AskMessage
                            {
                                DataContext = new SampleDialogViewModel(),
                                AskText = "Enter the correct regular expression to find the missing serial codes",
                                AnswerText = text2
                            };

                            text2 = (string)await DialogHost.Show(viewmsg, "RootDialog", DClose);
                            manualResetEvent.Set();
                        });

                        manualResetEvent.WaitOne();
                    }
                    flag3 = true;
                }
                if (htmlNode.InnerText.Contains("Date:"))
                {
                    text2 = "(\\d{1,2})\\/(\\d{1,2})\\/(\\d{4}) (\\d{2}):(\\d{2}):(\\d{2})\\s*(AM|PM|am|pm)";
                    match = Regex.Match(htmlNode.InnerText, text2);
                    while (match.Success)
                    {
                        list3.Add(match.Value);
                        match = match.NextMatch();
                    }
                    text2 = "(?<=\\$)(\\d{1,3})(?= PSN)";
                    match = Regex.Match(htmlNode.InnerHtml, text2);
                    while (match.Success)
                    {
                        list4.Add(match.Value);
                        match = match.NextMatch();
                    }
                }
                else
                {
                    string[] array4 = SearchGet(text, "التاريخ", " م", 40, includeEnds: true);
                    string[] array5 = SearchGet(text, "التاريخ", " ص", 40, includeEnds: true);
                    list3.AddRange(array4);
                    list3.AddRange(array5);
                    _ = array4?.LongLength;
                    _ = array5?.LongLength;
                    if (b != 3 && b != 4 && b != 5)
                    {
                        text2 = "(?<= )\\d{1,3}(?= )";
                        match = Regex.Match(htmlNode.InnerText, text2);
                        while (match.Success)
                        {
                            list4.Add(match.Value);
                            match = match.NextMatch();
                        }
                    }
                }
                streamReader.Close();
                streamReader.Dispose();
            }
            if (flag2 || rcodes.Count <= 0 || flag)
            {
                return;
            }
            for (int num3 = list3.Count - 1; num3 >= 0; num3--)
            {
                list3[num3] = list3[num3].Remove(0, 1).Trim();
            }
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Docx Document (*.Docx)|*.Docx";
            string text3 = (saveFileDialog.ShowDialog() == true) ? saveFileDialog.FileName : "";
            if (!(text3 != ""))
            {
                return;
            }
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
            {
                status.Content = "Status: processing...";
            }, new object[0]);
            int pages = (int)Math.Ceiling((double)rcodes.Count / 2.0);
            List<string> list5 = new List<string>();
            List<string> list6 = new List<string>();
            bool flag4 = true;
            int count = rcodes.Count;
            if (b == 3 || b == 4 || b == 5)
            {
                Trace.Assert(rcodes.Count == list3.Count && list3.Count == list2.Count, $"Fatal error: cards, dates and serials don't match each other!\ncards: {list.Count.ToString()}, dates: {list3.Count.ToString()}, serials: {list2.Count.ToString()}");
            }
            else
            {
                Trace.Assert(rcodes.Count == list3.Count && list3.Count == list4.Count && list4.Count == list2.Count, $"Fatal error: cards, dates, serials and dollars don't match each other!\ncards: {list.Count.ToString()}, dates: {list3.Count.ToString()}, dollars: {list4.Count.ToString()}, serials: {list2.Count.ToString()}");
            }
            for (int j = 0; j < count; j++)
            {
                if (flag4)
                {
                    if (b != 3 && b != 4 && b != 5)
                    {
                        list5.Add("ZA");
                        list6.Add(list4[j]);
                        list5.Add("ZA");
                        list6.Add(list4[j]);
                    }
                    list5.Add("CODE1CODE2CODA");
                    list6.Add(rcodes[j]);
                    list5.Add("SERIALXXXXXXXXA");
                    list6.Add(list2[j]);
                    list5.Add("DATEINSERTXXXXXXXXXXA");
                    list6.Add(list3[j]);
                }
                else
                {
                    if (b != 3 && b != 4 && b != 5)
                    {
                        list5.Add("ZB");
                        list6.Add(list4[j]);
                        list5.Add("ZB");
                        list6.Add(list4[j]);
                    }
                    list5.Add("CODE1CODE2CODB");
                    list6.Add(rcodes[j]);
                    list5.Add("SERIALXXXXXXXXB");
                    list6.Add(list2[j]);
                    list5.Add("DATEINSERTXXXXXXXXXXB");
                    list6.Add(list3[j]);
                }
                flag4 = !flag4;
            }
            VerySimpleReplaceText(text3, list5, list6, pages, rcodes.Count, b);
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
            {
                printbtn.IsEnabled = true;
                status.Content = "Status: finished.";
                progress.IsIndeterminate = false;
            }, new object[0]);
        }

        private void DClose(object sender, DialogClosingEventArgs eventargs)
        {

        }

        private void Grid_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                string[] array = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
                if (File.Exists(array[0]))
                {
                    htmlfile.Text = array[0];
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() => sonyCards(true))
            {
                IsBackground = true
            };
            thread.Start();
        }

        private void htmlfile_Drop(object sender, System.Windows.DragEventArgs e)
        {
        }

        async Task<string> ShowAsk(string text)
        {
            var viewmsg = new AskMessage
            {
                DataContext = new SampleDialogViewModel(),
                AskText = "Enter the correct regular expression to find the missing serial codes",
                AnswerText = text
            };

            return (string)DialogHost.Show(viewmsg, "RootDialog", DClose).GetAwaiter().GetResult();
        }
    }
}
