namespace BalloonsPops
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ScoreBoard
    {
        //TODO:
        private static SortedDictionary<int, string> statistics = new SortedDictionary<int, string>();      

        public static void PrintAgain()
        {
            int points = 0;

            Console.WriteLine("Scoreboard:");

            foreach (KeyValuePair<int, string> s in statistics)
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
    }
}
