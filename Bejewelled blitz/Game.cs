using System;
using System.Linq;
using System.Timers;

namespace Bejewelled
{
    class Game
    {
        static Random R = new Random();
        const int XSIZE = 24;
        const int YSIZE = 19;

        Tile[,] tiles = new Tile[YSIZE, XSIZE]; // Game matrix
        Timer tr;
        Score scr;
        bool isOver = false; // Is the game over?
        int xpos, ypos; // Actual position
        int xsel, ysel; // Selected position
        int score; // Our score
        bool selected;// Is there selected position?

        int curtime;
        int mtime;
        string mname;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public string Name
        {
            get { return mname; }
            set { mname = value; }
        }
        public bool IsOver
        {
            get { return isOver; }
            set { isOver = value; }
        }

        public Game(int time, string name)
        {
            int x, y;
            ypos = YSIZE / 2;
            xpos = XSIZE / 2;

            xsel = -1;
            ysel = -1;
            selected = false;
            score = 0;
            curtime = 0;

            mtime = time;
            mname = name;

            
            // Game matrix initialisation
            for (y = 0; y < YSIZE; y++)
            {
                for (x = 0; x < XSIZE; x++)
                {
                    tiles[y, x] = new Tile();
                }
            }
            
            // Setting game matrix elements to random tile
            for (y = 0; y < YSIZE; y++)
            {
                for (x = 0; x < XSIZE; x++)
                {
                    tiles[y, x].TileType = random_Tile(x, y);
                }
            }

            // Set timer
            tr = new Timer();
            tr.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            tr.Interval = 1000;
            tr.Enabled = true;


            scr = new Score();
        }

        //Timer event called every second
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (curtime >= mtime)
            {
                IsOver = true;
                ShowEnd();
                return;

            }
            Console.Clear();
            Console.Write(DrawGame());
            curtime++;
        }

        // Moving in the Game
        public void Move(int dy, int dx)
        {
            xpos = (XSIZE + (xpos + dx)) % XSIZE;
            ypos = (YSIZE + (ypos + dy)) % YSIZE;
        }

        // Converting Game matrix into string
        public string DrawGame()
        {
            string s = "";
            for (int y = 0; y < YSIZE; y++)
            {
                for (int x = 0; x < XSIZE; x++)
                {
                    string akt = tiles[y, x].TileType;
                    if (xpos == x && ypos == y || xsel == x && ysel == y)
                    {
                        s += "[" + akt + "]";
                    } else
                    {
                        s += " " + akt + " ";
                    }
                }
                s += "\r\n";
            }
            s += "\nName: " + mname + "\t\t\t Controls:\r\n";
            s += "Score: " + score + "\t\t\t Move: arrows\r\n";
            s += "Time left: " + (mtime-curtime) + "\t\t Swap/Select: space\r\n";

            return s;
        }

        public void ShowEnd()
        {
            // Stop Timer
            tr.Enabled = false;

            // Write game end
            Console.Clear();
            Console.WriteLine("<========= Game ended ==========>");
            Console.WriteLine("Name: " + mname + " Score: " + score + "\r\n");

            // Add score to the list
            scr.addNew(mname, score, mtime);

            // Writing high score board
            scr.DrawHighscore();
            // Saving high score board
            scr.saveHighscore();

            Console.WriteLine("Press a button...\n");
        }

        // Select or Swap depending if there is already a tile selected or not
        public void SelectOrSwap()
        {
            if (selected)
            {
                SwapIfCan();
            }
            else
            {
                Select();
            }
        }
        public bool IsGameOver()
        {
            return IsOver;
        }

        // Set selected position
        private void Select()
        {   
            xsel = xpos;
            ysel = ypos;
            selected = true;
        }
        // Unset selected position
        private void Unselect()
        {
            xsel = -1;
            ysel = -1;
            selected = false;
        }

        // Delete in left direction until it equals to 'our' tile
        private int TorlesBalra(int x, int y, string mi)
        {
            int score = 0;
            int i = x; 
            while(i >= 0 && tiles[y, i].TileType == mi)
            {
                tiles[y, i].TileType = "-";
                score++;
                i--;
            }
            return score;
        }
        // Delete in right direction until it equals to 'our' tile
        private int TorlesJobbra(int x, int y, string mi)
        {
            int score = 0;
            int i = x;
            while (i <= XSIZE-1 && tiles[y, i].TileType == mi)
            {
                tiles[y, i].TileType = "-";
                score++;
                i++;
            }
            return score;
        }
        // Delete in up direction until it equals to 'our' tile
        private int TorlesFel(int x, int y, string mi)
        {
            int score = 0;
            int i = y;
            while (i >= 0 && tiles[i, x].TileType == mi)
            {
                tiles[i, x].TileType = "-";
                score++;
                i--;
            }
            return score;
        }
        // Delete in down direction until it equals to 'our' tile
        private int TorlesLe(int x, int y, string mi)
        {
            int score = 0;
            int i = y;
            while (i <= YSIZE-1 && tiles[i, x].TileType == mi)
            {
                tiles[i, x].TileType = "-";
                score++;
                i++;
            }
            return score;
        }

        // Delete the same tiletypes and gives back the number of deleted tiles
        private int RemoveSame(int x, int y)
        {
            if (tiles[y, x].TileType == "-")
                return 0;

            int score = 0;
            //If it equals to the left one then it deletes that way, if right equals then there etc..
            if (x > 0 && tiles[y, x - 1].equals(tiles[y, x]))
            {
                score += TorlesBalra(x - 1, y, tiles[y,x].TileType);
            }
            if (x < XSIZE-1 && tiles[y, x + 1].equals(tiles[y, x]))
            {
                score += TorlesJobbra(x + 1, y, tiles[y, x].TileType);
            }
            if (y > 0 && tiles[y - 1, x].equals(tiles[y, x]))
            {
                score += TorlesFel(x, y - 1, tiles[y, x].TileType);
            }
            if (y < YSIZE-1 && tiles[y + 1, x].equals(tiles[y, x]))
            {
                score += TorlesLe(x, y + 1, tiles[y, x].TileType);
            }
            if(score > 0)//If any deletion happened delete self and gives back the deleted number of tiles
            {
                score++;
                tiles[y, x].TileType = "-";
            }
            return score;
        }
        // Filling the empty spots with 'falling tiles' from above
        private void Collide()
        {
            int x, y;
            bool end;
            do
            {
                end = true;
                for (y = 0; y < YSIZE; y++)
                {
                    for (x = 0; x < XSIZE; x++)
                    {
                        if (y > 0 && tiles[y, x].TileType == "-" && tiles[y-1, x].TileType != "-")
                        {
                            Swap(x, y, x, y-1);
                            end = false;
                        }
                    }
                }
            } while (!end);// Swaping empty tiles upwards until every one is on the top
            int chainreac_score = 0;// Chainreaction score
            for (y = 0; y < YSIZE; y++)
            {
                for (x = 0; x < XSIZE; x++)
                {
                    chainreac_score += RemoveSame(x, y);// Deleting the same tiles
                }
            }
            score += chainreac_score;
            if(chainreac_score > 0)// If there was a deletion during chain reaction we must do it again
            {
                Collide();
            }
        }
        //Filling empty tiles with random values
        private void addNewTiles()
        {
            int x, y;
            for (y = 0; y < YSIZE; y++)
            {
                for (x = 0; x < XSIZE; x++)
                {
                    if(tiles[y, x].TileType == "-")
                        tiles[y, x].TileType = random_Tile(x, y);
                }
            }
        }
        // Swaping the selected and the actual tile
        private void SwapCurrent()
        {
            Swap(xsel, ysel, xpos, ypos);
        }
        // Swapping (x1,y1) and (x2, y2) tile
        private void Swap(int x1, int y1, int x2, int y2)
        {
            Tile tile;
            tile = tiles[y1, x1];
            tiles[y1, x1] = tiles[y2, x2];
            tiles[y2, x2] = tile;
        }

        // Swapping the selected and the actual tile if they are neighbours
        // And delete if they are same and let the above tiles fall
        // Also filling the empty tiles with random ones
        private void SwapIfCan()
        {
            // We check if selected and actual distance is 1
            if (Math.Abs(xsel - xpos) == 1 && Math.Abs(ysel - ypos) == 0 ||
                     Math.Abs(xsel - xpos) == 0 && Math.Abs(ysel - ypos) == 1)
            {
                SwapCurrent();

                // Deleting around the selected and the actual tile if any equals

                int swapscore = RemoveSame(xpos, ypos) + RemoveSame(xsel, ysel);
                // If there was a deletion we play a sound let the tiles fall and fill with new ones
                if (swapscore > 0)
                {
                    //H
                    Console.Beep();
                    Collide();
                    addNewTiles();
                }
                else// If no point was obtained then we swap back
                {
                    SwapCurrent();
                }
                score += swapscore;
                Unselect();
            }// Else we unselect
            else
            {
                Unselect();
            }

        }
        // Gives back a random tile on the position that is not like its neighbours
        private string random_Tile(int x, int y)
        {
            // Possible tiles
            string[] options = { "#", "@", "&", "%", "$", "☺", "?", "O", "H", "X"};
            var optionsList = options.ToList();

            // Removing neighbors from the possible tiles
            if (x > 0)
                optionsList.Remove(tiles[y, x - 1].TileType);
            if (x < XSIZE - 1)
                optionsList.Remove(tiles[y, x + 1].TileType);
            if (y > 0)
                optionsList.Remove(tiles[y - 1, x].TileType);
            if (y < YSIZE - 1)
                optionsList.Remove(tiles[y + 1, x].TileType);

            options = optionsList.ToArray();

            //Randomizing a random tile
            return options[R.Next(optionsList.Count)];
        }
    }
}
