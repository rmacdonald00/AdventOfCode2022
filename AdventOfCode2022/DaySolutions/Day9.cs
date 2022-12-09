using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022.DaySolutions
{
    class Day9 : DaySolver
    {
        public Day9(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            return GetNumPositionsTraversedByTail(2).ToString();
        }

        public override string GetPart2Solution()
        {
            return GetNumPositionsTraversedByTail(10).ToString();
        }

        private int GetNumPositionsTraversedByTail(int ropeSize)
        {
            List<(char direction, int amount)> moves = _rawInput.Split("\r\n").Select(x => {
                var pieces = x.Split(" ");
                return (pieces[0][0], int.Parse(pieces[1].ToString()));
            }).ToList();
            var placesFromOriginCovered = new HashSet<(int, int)>();
            placesFromOriginCovered.Add((0, 0)); //starting location

            List<(int index1, int index2)> ropePieces = new List<(int index1, int index2)>();
            for(int i = 0; i < ropeSize;  i++)
            {
                ropePieces.Add((0, 0));
            }
         

            foreach (var move in moves)
            {
                for (int i = 0; i < move.amount; i++)
                {
                    //move head
                    ropePieces[0] = MakeSingleMove(move.direction, ropePieces[0]);

                    for(int t = 1; t < ropeSize; t++)
                    {
                        //move tail
                        var tailMoves = GetTailMoves(ropePieces[t-1], ropePieces[t]);
                        foreach (var tailMove in tailMoves)
                        {
                            for (int j = 0; j < tailMove.amount; j++)
                            {
                                ropePieces[t] = MakeSingleMove(tailMove.direction, ropePieces[t]);
                            }
                        }
                        //add tail location to set
                    }
                    placesFromOriginCovered.Add((ropePieces[ropeSize - 1].index1, ropePieces[ropeSize - 1].index2));
                }
            }
            return placesFromOriginCovered.Count;
        }

        private (int index1, int index2) MakeSingleMove(char direction, (int index1, int index2) location)
        {
            switch (direction)
            {
                case 'R':
                    return (location.index1, location.index2+1);
                case 'L':
                    return (location.index1, location.index2 - 1);
                case 'U':
                    return (location.index1 - 1, location.index2);
                case 'D':
                    return (location.index1 + 1, location.index2);
            }
            return location;
        }

        private List<(char direction, int amount)> GetTailMoves((int index1, int index2) head, (int index1, int index2) tail)
        {
            if(head.index1 == tail.index1) // in straight line horiz
            {
                if(head.index2 > tail.index2 + 1) //move right
                {
                    return new List<(char direction, int amount)>() { ('R', 1) };
                } else if (head.index2 < tail.index2 - 1)// move left
                {
                    return new List<(char direction, int amount)>() { ('L', 1) };
                }
            } 
            else if (head.index2 == tail.index2) //vert
            {
                if (head.index1 > tail.index1 + 1) //move down
                {
                    return new List<(char direction, int amount)>() { ('D', 1) };
                }
                else if (head.index1 < tail.index1 - 1)// move up
                {
                    return new List<(char direction, int amount)>() { ('U', 1) };
                }
            }

            //else diagonal
            if(Math.Abs(head.index1 - tail.index1) < 2 && Math.Abs(head.index2 - tail.index2) < 2)
            {
                return new List<(char direction, int amount)>();
            }

            if(head.index1 > tail.index1 + 1) //head lower
            {
                return new List<(char direction, int amount)>()
                {
                    ('D', 1),
                    (head.index2 > tail.index2 ? 'R' : 'L', 1)
                };
            } 
            else if (head.index1 < tail.index1 + 1) //head higher
            {
                return new List<(char direction, int amount)>()
                {
                    ('U', 1),
                    (head.index2 > tail.index2 ? 'R' : 'L', 1)
                };
            }
            else if (head.index2 > tail.index2 + 1) //head right
            {
                return new List<(char direction, int amount)>()
                {
                    ('R', 1),
                    (head.index1 > tail.index1 ? 'D' : 'U', 1)
                };
            }
            else if (head.index2 < tail.index2 + 1) //head left
            {
                return new List<(char direction, int amount)>()
                {
                    ('L', 1),
                    (head.index1 > tail.index1 ? 'D' : 'U', 1)
                };
            }
            return new List<(char direction, int amount)>();
        }
    }
}
