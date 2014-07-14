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

        private static SortedDictionary<int, string> statistics = new SortedDictionary<int, string>();

        public static string[,] tableOfGame = new string[ROW, COLUMN];

        public static StringBuilder inputCommand = new StringBuilder();

        public static void Start()
        {
            Console.WriteLine("Welcome to “Balloons Pops” game. Please try to pop the balloons." +
                " Use 'top' to view the top scoreboard, 'restart' to start a new game and 'exit' to quit the game.");

            activeCells = ROW * COLUMN;

            InitialiseTableOfGame();
            DrawTableOfGame();
            GameLogic(inputCommand);
        }

        private static void InitialiseTableOfGame()
        {
            for(int indexRow = 0; indexRow < ROW; indexRow++)
            {
                for(int indexColumn = 0; indexColumn < COLUMN; indexColumn++)
                {
                    tableOfGame[indexRow, indexColumn] = RND.GetRandomInt();
                }
            }
        }

        private static void GameLogic(StringBuilder userInput)
        {
            PlayGame();
            counter++;
            userInput.Clear();
            GameLogic(userInput);
        }

        private static bool IsLegalMove(int indexRow, int indexColumn)
        {
            if((indexRow < 0) || (indexColumn < 0) || (indexColumn > COLUMN - 1) || (indexRow > ROW - 1))
            {
                return false;
            }
            else
            {
                return (tableOfGame[indexRow, indexColumn] != ".");
            }
        }

        private static void InvalidCommand()
        {
            Console.WriteLine("Invalid move or command");
            inputCommand.Clear();
            GameLogic(inputCommand);
        }

        private static void InvalidMove()
        {
            Console.WriteLine("Illegal move: cannot pop missing ballon!");
            inputCommand.Clear();
            GameLogic(inputCommand);
        }

        private static void ShowStatistics()
        {
            PrintAgain();
        }

        private static void Exit()
        {
            Console.WriteLine("Good Bye");
            Thread.Sleep(1000);

            Console.WriteLine(counter.ToString());
            Console.WriteLine(activeCells.ToString());
            Environment.Exit(0);
        }

        private static void Restart()
        {
            Start();
        }

        private static void ReadTheInput()
        {
            if(!IsFinished())
            {
                Console.Write("Enter a row and column: ");
                inputCommand.Append(Console.ReadLine());
            }
            else
            {
                Console.Write("opal;aaaaaaaa! You popped all baloons in " + counter +
                    " moves. Please enter your name for the top scoreboard:");

                inputCommand.Append(Console.ReadLine());
                statistics.Add(counter, inputCommand.ToString());

                PrintAgain();
                inputCommand.Clear();
                Start();
            }
        }

        private static void PrintAgain()
        {
            int points = 0;

            Console.WriteLine("Scoreboard:");

            foreach(KeyValuePair<int, string> s in statistics)
            {
                if(points == 4)
                {
                    break;
                }
                else
                {
                    points++;
                    Console.WriteLine("{0}. {1} --> {2} moves", points, s.Value, s.Key);
                }
            }
        }

        private static void PlayGame()
        {
            int i = -1;
            int j = -1;


            // change GOTO
        Play:

            ReadTheInput();

            string hop = inputCommand.ToString();

            if(inputCommand.ToString() == "")
            {
                InvalidCommand();
            }

            if(inputCommand.ToString() == "top")
            {
                ShowStatistics();
                inputCommand.Clear();
                goto Play;
            }
            // change GOTO



            if(inputCommand.ToString() == "restart")
            {
                inputCommand.Clear();
                Restart();
            }

            if(inputCommand.ToString() == "exit")
            {
                Exit();
            }

            string activeCell;
            inputCommand.Replace(" ", "");

            try
            {
                i = Int32.Parse(inputCommand.ToString()) / 10;
                j = Int32.Parse(inputCommand.ToString()) % 10;
            }
            catch(Exception)
            {
                InvalidCommand();
            }

            if(IsLegalMove(i, j))
            {
                activeCell = tableOfGame[i, j];
                ClearCells(i, j, activeCell);
            }
            else
            {
                InvalidMove();
            }

            RemoveBaloons();

            DrawTableOfGame();
        }

        private static void DrawTableOfGame()
        {
            Console.WriteLine("    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("   ---------------------");

            for(int indexRow = 0; indexRow < ROW; indexRow++)
            {
                Console.Write(indexRow + " | ");

                for(int indexColumn = 0; indexColumn < COLUMN; indexColumn++)
                {
                    Console.Write(tableOfGame[indexRow, indexColumn] + " ");
                }

                Console.Write("| ");
                Console.WriteLine();
            }

            Console.WriteLine("   ---------------------");
        }

        private static void ClearCells(int indexRow, int indexColumn, string activeCell)
        {
            if((indexRow >= 0) && (indexRow <= 4) && (indexColumn <= 9) && (indexColumn >= 0) &&
                (tableOfGame[indexRow, indexColumn] == activeCell))
            {
                tableOfGame[indexRow, indexColumn] = ".";
                clearedCells++;
                //Up
                ClearCells(indexRow - 1, indexColumn, activeCell);
                //Down
                ClearCells(indexRow + 1, indexColumn, activeCell);
                //Left
                ClearCells(indexRow, indexColumn + 1, activeCell);
                //Right
                ClearCells(indexRow, indexColumn - 1, activeCell);
            }
            else
            {
                activeCells -= clearedCells;
                clearedCells = 0;
                return;
            }
        }

        private static void RemoveBaloons()
        {
            int indexRow;
            int indexColumn;

            Queue<string> temp = new Queue<string>();

            for(indexColumn = COLUMN - 1; indexColumn >= 0; indexColumn--)
            {
                for(indexRow = ROW - 1; indexRow >= 0; indexRow--)
                {
                    if(tableOfGame[indexRow, indexColumn] != ".")
                    {
                        temp.Enqueue(tableOfGame[indexRow, indexColumn]);
                        tableOfGame[indexRow, indexColumn] = ".";
                    }
                }

                indexRow = 4;

                while(temp.Count > 0)
                {
                    tableOfGame[indexRow, indexColumn] = temp.Dequeue();                    
                    indexRow--;
                }

                temp.Clear();
            }
        }

        private static bool IsFinished()
        {
            return (activeCells == 0);
        }
    }
}
