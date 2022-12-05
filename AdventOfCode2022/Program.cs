using AdventOfCode2022.DaySolutions;
using System;

namespace AdventOfCode2022
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = GetSolver();
            Console.WriteLine($"Part 1 Solution: {solver.GetPart1Solution()}");
            Console.WriteLine($"Part 2 Solution: {solver.GetPart2Solution()}");
        }

        private static DaySolver GetSolver()
        {
            Console.WriteLine("What day are you solving?:");
            var day = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter T to use test input:");
            var useTestData = Console.ReadLine() == "T" ? true : false;

            return GetSolverByDay(day, useTestData);
        }

        private static string ReadInput(int day, Boolean testData)
        {
            var filepath = $"../../../Inputs/{(testData ? "Test" : "Full")}/Day{day}.txt";
            return System.IO.File.ReadAllText(filepath);
        }

        private static DaySolver GetSolverByDay(int day, bool useTestData)
        {
            var rawInputText = ReadInput(day, useTestData);

            // This is icky, I know, but I don't really care
            switch (day)
            {
                case 1: return new Day1(rawInputText);
                case 2: return new Day2(rawInputText);
                case 3: return new Day3(rawInputText);
                case 4: return new Day4(rawInputText);
                default: return null;
            }
        }
    }
}
