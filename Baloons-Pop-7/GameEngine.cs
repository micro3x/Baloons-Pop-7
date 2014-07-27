namespace BalloonsPops
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class GameEngine
    {
        private const byte ROW = 5;
        private const byte COLUMN = 10;

        private int activeCells;
        private int counter;
        private int clearedCells;

        private char[,] tableOfGame;

        private DrawFieldInformation drawer = DrawFieldInformation.Instance;

        public GameEngine()
        {
        }

        public void Start()
        {
            counter = 0;
            clearedCells = 0;
            activeCells = ROW * COLUMN;

            GameField gameField = new GameField(ROW, COLUMN);
            this.tableOfGame = gameField.TableOfGame;

            drawer.DrawWelcomeMessage();

            GameCounter();
        }

        private string ReadTheInput()
        {
            string input = string.Empty;

            if (!IsFinished())
            {
                Console.Write("Enter a row and column: ");
                input = Console.ReadLine();
            }
            else
            {
                Console.Write("You popped all baloons in " + counter + " moves.");
                Console.Write("Please enter your name for the top scoreboard: ");

                input = Console.ReadLine();
                ScoreBoard.AddPlayer(input.ToString(), counter);
                ScoreBoard.Print();
                Start();
            }

            return input;
        }

        private void PlayGame()
        {
            drawer.DrawGameField(tableOfGame);

            int row = -1;
            int col = -1;
            string inputCommand = ReadTheInput();
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
                        ScoreBoard.Print();
                        PlayGame();
                        break;
                    }
                case "restart":
                    {
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
            catch (Exception)
            {
                InvalidCommand();
            }

            if (IsLegalMove(row, col))
            {
                char currentCell = tableOfGame[row, col];
                ClearCells(row, col, currentCell);
            }
            else
            {
                InvalidMove();
            }

            DropDownBaloons();
        }

        private void ClearCells(int indexRow, int indexColumn, char currentCell)
        {
            if ((indexRow >= 0) && (indexRow <= 4) &&
                (indexColumn >= 0) && (indexColumn <= 9) &&
                (tableOfGame[indexRow, indexColumn] == currentCell))
            {
                tableOfGame[indexRow, indexColumn] = '.';
                clearedCells++;
                //Up
                ClearCells(indexRow - 1, indexColumn, currentCell);
                //Down
                ClearCells(indexRow + 1, indexColumn, currentCell);
                //Left
                ClearCells(indexRow, indexColumn + 1, currentCell);
                //Right
                ClearCells(indexRow, indexColumn - 1, currentCell);
            }
            else
            {
                activeCells -= clearedCells;
                clearedCells = 0;
                return;
            }
        }

        private void DropDownBaloons()
        {
            int indexRow;
            int indexColumn;

            Queue<char> queue = new Queue<char>();

            for (indexColumn = COLUMN - 1; indexColumn >= 0; indexColumn--)
            {
                for (indexRow = ROW - 1; indexRow >= 0; indexRow--)
                {
                    if (tableOfGame[indexRow, indexColumn] != '.')
                    {
                        queue.Enqueue(tableOfGame[indexRow, indexColumn]);
                        tableOfGame[indexRow, indexColumn] = '.';
                    }
                }

                indexRow = 4;

                while (queue.Count > 0)
                {
                    tableOfGame[indexRow, indexColumn] = queue.Dequeue();
                    indexRow--;
                }

                queue.Clear();
            }
        }

        private void GameCounter()
        {
            PlayGame();
            counter++;
            GameCounter();
        }

        private bool IsLegalMove(int indexRow, int indexColumn)
        {
            if ((indexRow < 0) || (indexColumn < 0) || (indexColumn > COLUMN - 1) || (indexRow > ROW - 1))
            {
                return false;
            }
            else
            {
                return this.tableOfGame[indexRow, indexColumn] != '.';
            }
        }

        private void InvalidCommand()
        {
            Console.WriteLine("Invalid move or command");
            GameCounter();
        }

        private void InvalidMove()
        {
            Console.WriteLine("Illegal move: cannot pop missing ballon!");
            GameCounter();
        }

        private bool IsFinished()
        {
            return activeCells == 0;
        }

        private void Exit()
        {
            Console.WriteLine("Good Bye");
            Thread.Sleep(1000);
            Console.WriteLine(counter.ToString());
            Console.WriteLine(activeCells.ToString());
            Environment.Exit(0);
        }
    }
}
