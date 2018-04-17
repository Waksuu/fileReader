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

using System.Diagnostics;
using System.IO;

using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;

namespace fileReaderWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string folderPath;
        private List<string> vilableFilesToRead = new List<string>();
        
        public MainWindow()
        {

            InitializeComponent();
            
        }

        private List<PhraseLocation> phraseLocation()
        {
             
            var location = new List<PhraseLocation>();
            this.Dispatcher.Invoke(() =>
            {

                List<string> lines = new List<string>();
               
                string line;

                string regexText = phraseTxt.Text;

                int lineCount = 0;
                
             
                foreach (var item in vilableFilesToRead)
                {
                    StreamReader sr = new StreamReader(item);
                    while ((line = sr.ReadLine()) != null)
                    {
                       
                        
                        lineCount++;
                        
                        bool contains = Regex.IsMatch(line, @"\b" + regexText + @"\b");
                        if (contains)
                        {
                            location.Add(new PhraseLocation { Line = lineCount, Path = item });
                            
                        }
                    }
                    sr.Close();
                    lineCount = 0;

                }
              
            });
            return location;

         
            
   
        }
        private Task<List<PhraseLocation>> phraseLocationAsync()
        {
            return Task.Run(() => phraseLocation());
        }

        private void updateGrid(List<PhraseLocation> list)
        {
            
                this.dataGridViev.ItemsSource = list;
          


        }
        private void folderSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.Description = "Select folder with files to search";
            if (fbd.ShowDialog().ToString().Equals("OK"))
            {
                runSeatchBtn.IsEnabled = true;
                vilableFilesToRead.Clear();
                folderPath = fbd.SelectedPath;
                System.Windows.MessageBox.Show(folderPath);
                foreach (string file in Directory.GetFiles(@folderPath))
                {
                    if (System.IO.Path.GetExtension(file) == ".odt")
                    {
                        vilableFilesToRead.Add(file);
                    }
                }
            }
        }

        private async void runSeatchBtn_Click(object sender, RoutedEventArgs e)
        {
            List<PhraseLocation> wordsLocation2 = await phraseLocationAsync();
            updateGrid(wordsLocation2);
           // this.dataGridViev.ItemsSource = wordsLocation2;
            //foreach (var word in wordsLocation2)
            //{
            //    System.Windows.MessageBox.Show($"Path: {word.Path}\nLine: {word.Line}");
            //}
        }
    }
}
