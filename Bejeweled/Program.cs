using System;

namespace Bejeweled
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo info;
            string name;//Player name
            int time = 5;//Set time
            bool exitgame = false;//Does it exists the whole program?
            
            do
            {
                Console.WriteLine("Your name:");
                name = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Select time: a)1 min b) 2 min c) 5 min)\r\n");
                info = Console.ReadKey(true);
                switch (info.Key)
                {
                    case ConsoleKey.A: time = 60; break;
                    case ConsoleKey.B: time = 120; break;
                    case ConsoleKey.C: time = 300; break;
                }
                Game myGame = new Game(time, name);
                do
                {
                    Console.Clear();
                    Console.WriteLine(myGame.DrawGame());//Game drawing

                    info = Console.ReadKey(true);
                    switch (info.Key)// Input Reading
                    {
                        case ConsoleKey.UpArrow: myGame.Move(-1, 0); break;
                        case ConsoleKey.DownArrow: myGame.Move(1, 0); break;
                        case ConsoleKey.LeftArrow: myGame.Move(0, -1); break;
                        case ConsoleKey.RightArrow: myGame.Move(0, 1); break;
                        case ConsoleKey.Spacebar: myGame.SelectOrSwap(); break;
                        case ConsoleKey.Escape: myGame.IsOver = true;  exitgame = true;  break;
                    }
                } while (!myGame.IsGameOver());

            } while (!exitgame);//While we not exiting, the game restarts

        }
    }
}
