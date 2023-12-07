using System;

namespace Lab4
{
    public class CustomRandom : IRandom
    {
        public Random Random { get; private set; }

        public CustomRandom(Random random)
        {
            Random = random;
        }

        public int Next(int min, int max)
        {
            return Random.Next(min, max);
        }
    }
}
