using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2022.DaySolutions
{
    class Day1: DaySolver
    {
        public Day1(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var elves = ParseElves();
            var maxTotalCalories = elves.Select(x => x.GetTotalCaloriesHolding()).OrderByDescending(x => x).ToList().First();
            return maxTotalCalories.ToString();
        }

        public override string GetPart2Solution()
        {
            var elves = ParseElves();
            var totalCaloriesSorted = elves.Select(x => x.GetTotalCaloriesHolding()).OrderByDescending(x => x).ToList();
            return (totalCaloriesSorted[0] + totalCaloriesSorted[1] + totalCaloriesSorted[2]).ToString();
        }

        private List<ElfWithFood> ParseElves()
        {
            var elfSections = _rawInput.Split("\r\n\r\n");
            return elfSections.Select(x =>
            {
                var foodAmounts = x.Split("\r\n").Select(y => int.Parse(y)).ToList();
                return new ElfWithFood(foodAmounts);
            }).ToList();
        }

        private class ElfWithFood
        {
            private readonly List<int> _foodValues;
            public ElfWithFood(List<int> foodValues)
            {
                _foodValues = foodValues;
            }

            public int GetTotalCaloriesHolding()
            {
                return _foodValues.Sum();
            }
        }
    }
}
