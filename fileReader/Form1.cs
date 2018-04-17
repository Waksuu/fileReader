using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fileReader
{
    public partial class Form1 : Form
    {
        private string folderPath;
        private List<string> vilableFilesToRead = new List<string>();

        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        private  void button1_Click(object sender, EventArgs e)
        {

            #region test




            #endregion



            //List<PhraseLocation> wordsLocation = await phraseLocationAsync();
           List<PhraseLocation> wordsLocation2 = phraseLocation();
            //var test = await testAsync();


            //foreach (var word in wordsLocation)
            //{
            //    MessageBox.Show($"Path: {word.path}\nLine: {word.line}");
            //}
            foreach (var word in wordsLocation2)
            {
                MessageBox.Show($"Path: {word.path}\nLine: {word.line}");
            }

            #region dzialaAleNieOptymalnie

            //string[] words = sr.ReadToEnd().Split(new[] { "\r\n", "\r", "\n", " " }, StringSplitOptions.None);
            //  for (int i = 0; i < words.Length; i++)
            //    {
            //        for (int j = 0; j < wordCount; j++)
            //         {
            //                phraseWithSpaces += words[i+j] + " ";

            //        }
            //        phraseWithSpaces = phraseWithSpaces.Remove(phraseWithSpaces.Length - 1);

            //                if (phrase.Text.Equals(phraseWithSpaces))
            //                {
            //                    wordCounter++;
            //                    MessageBox.Show("Znaleziono fraze");
            //                }

            //        phraseWithSpaces = "";

            //    }

            //// }
            ////string[] words2;
            ////while ((line = sr.ReadLine()) != null)
            ////{
            ////    words2 = ""

            ////    if (line.Contains(phrase.Text))
            ////    {
            ////        MessageBox.Show("Znaleziono fraze");
            ////        break;
            ////    }

            ////}
            ////   MessageBox.Show(wordCount.ToString());

            #endregion dzialaAleNieOptymalnie
        }
        //private List<PhraseLocation> test()
        //{
        //    string line;
        //    var lista = new List<PhraseLocation>();
        //    foreach (var item in vilableFilesToRead)
        //    {
        //        StreamReader sr = new StreamReader(item);

        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            if (line.Contains(phrase.Text))
        //            {
        //                lista.Add(new PhraseLocation { row = 2, col = 1, path = "awdw" });
                       
        //            }

        //        }
        //    }
        //    return lista;
        //}
        //private Task<List<PhraseLocation>> testAsync()
        //{
        //    return Task.Run(() => test());
        //}
    private Task<List<PhraseLocation>> phraseLocationAsync()
        {
            return Task.Run(() => phraseLocation());
        }

        private List<PhraseLocation> phraseLocation()
        {
            var location = new List<PhraseLocation>();
            List<string> lines = new List<string>();
            Stopwatch sw = new Stopwatch();
            string line;
            int lineCount = 0;




            //if (File.Exists(path))
            //{
            //    MessageBox.Show("Plik istnieje");
            //}
            //else
            //{
            //    MessageBox.Show("Plik nie istnieje");
            //}
            sw.Start();
            foreach (var item in vilableFilesToRead)
            {
                StreamReader sr = new StreamReader(item);

                #region optymalnaProba


                while ((line = sr.ReadLine()) != null)
                {
                    lineCount++;
                    bool contains = Regex.IsMatch(line, @"\b" + phrase.Text + @"\b");
                    if (contains)
                    {
                        location.Add(new PhraseLocation { line = lineCount, path = item });
                    }
                }


                sr.Close();
                lineCount = 0;
              
            }
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                MessageBox.Show(ts.ToString());
            return location;

            #endregion optymalnaProba
            //sw.Start();
            //foreach (var item in vilableFilesToRead)
            //{
            //    StreamReader sr = new StreamReader(item);

            //    #region optymalnaProba


            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        lines.Add(line);
            //    }

            //    Parallel.ForEach(lines, listLine =>
            //    {
            //        bool contains = Regex.IsMatch(listLine, @"\b" + phrase.Text + @"\b");
            //        if (contains)
            //        {
            //            location.Add(new PhraseLocation { line = 2, path = item });
            //        }

            //    });
            //    lines.Clear();

            //    sr.Close();

            //}
            //sw.Stop();
            //TimeSpan ts = sw.Elapsed;
            //MessageBox.Show(ts.ToString());
            //return location;

            //#endregion optymalnaProba
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.Description = "Select folder with files to search";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                button1.Enabled = true;
                vilableFilesToRead.Clear();
                folderPath = fbd.SelectedPath;
                MessageBox.Show(folderPath);
                foreach (string file in Directory.GetFiles(@folderPath))
                {
                    if (Path.GetExtension(file) == ".txt")
                    {
                        vilableFilesToRead.Add(file);
                    }
                }
            }
        }
    }
}