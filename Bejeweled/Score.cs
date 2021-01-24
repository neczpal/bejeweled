using System;
using System.Collections.Generic;
using System.IO;

namespace Bejeweled
{
    class Score
    {
        //One line data: (name, score, time)
        class Sor
        {
            string mnev;
            int mert;
            int mtime;
            public Sor(string nev, int ert, int time)
            {
                mnev = nev;
                mert = ert;
                mtime = time;
            }
            public string Nev
            {
                get { return mnev; }
                set { mnev = value; }
            }
            public int Ert
            {
                get { return mert; }
                set { mert = value; }
            }
            public int Time
            {
                get { return mtime; }
                set { mtime = value; }
            }
        }
        List<Sor> highscore;
        
        // Reading the content of the score.txt
        public Score()
        {

            StreamReader sr = new StreamReader(File.Open("score.txt", FileMode.OpenOrCreate));

            highscore = new List<Sor>();
            while (!sr.EndOfStream)
            {
                string temp = sr.ReadLine();
                string[] darabok = temp.Split('\t');

                string tnev = darabok[0];
                int tert = Convert.ToInt32(darabok[1]);
                int ttime = Convert.ToInt32(darabok[2]);
                highscore.Add(new Sor(tnev, tert, ttime));
            }
            sr.Close();
        }
        // Writing the high score to score.txt
        public void saveHighscore()
        {
            StreamWriter sw = new StreamWriter("score.txt");
            for(int i=0; i < highscore.Count; i++)
            {
                sw.WriteLine(highscore[i].Nev+"\t"+
                    Convert.ToString(highscore[i].Ert)+"\t"+
                    Convert.ToString(highscore[i].Time));
            }
            sw.Close();
        }
        // Adding new score to the arranged list
        public void addNew(string nev, int ert, int time)
        {
            for(int i=0; i < highscore.Count; i++)
            {
                // Because its an arranged list we place it in the correct place
                if(highscore[i].Time >= time && highscore[i].Ert <= ert)
                {
                    highscore.Add(new Sor("", 0, 0));
                    for(int j = highscore.Count-1; j > i; j--)
                    {
                        highscore[j] = highscore[j - 1];
                    }
                    highscore[i] = new Sor(nev, ert, time);
                    return;
                }
            }
            // If insertion wasn't necessary we put it in the end
            highscore.Add(new Sor(nev, ert, time));
        }
        // Writing the score board on console
        public void DrawHighscore()
        {
            Console.WriteLine("<======== Score board ========>\n");
            for (int i = 0; i < highscore.Count; i++)
            {
              Console.WriteLine("Name: "+highscore[i].Nev + " " + 
              Convert.ToString("Score: "+highscore[i].Ert) + " " + 
              Convert.ToString("Time: "+highscore[i].Time)+"\n");
            }
        }
    }
}
