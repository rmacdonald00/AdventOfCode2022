using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2022.DaySolutions
{
    class Day6 : DaySolver
    {
        public Day6(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            return SolveBySize(4);
        }

        public override string GetPart2Solution()
        {
            return SolveBySize(14);
        }

        private string SolveBySize(int numDistinctCharsInARowNeeded)
        {
            var i = numDistinctCharsInARowNeeded;
            while (i < _rawInput.Length)
            {
                HashSet<char> charSet = new HashSet<char>(_rawInput.Substring(i - numDistinctCharsInARowNeeded, numDistinctCharsInARowNeeded).ToCharArray());
                if (charSet.Count == numDistinctCharsInARowNeeded)
                {
                    return (i).ToString();
                }
                i++;
            }
            return "oopsies, something wrong";
        }
    }
}
