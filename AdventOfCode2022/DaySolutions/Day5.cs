using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2022.DaySolutions
{
    class Day5 : DaySolver
    {
        public Day5(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var stackData = ParseMovesAndStack();
            return stackData.GetTopOfStackString(false);
        }

        public override string GetPart2Solution()
        {
            var stackData = ParseMovesAndStack();
            return stackData.GetTopOfStackString(true);
        }

        private CrateStacks ParseMovesAndStack()
        {
            var twoPieces = _rawInput.Split("\r\n\r\n");
            var stackInput = twoPieces[0];
            var movesText = twoPieces[1];

            //parse moves
            var allMoveTexts = movesText.Split("\r\n");
            var moves = new List<Move>();
            foreach(var moveString in allMoveTexts)
            {
                var pieces = moveString.Split(" ");
                moves.Add(new Move(int.Parse(pieces[1]), int.Parse(pieces[3]), int.Parse(pieces[5])));
            }

            //parse starting crate stacks
            var stackStrings = stackInput.Split("\r\n");
            int numStacks = int.Parse(stackStrings.Last().Trim().ToCharArray().Last().ToString());
            var stackRowArrays = stackStrings.Select(x => x.ToCharArray()).ToList();
            var allStacks = new List<Stack<char>>();
            for(var i = 0; i < numStacks; i++)
            {
                var newStack = new Stack<char>();
                for (var j = stackStrings.Length - 2; j >= 0; j--)
                {
                    var label = stackRowArrays[j][i * 4 + 1];
                    if(label != 32)
                    {
                        newStack.Push(label);
                    }
                }
                allStacks.Add(newStack);
            }
            return new CrateStacks(allStacks, moves);
        }

        private class Move
        {
            public readonly int _amountToMove;
            public readonly int _sourceStackIndexAdjusted;
            public readonly int _destinationStackIndexAdjusted;

            public Move(int amountToMove, int sourceStack, int destinationStack)
            {
                _amountToMove = amountToMove;
                _sourceStackIndexAdjusted = sourceStack - 1;
                _destinationStackIndexAdjusted = destinationStack - 1;
            }
        }
        private class CrateStacks
        {
            private List<Stack<char>> _stacks;
            private List<Move> _moves;
            public CrateStacks(List<Stack<char>> stacks, List<Move> moves)
            {
                _stacks = stacks;
                _moves = moves;
            }

            private void ExecuteMovesOneByOne()
            {
                foreach(var move in _moves)
                {
                    for(var i = 0; i < move._amountToMove; i++)
                    {
                        _stacks[move._destinationStackIndexAdjusted].Push(_stacks[move._sourceStackIndexAdjusted].Pop());
                    }
                }
            }

            private void ExecuteMovesInChunks()
            {
                foreach (var move in _moves)
                {
                    var tempStack = new Stack<char>();
                    for (var i = 0; i < move._amountToMove; i++)
                    {
                        tempStack.Push(_stacks[move._sourceStackIndexAdjusted].Pop());
                    }
                    for (var i = 0; i < move._amountToMove; i++)
                    {
                        _stacks[move._destinationStackIndexAdjusted].Push(tempStack.Pop());
                    }
                }
            }

            public string GetTopOfStackString(bool inChunks)
            {
                if (inChunks)
                {
                    ExecuteMovesInChunks();
                } else
                {
                    ExecuteMovesOneByOne();
                }

                var sb = new StringBuilder();
                foreach(var stack in _stacks)
                {
                    sb.Append(stack.First());
                }
                return sb.ToString();
            }    
        }
    }
}
