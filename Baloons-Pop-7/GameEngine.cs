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
           this.counter = 0;
           this.clearedCells = 0;
           this.activeCells = ROW * COLUMN;

            GameField gameField = new GameField(ROW, COLUMN);
            this.tableOfGame = gameField.TableOfGame;

            this.drawer.DrawWelcomeMessage();

            this.GameCounter();
        }

        private string ReadTheInput()
        {
            string input = string.Empty;

            if (!this.IsFinished())
            {
                Console.Write("Enter a row and column: ");
                input = Console.ReadLine();
            }
            else
            {
                Console.Write("You popped all baloons in " + this.counter + " moves.");
                Console.Write("Please enter your name for the top scoreboard: ");

                input = Console.ReadLine();
                ScoreBoard.AddPlayer(input.ToString(), this.counter);
                ScoreBoard.Print();
                this.Start();
            }

            return input;
        }

        private void PlayGame()
        {
            this.drawer.DrawGameField(this.tableOfGame);

            int row = -1;
            int col = -1;
            string inputCommand = this.ReadTheInput();
            string command = inputCommand.Replace(" ", "");

            switch (command)
            {
                case "":
                    {
                        this.InvalidCommand();
                        break;
                    }
                case "top":
                    {
                        ScoreBoard.Print();
                        this.PlayGame();
                        break;
                    }
                case "restart":
                    {
                        this.Start();
                        break;
                    }
                case "exit":
                    {
                        this.Exit();
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
                this.InvalidCommand();
            }

            if (this.IsLegalMove(row, col))
            {
                char currentCell = this.tableOfGame[row, col];
                this.ClearCells(row, col, currentCell);
            }
            else
            {
                this.InvalidMove();
            }

            this.DropDownBaloons();
        }

        private void ClearCells(int indexRow, int indexColumn, char currentCell)
        {
            if ((indexRow >= 0) && (indexRow <= 4) &&
                (indexColumn >= 0) && (indexColumn <= 9) &&
                (this.tableOfGame[indexRow, indexColumn] == currentCell))
            {
                this.tableOfGame[indexRow, indexColumn] = '.';
                this.clearedCells++;
                //Up
                this.ClearCells(indexRow - 1, indexColumn, currentCell);
                //Down
                this.ClearCells(indexRow + 1, indexColumn, currentCell);
                //Left
                this.ClearCells(indexRow, indexColumn + 1, currentCell);
                //Right
                this.ClearCells(indexRow, indexColumn - 1, currentCell);
            }
            else
            {
                this.activeCells -= this.clearedCells;
                this.clearedCells = 0;
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
                    if (this.tableOfGame[indexRow, indexColumn] != '.')
                    {
                        queue.Enqueue(this.tableOfGame[indexRow, indexColumn]);
                        this.tableOfGame[indexRow, indexColumn] = '.';
                    }
                }

                indexRow = 4;

                while (queue.Count > 0)
                {
                    this.tableOfGame[indexRow, indexColumn] = queue.Dequeue();
                    indexRow--;
                }

                queue.Clear();
            }
        }

        private void GameCounter()
        {
            this.PlayGame();
            this.counter++;
            this.GameCounter();
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
            this.GameCounter();
        }

        private void InvalidMove()
        {
            Console.WriteLine("Illegal move: cannot pop missing ballon!");
            this.GameCounter();
        }

        private bool IsFinished()
        {
            return this.activeCells == 0;
        }

        private void Exit()
        {
            Console.WriteLine("Good Bye");
            Thread.Sleep(1000);
            Console.WriteLine(this.counter.ToString());
            Console.WriteLine(this.activeCells.ToString());
            Environment.Exit(0);
        }
    }
}
