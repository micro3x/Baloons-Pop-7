namespace BalloonsPops
{
    using System;

    public sealed class DrawFieldInformation
    {
        private static readonly DrawFieldInformation drawer = new DrawFieldInformation();

        public static DrawFieldInformation Instance
        {
            get
            {
                return drawer;
            }
        }

        public void DrawGameField(char[,] tableOfGame)
        {
            Console.WriteLine("    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("   ---------------------");

            for (int indexRow = 0; indexRow < tableOfGame.GetLength(0); indexRow++)
            {
                Console.Write(indexRow + " | ");

                for (int indexColumn = 0; indexColumn < tableOfGame.GetLength(1); indexColumn++)
                {
                    char symbol = tableOfGame[indexRow, indexColumn];

                    if (symbol == '1') Console.BackgroundColor = ConsoleColor.Red;
                    else if (symbol == '2') Console.BackgroundColor = ConsoleColor.Green;
                    else if (symbol == '3') Console.BackgroundColor = ConsoleColor.Blue;
                    else if (symbol == '4') Console.BackgroundColor = ConsoleColor.Yellow;
                    else Console.ResetColor();

                    Console.Write(symbol + " ");
                    Console.ResetColor();
                }

                Console.Write("| ");
                Console.WriteLine();
            }

            Console.WriteLine("   ---------------------");
        }

        public void DrawWelcomeMessage()
        {
            Console.WriteLine("Welcome to “Balloons Pops” game. Please try to pop the balloons." +
                " Use 'top' to view the top scoreboard, 'restart' to start a new game and 'exit' to quit the game.");
        }
    }
}
