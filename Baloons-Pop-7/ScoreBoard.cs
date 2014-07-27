namespace BalloonsPops
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ScoreBoard
    {
        private static IDictionary<string, int> statistics = new Dictionary<string, int>();

        public static void AddPlayer(string name, int games)
        {
            if (statistics.ContainsKey(name))
            {
                if (games < statistics[name])
                {
                    statistics[name] = games;
                }
            }
            else
            {
                statistics.Add(name, games);
            }

            var sortedPlayers = from pair in statistics
                                orderby pair.Value ascending
                                select pair;

            statistics = sortedPlayers.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static void Print()
        {
            int points = 0;

            Console.WriteLine("Scoreboard:");

            foreach (KeyValuePair<string, int> s in statistics)
            {
                if (points == 4)
                {
                    break;
                }
                else
                {
                    points++;
                    Console.WriteLine("{0}. {1} --> {2} moves", points, s.Key, s.Value);
                }
            }
        }
    }
}
