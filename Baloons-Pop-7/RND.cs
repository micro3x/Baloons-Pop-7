namespace BalloonsPops
{
    using System;

    public static class RND
    {
        static Random randomGenerator = new Random();

        public static string GetRandomInt()
        {
            string legalChars = "1234";
            string builder = null;

            builder = legalChars[randomGenerator.Next(0, legalChars.Length)].ToString();

            return builder;
        }
    }
}
