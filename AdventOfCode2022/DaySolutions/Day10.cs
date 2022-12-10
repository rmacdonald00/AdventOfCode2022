using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022.DaySolutions
{
    class Day10 : DaySolver
    {
        public Day10(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            return GetSumOfLoopsCaredAbout(new int[] { 20, 60, 100, 140, 180, 220}).ToString();
        }

        public override string GetPart2Solution()
        {
            return GetCRTImage(new int[] { 20, 60, 100, 140, 180, 220 });
        }

        private int GetSumOfLoopsCaredAbout(int[] importantCycles)
        {
            List<(string changeType, int amount)> changes = _rawInput.Split("\r\n").Select(x => {
                var pieces = x.Split(" ");
            int piece2 = pieces.Length > 1 ? int.Parse(pieces[1]) : 0;
                return (pieces[0], piece2);
            }).ToList();

            var cycleNumber = 0;
            List<(int cycle, int xVal)> totalOfSignalStrengthsAtSpecialIndexes = new List<(int cycle, int xVal)>();
            var x = 1;
            foreach (var change in changes)
            {
                if(change.changeType == "noop")
                {
                    cycleNumber++;
                    if (importantCycles.Contains(cycleNumber))
                    {
                        totalOfSignalStrengthsAtSpecialIndexes.Add((x, cycleNumber));
                    }
                } else if (change.changeType == "addx")
                {
                    cycleNumber++;
                    if (importantCycles.Contains(cycleNumber))
                    {
                        totalOfSignalStrengthsAtSpecialIndexes.Add((x, cycleNumber));
                    }
                    cycleNumber++;
                    if (importantCycles.Contains(cycleNumber))
                    {
                        totalOfSignalStrengthsAtSpecialIndexes.Add((x, cycleNumber));
                    }
                    x += change.amount;
                }
            }
            return totalOfSignalStrengthsAtSpecialIndexes.Select(x => x.cycle * x.xVal).Sum();
        }

        private string GetCRTImage(int[] importantCycles)
        {
            List<(string changeType, int amount)> changes = _rawInput.Split("\r\n").Select(x => {
                var pieces = x.Split(" ");
                int piece2 = pieces.Length > 1 ? int.Parse(pieces[1]) : 0;
                return (pieces[0], piece2);
            }).ToList();

            var cycleNumber = -1;
            List<char[]> totalOfSignalStrengthsAtSpecialIndexes = new List<char[]>() {
                "........................................".ToCharArray(),
                "........................................".ToCharArray(),
                "........................................".ToCharArray(),
                "........................................".ToCharArray(),
                "........................................".ToCharArray(),
                "........................................".ToCharArray(),
            };
            var x = 1;
            foreach (var change in changes)
            {   
                if (change.changeType == "noop")
                {
                    cycleNumber++;
                    if (cycleNumber % 40 >= x - 1 && cycleNumber % 40 <= x + 1)
                    {
                        totalOfSignalStrengthsAtSpecialIndexes[cycleNumber / 40][cycleNumber % 40] = '#';
                    }
                }
                else if (change.changeType == "addx")
                {
                    cycleNumber++;
                    if (cycleNumber % 40 >= x - 1 && cycleNumber % 40 <= x + 1)
                    {
                        totalOfSignalStrengthsAtSpecialIndexes[cycleNumber / 40][cycleNumber % 40] = '#';
                    }
                    cycleNumber++;
                    if (cycleNumber % 40 >= x - 1 && cycleNumber % 40 <= x + 1)
                    {
                        totalOfSignalStrengthsAtSpecialIndexes[cycleNumber / 40][cycleNumber % 40] = '#';
                    }
                    x += change.amount;
                }
            }
            var fullString = "\n";
            foreach(var row in totalOfSignalStrengthsAtSpecialIndexes)
            {
                fullString += new string(row) + "\n";
            }
            return fullString;
        }
    }
}
