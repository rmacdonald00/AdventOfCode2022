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
                case 5: return new Day5(rawInputText);
                case 6: return new Day6(rawInputText);
                case 7: return new Day7(rawInputText);
                case 8: return new Day8(rawInputText);
                case 9: return new Day9(rawInputText);
                case 10: return new Day10(rawInputText);
                case 11: return new Day11(rawInputText);
                case 12: return new Day12(rawInputText);
                case 13: return new Day13(rawInputText);
                case 14: return new Day14(rawInputText);
                case 15: return new Day15(rawInputText);
                case 16: return new Day16(rawInputText);
                case 17: return new Day17(rawInputText);
                case 18: return new Day18(rawInputText);
                default: return null;
            }
        }
    }
}
