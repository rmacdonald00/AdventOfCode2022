using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2022.DaySolutions
{
    abstract class DaySolver
    {
        protected readonly string _rawInput;
        public DaySolver(string input)
        {
            _rawInput = input;
        }

        public abstract string GetPart1Solution();

        public abstract string GetPart2Solution();
    }
}
