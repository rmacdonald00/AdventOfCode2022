using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2022.DaySolutions
{
    class Day4 : DaySolver
    {
        public Day4(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var elfPairs = ParseElfPairs();
            var countWithContsainment = elfPairs.Where(x => x.OneElfContainsOther()).ToList().Count;
            return countWithContsainment.ToString();
        }

        public override string GetPart2Solution()
        {
            var elfPairs = ParseElfPairs();
            var countWithOverlap = elfPairs.Where(x => x.AnyOverlap()).ToList().Count;
            return countWithOverlap.ToString();
        }

        private List<ElfPair> ParseElfPairs()
        {
            var rucksacks = _rawInput.Split("\r\n");

            return rucksacks.Select(x =>
            {
                var sides = x.Split(",");
                var elf1Range = sides[0].Split("-");
                var elf2Range = sides[1].Split("-");
                return new ElfPair(int.Parse(elf1Range[0]), int.Parse(elf1Range[1]), int.Parse(elf2Range[0]), int.Parse(elf2Range[1]));   
            }).ToList();
        }

        private class ElfPair
        {
            private readonly int _start1;
            private readonly int _end1;
            private readonly int _start2;
            private readonly int _end2;
            public ElfPair(int start1, int end1, int start2, int end2)
            {
                _start1 = start1;
                _end1 = end1;
                _start2 = start2;
                _end2 = end2;
            }

            public bool OneElfContainsOther()
            {
                var elf1Contains2 = _start1 <= _start2 && _end1 >= _end2;
                var elf2Contains1 = _start2 <= _start1 && _end2 >= _end1;
                return elf1Contains2 || elf2Contains1;
            }

            public bool AnyOverlap()
            {
                return OneElfContainsOther() || (_end1 >= _start2 && _end1 <= _end2) || (_start1 >= _start2 && _start1 <= _end2);
            }
        }
    }
}
