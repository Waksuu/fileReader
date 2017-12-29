using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fileReader
{
    public partial class Form1 : Form
    {
        string folderPath;
        List<string> vilableFilesToRead = new List<string>();
      
      
        public Form1()
        {
            
            InitializeComponent();
            button1.Enabled = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            List<PhraseLocation> wordsLocation = await phraseLocationAsync();
            //List<PhraseLocation> wordsLocation = phraseLocation();

            foreach (var word in wordsLocation)
            {
                MessageBox.Show($" Path: {word.path} \n Row: {word.row} \n Col: {word.col}" );
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
            #endregion

        }
        private Task<List<PhraseLocation>> phraseLocationAsync()
        {
            

            return Task.Run(() => phraseLocation()); ;
        }
   
        private List<PhraseLocation> phraseLocation()
        {
            var location = new List<PhraseLocation>();
            List<string> wordFromLine = new List<string>();

            string line;
            int rowCounter = 0;
            int colCounter = 0;
            int wordCount = phrase.Text.Count(char.IsWhiteSpace) + 1;
            int lineWordCounter = 0;

            string phraseWithSpaces = "";

            //if (File.Exists(path))
            //{
            //    MessageBox.Show("Plik istnieje");
            //}
            //else
            //{
            //    MessageBox.Show("Plik nie istnieje");
            //}
           
                foreach (var item in vilableFilesToRead)
                {



                    StreamReader sr = new StreamReader(item);

                    #region optymalnaProba


                    if (wordCount == 1)
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            wordFromLine = line.Split(' ', '\t').ToList();
                            lineWordCounter = line.Count(char.IsWhiteSpace) + 1;
                            rowCounter++;
                            for (int j = 0; j < lineWordCounter; j++)
                            {
                                phraseWithSpaces = wordFromLine[j];
                                colCounter++;
                                if (phrase.Text.Equals(phraseWithSpaces))
                                {

                                    location.Add(new PhraseLocation { row = rowCounter, col = colCounter, path = item });
                                }


                            }
                            colCounter = 0;
                            wordFromLine.Clear();
                            phraseWithSpaces = "";

                            //rowCounter = line.Count(char.IsWhiteSpace) +1;
                            // for (int i = 0; i < rowCounter; i++)
                            //{
                            //    //wordFromLine[i] = line
                            //}
                        }
                        rowCounter = 0;
                    }
                    else
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            
                            wordFromLine = line.Split(' ', '\t').ToList();
                            rowCounter++;
                            if (wordCount > wordFromLine.Count)
                            {
                                continue;
                            }
                            else
                            {
                                lineWordCounter = line.Count(char.IsWhiteSpace) + 1;
                                for (int i = 0; i < (lineWordCounter - wordCount); i++)
                                {
                                    colCounter++;
                                    for (int j = 0; j < wordCount; j++)
                                    {
                                        phraseWithSpaces += wordFromLine[i + j] + " ";



                                    }
                                    phraseWithSpaces = phraseWithSpaces.Remove(phraseWithSpaces.Length - 1);
                                    if (phrase.Text.Equals(phraseWithSpaces))
                                    {
                                        
                                        location.Add(new PhraseLocation { row = rowCounter, col = colCounter, path = item });
                                        
                                    }


                                    phraseWithSpaces = "";

                                }
                                colCounter = 0;

                                wordFromLine.Clear();
                            }

                        }
                        rowCounter = 0;
                    }
                    sr.Close();
                }
                return location;
                #endregion
                
           
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.Description = "Select folder with files to search";
            if(fbd.ShowDialog() == DialogResult.OK)
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
