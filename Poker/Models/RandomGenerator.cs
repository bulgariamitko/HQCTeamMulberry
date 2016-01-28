namespace Poker.Models
{
    using System;

    using Poker.Interfaces;

    public class RandomGenerator : IRandomGenerator
    {
        private Random generator;

        public RandomGenerator()
        {
            this.generator = new Random();
        }

        public int RandomFromTo(int start, int end)
        {
            int result = this.generator.Next(start, end);

            return result;
        }
    }
}
