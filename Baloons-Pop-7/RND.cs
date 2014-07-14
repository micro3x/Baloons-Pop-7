namespace BalloonsPops
{
    using System;

    internal static class RND
    {
        private static Random randomGenerator = new Random();

        public static string GetRandomInt()
        {
            string legalChars = "@#$*";
            string builder = null;

            builder = legalChars[randomGenerator.Next(0, legalChars.Length)].ToString();

            return builder;
        }
    }
}
