namespace BalloonsPops
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class Baloons
    {
        const int ROW = 5;
        const int COLUMN = 10;

        private static int activeCells = 0;
        private static int counter = 0;
        private static int clearedCells = 0;

        private static SortedDictionary<int, string> statistics = new SortedDictionary<int, string> ();

        public static string[,] tableOfGame = new string[ROW, COLUMN];

        public static StringBuilder inputCommand = new StringBuilder ();

        public static void Start ()
        {
            Console.WriteLine ("Welcome to “Balloons Pops” game. Please try to pop the balloons." +
                " Use 'top' to view the top scoreboard, 'restart' to start a new game and 'exit' to quit the game.");

            activeCells = ROW * COLUMN;            

            for (int indexROW = 0; indexROW < ROW; indexROW++)
            {
                for (int indexCOLUMN = 0; indexCOLUMN < COLUMN; indexCOLUMN++)
                {
                    tableOfGame[indexROW, indexCOLUMN] = RND.GetRandomInt ();
                }
            }

            Console.WriteLine ("    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine ("   ---------------------");

            for (int indexROW = 0; indexROW < ROW; indexROW++)
            {
                Console.Write (indexROW + " | ");

                for (int indexCOLUMN = 0; indexCOLUMN < COLUMN; indexCOLUMN++)
                {
                    Console.Write (tableOfGame[indexROW, indexCOLUMN] + " ");
                }

                Console.Write ("| ");
                Console.WriteLine ();
            }

            Console.WriteLine ("   ---------------------");
            GameLogic (inputCommand);
        }

        public static void GameLogic (StringBuilder userInput)
        {
            PlayGame ();
            counter++;
            userInput.Clear ();
            GameLogic (userInput);
        }

        private static bool IsLegalMove (int indexROW, int indexCOLUMN)
        {
            if ((indexROW < 0) || (indexCOLUMN < 0) || (indexCOLUMN > COLUMN - 1) || (indexROW > ROW - 1))
            {
                return false;
            }
            else
            {
                return (tableOfGame[indexROW, indexCOLUMN] != ".");
            }
        }

        private static void InvalidCommand ()
        {
            Console.WriteLine ("Invalid move or command");
            inputCommand.Clear ();
            GameLogic (inputCommand);
        }

        private static void InvalidMove ()
        {
            Console.WriteLine ("Illegal move: cannot pop missing ballon!");
            inputCommand.Clear ();
            GameLogic (inputCommand);
        }

        private static void ShowStatistics ()
        {
            PrintAgain ();
        }

        private static void Exit ()
        {
            Console.WriteLine ("Good Bye");
            Thread.Sleep (1000);

            Console.WriteLine (counter.ToString ());
            Console.WriteLine (activeCells.ToString ());
            Environment.Exit (0);
        }

        private static void Restart ()
        {
            Start ();
        }

        private static void ReadTheIput ()
        {
            if (!IsFinished ())
            {
                Console.Write ("Enter a row and column: ");
                inputCommand.Append (Console.ReadLine ());
            }
            else
            {
                Console.Write ("opal;aaaaaaaa! You popped all baloons in " + counter +
                    " moves. Please enter your name for the top scoreboard:");

                inputCommand.Append (Console.ReadLine ());
                statistics.Add (counter, inputCommand.ToString ());
                PrintAgain ();
                inputCommand.Clear ();
                Start ();
            }
        }

        private static void PrintAgain ()
        {
            int p = 0;

            Console.WriteLine ("Scoreboard:");

            foreach (KeyValuePair<int, string> s in statistics)
            {
                if (p == 4)
                {
                    break;
                }
                else
                {
                    p++;
                    Console.WriteLine ("{0}. {1} --> {2} moves", p, s.Value, s.Key);
                }
            }
        }

        private static void PlayGame ()
        {
            int i = -1;
            int j = -1;


            // change GOTO
            Play:

            ReadTheIput ();

            string hop = inputCommand.ToString ();

            if (inputCommand.ToString () == "")
            {
                InvalidCommand ();
            }

            if (inputCommand.ToString () == "top") 
            { 
                ShowStatistics (); 
                inputCommand.Clear (); 
                goto Play; 
            }
            // change GOTO



            if (inputCommand.ToString () == "restart") 
            { 
                inputCommand.Clear (); 
                Restart (); 
            }

            if (inputCommand.ToString () == "exit")
            {
                Exit ();
            }

            string activeCell;
            inputCommand.Replace (" ", "");

            try
            {
                i = Int32.Parse (inputCommand.ToString ()) / 10;
                j = Int32.Parse (inputCommand.ToString ()) % 10;
            }
            catch (Exception)
            {
                InvalidCommand ();
            }

            if (IsLegalMove (i, j))
            {
                activeCell = tableOfGame[i, j];
                ClearCells (i, j, activeCell);
            }
            else
            {
                InvalidMove ();                
            }

            RemoveBaloons ();

            Console.WriteLine ("    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine ("   ---------------------");

            for (int indexROW = 0; indexROW < ROW; indexROW++)
            {
                Console.Write (indexROW + " | ");

                for (int indexCOLUMN = 0; indexCOLUMN < COLUMN; indexCOLUMN++)
                {
                    Console.Write (tableOfGame[indexROW, indexCOLUMN] + " ");
                }

                Console.Write ("| ");
                Console.WriteLine ();
            }

            Console.WriteLine ("   ---------------------");
        }

        private static void ClearCells (int indexROW, int indexCOLUMN, string activeCell)
        {
            if ((indexROW >= 0) && (indexROW <= 4) && (indexCOLUMN <= 9) && (indexCOLUMN >= 0) && 
                (tableOfGame[indexROW, indexCOLUMN] == activeCell))
            {
                tableOfGame[indexROW, indexCOLUMN] = ".";
                clearedCells++;
                //Up
                ClearCells (indexROW - 1, indexCOLUMN, activeCell);
                //Down
                ClearCells (indexROW + 1, indexCOLUMN, activeCell);
                //Left
                ClearCells (indexROW, indexCOLUMN + 1, activeCell);
                //Right
                ClearCells (indexROW, indexCOLUMN - 1, activeCell);
            }
            else
            {
                activeCells -= clearedCells;
                clearedCells = 0;
                return;
            }
        }

        private static void RemoveBaloons ()
        {
            int indexROW;
            int indexCOLUMN;

            Queue<string> temp = new Queue<string> ();

            for (indexCOLUMN = COLUMN - 1; indexCOLUMN >= 0; indexCOLUMN--)
            {
                for (indexROW = ROW - 1; indexROW >= 0; indexROW--)
                {
                    if (tableOfGame[indexROW, indexCOLUMN] != ".")
                    {
                        temp.Enqueue (tableOfGame[indexROW, indexCOLUMN]);
                        tableOfGame[indexROW, indexCOLUMN] = ".";
                    }
                }

                indexROW = 4;

                while (temp.Count > 0)
                {
                    tableOfGame[indexROW, indexCOLUMN] = temp.Dequeue ();
                    indexROW--;
                }

                temp.Clear ();
            }
        }

        private static bool IsFinished ()
        {
            return (activeCells == 0);
        }
    }
}
