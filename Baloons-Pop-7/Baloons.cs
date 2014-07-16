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
        const string SYMBOLS = "1234";

        private static int activeCells = 0;
        private static int counter = 0;
        private static int clearedCells = 0;
        private static Random randomGenerator = new Random();

        private static SortedDictionary<int, string> statistics = new SortedDictionary<int, string>();

        private static char[,] tableOfGame = new char[ROW, COLUMN];

        private static string inputCommand;

        public static void Start()
        {
            Console.WriteLine("Welcome to “Balloons Pops” game. Please try to pop the balloons." +
                " Use 'top' to view the top scoreboard, 'restart' to start a new game and 'exit' to quit the game.");

            activeCells = ROW * COLUMN;

            InitialiseTableOfGame();
            DrawTableOfGame();
            GameCounter();
        }

        private static void InitialiseTableOfGame()
        {
            for(int indexRow = 0; indexRow < ROW; indexRow++)
            {
                for(int indexColumn = 0; indexColumn < COLUMN; indexColumn++)
                {
                    tableOfGame[indexRow, indexColumn] = GetRandomSymbol();
                }
            }
        }

        private static void GameCounter()
        {
            PlayGame();
            counter++;            
            GameCounter();
        }

        private static bool IsLegalMove(int indexRow, int indexColumn)
        {
            if((indexRow < 0) || (indexColumn < 0) || (indexColumn > COLUMN - 1) || (indexRow > ROW - 1))
            {
                return false;
            }
            else
            {
                return (tableOfGame[indexRow, indexColumn] != '.');
            }
        }

        private static void InvalidCommand()
        {
            Console.WriteLine("Invalid move or command");            
            GameCounter();
        }

        private static void InvalidMove()
        {
            Console.WriteLine("Illegal move: cannot pop missing ballon!");
            GameCounter();
        }

        private static void Exit()
        {
            Console.WriteLine("Good Bye");
            Thread.Sleep(1000);

            Console.WriteLine(counter.ToString());
            Console.WriteLine(activeCells.ToString());
            Environment.Exit(0);
        }

        private static void ReadTheInput()
        {
            if(!IsFinished())
            {
                Console.Write("Enter a row and column: ");
                inputCommand = Console.ReadLine();
            }
            else
            {
                Console.Write("You popped all baloons in " + counter +
                    " moves. Please enter your name for the top scoreboard:");

                inputCommand = Console.ReadLine();
                statistics.Add(counter, inputCommand.ToString());

                PrintAgain();
                inputCommand = string.Empty;
                Start();
            }
        }

        private static void PrintAgain()
        {
            int points = 0;

            Console.WriteLine("Scoreboard:");

            foreach(KeyValuePair<int, string> s in statistics)
            {
                if (points == 4)
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
            int row = -1;
            int col = -1;
       
            ReadTheInput();
            string command = inputCommand.Replace(" ", "");

            switch (command)
            {
                case "":
                    {
                        InvalidCommand();
                        break;
                    }
                case "top":
                    {
                        PrintAgain();
                        inputCommand = string.Empty;
                        PlayGame();
                        break;
                    }
                case "restart":
                    {
                        inputCommand = string.Empty;
                        Start();
                        break;
                    }
                case "exit":
                    {
                        Exit();
                        break;
                    }
            }

            try
            {
                string rowInput = command[0].ToString();
                row = int.Parse(rowInput);
                string colInput = command[1].ToString();
                col = int.Parse(colInput);
            }
            catch(Exception)
            {
                InvalidCommand();
            }

            if(IsLegalMove(row, col))
            {
                char activeCell = tableOfGame[row, col];
                ClearCells(row, col, activeCell);
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
                    char symbol = tableOfGame[indexRow, indexColumn];

                    if (symbol == SYMBOLS[0]) Console.BackgroundColor = ConsoleColor.Red;
                    else if (symbol == SYMBOLS[1]) Console.BackgroundColor = ConsoleColor.Green;
                    else if (symbol == SYMBOLS[2]) Console.BackgroundColor = ConsoleColor.Blue;
                    else if (symbol == SYMBOLS[3]) Console.BackgroundColor = ConsoleColor.Yellow;
                    else Console.ResetColor();
                    
                    Console.Write(symbol + " ");
                    Console.ResetColor();
                }

                Console.Write("| ");
                Console.WriteLine();
            }

            Console.WriteLine("   ---------------------");
        }

        private static void ClearCells(int indexRow, int indexColumn, char activeCell)
        {
            if((indexRow >= 0) && (indexRow <= 4) && (indexColumn <= 9) && (indexColumn >= 0) &&
                (tableOfGame[indexRow, indexColumn] == activeCell))
            {
                tableOfGame[indexRow, indexColumn] = '.';
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

            Queue<char> temp = new Queue<char>();

            for(indexColumn = COLUMN - 1; indexColumn >= 0; indexColumn--)
            {
                for(indexRow = ROW - 1; indexRow >= 0; indexRow--)
                {
                    if(tableOfGame[indexRow, indexColumn] != '.')
                    {
                        temp.Enqueue(tableOfGame[indexRow, indexColumn]);
                        tableOfGame[indexRow, indexColumn] = '.';
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

        private static char GetRandomSymbol()
        {                        
            int randomNumber = randomGenerator.Next(0, SYMBOLS.Length);
            char symbol = SYMBOLS[randomNumber];

            return symbol;
        }
    }
}
