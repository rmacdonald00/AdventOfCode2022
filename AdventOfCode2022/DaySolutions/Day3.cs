using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022.DaySolutions
{
    class Day3 : DaySolver
    {
        public Day3(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var rucksacks = ParseRucksacks();
            var values = rucksacks.Select(x => x.GetPriorityOfSharedCharacter()).ToList();
            var sum = values.Sum();
            return sum.ToString();
        }

        public override string GetPart2Solution()
        {
            var rucksacks = ParseRucksackGroupsPart2();
            var values = rucksacks.Select(x => x.GetPriorityOfSharedCharacter()).ToList();
            var sum = values.Sum();
            return sum.ToString();
        }

        private List<StringCompareRucksackyBoi> ParseRucksacks()
        {
            var rucksacks = _rawInput.Split("\r\n");

            return rucksacks.Select(x =>
            {
                var halfLength = x.Length / 2;
                var firstHalf = x.Substring(0, halfLength).ToCharArray();
                var secondHalf = x.Substring(halfLength).ToCharArray();
                return new StringCompareRucksackyBoi(new List<char[]>() { firstHalf, secondHalf });   
            }).ToList();
        }

        private List<StringCompareRucksackyBoi> ParseRucksackGroupsPart2()
        {
            var rucksacks = _rawInput.Split("\r\n");

            var groups = new List<StringCompareRucksackyBoi>();
            for (var i = 0; i < rucksacks.Length; i += 3)
            {
                groups.Add(new StringCompareRucksackyBoi(new List<char[]>() { rucksacks[i].ToCharArray(), rucksacks[i+1].ToCharArray(), rucksacks[i+2].ToCharArray() }));
            }
            return groups;
        }

        private class StringCompareRucksackyBoi
        {
            private readonly List<Char[]> _stringsToFindSharedBetween;
            public StringCompareRucksackyBoi(List<Char[]> stringsToCompare)
            {
                _stringsToFindSharedBetween = stringsToCompare;
            }

            private char GetSharedLetter()
            {
                if(_stringsToFindSharedBetween.Count < 3)
                {
                    foreach (var character in _stringsToFindSharedBetween[0])
                    {
                        if (_stringsToFindSharedBetween[1].Contains(character))
                        {
                            return character;
                        }
                    }
                } else
                {
                    foreach (var character in _stringsToFindSharedBetween[0])
                    {
                        if (_stringsToFindSharedBetween[1].Contains(character) && _stringsToFindSharedBetween[2].Contains(character))
                        {
                            return character;
                        }
                    }
                }
                return '0';
            }

            public int GetPriorityOfSharedCharacter()
            {
                int character = GetSharedLetter();

                if (character > 96) //lowercase
                {
                    return character - 96;
                }
                return character - 38;
            }
        }
    }
}
