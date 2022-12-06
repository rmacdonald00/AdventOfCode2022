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
            var charArr = _rawInput.ToCharArray();
            var i = numDistinctCharsInARowNeeded - 1;
            var recentChars = _rawInput.Substring(0, numDistinctCharsInARowNeeded).ToCharArray();
            while (i < charArr.Length)
            {
                recentChars[i % numDistinctCharsInARowNeeded] = charArr[i];
                if (!ContainsDuplicates(recentChars))
                {
                    return (i + 1).ToString();
                }
                i++;
            }
            return "oopsies, something wrong";
        }

        private bool ContainsDuplicates(char[] recentChars)
        {

            HashSet<char> charSet = new HashSet<char>(recentChars);
            return charSet.Count != recentChars.Length;
        }
    }
}
