namespace BalloonsPops
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GameField
    {
        private const string SYMBOLS = "1234";

        public GameField(int rows, int cols)
        {
            this.Rows = rows;
            this.Cols = cols;
            this.TableOfGame = this.InitialiseTableOfGame(this.Rows, this.Cols);
        }

        public int Rows { get; private set; }

        public int Cols { get; private set; }

        public char[,] TableOfGame { get; private set; }

        private char[,] InitialiseTableOfGame(int rows, int cols)
        {
            RandomGenerator rnd = new RandomGenerator();
            char[,] tableOfGame = new char[rows, cols];

            for (int indexRow = 0; indexRow < rows; indexRow++)
            {
                for (int indexColumn = 0; indexColumn < cols; indexColumn++)
                {
                    tableOfGame[indexRow, indexColumn] = GetRandomSymbol(rnd);
                }
            }

            return tableOfGame;
        }

        private char GetRandomSymbol(RandomGenerator randomGenerator)
        {
            int randomNumber = randomGenerator.GetRandomNumber(0, SYMBOLS.Length);
            char symbol = SYMBOLS[randomNumber];

            return symbol;
        }
    }
}