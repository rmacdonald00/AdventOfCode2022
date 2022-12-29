using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2022.DaySolutions
{
    class Day17 : DaySolver
    {
        public Day17(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var chamberWidth = 7;
            var numRocksToFall = 2022;
            //var numRocksToFall = 200;

            var rockTypeOrder = new RockType[5] { RockType.HorizLine, RockType.Plus, RockType.L, RockType.VertLine, RockType.Square };
            var rockTypeLocation = 0;

            var chamber = new Chamber(chamberWidth, _rawInput);

            for (var i = 0; i < numRocksToFall; i++)
            {
                var rockType = rockTypeOrder[rockTypeLocation % rockTypeOrder.Length];
                rockTypeLocation++;
                chamber.AddRock(rockType);
            }

            var fullRows = chamber.GetFullRows();

            return chamber.GetHeight().ToString();
        }

        public override string GetPart2Solution()
        {
            //var chamberWidth = 7;
            //var numRocksToFall = 1000000000000;

            //var rockTypeOrder = new RockType[5] { RockType.HorizLine, RockType.Plus, RockType.L, RockType.VertLine, RockType.Square };
            //var rockTypeLocation = 0;

            //var chamber = new Chamber(chamberWidth, _rawInput);

            //for (var i = 0; i < numRocksToFall; i++)
            //{
            //    var rockType = rockTypeOrder[rockTypeLocation % rockTypeOrder.Length];
            //    rockTypeLocation++;
            //    chamber.AddRock(rockType);
            //}

            //return chamber.GetHeight().ToString();

            return "";
        }

        public enum RockType
        {
            HorizLine,
            Plus,
            L,
            VertLine,
            Square
        }

        public class Chamber
        {
            private readonly List<List<int>> _chamber = new List<List<int>>();
            private const int _numEmptyRowsBetweenNewDropped = 3;
            private readonly string _jetsOrder;
            private int _jetsLocation;
            public Chamber(int chamberWidth, string jetsOrder)
            {
                for (int i = 0; i < chamberWidth; i++)
                {
                    _chamber.Add(new List<int>());
                    //for (int j = 0; j < _numEmptyRowsBetweenNewDropped; j++)
                    //{
                    //    _chamber[i].Add(0);
                    //}
                }

                _jetsOrder = jetsOrder;
                _jetsLocation = 0;
            }

            public int GetHeight()
            {
                var heights = _chamber.Select(x => Math.Max(x.FindLastIndex(y => y == 1) + 1, 0)).ToList();
                return heights.Max();
            }

            public void AddRock(RockType type)
            {
                var rockLocationInfo = GetRockStartingLocations(type);
                var rockLocations = rockLocationInfo.locations;

                for (int i = GetHeight(); i < GetHeight() + _numEmptyRowsBetweenNewDropped + rockLocationInfo.height; i++)
                {
                    foreach (var column in _chamber)
                    {
                        column.Add(0);
                    }
                }

                var rockStillFalling = true;

                while (rockStillFalling)
                {
                    var nextJetShift = _jetsOrder.ElementAt(_jetsLocation % _jetsOrder.Count());

                    HashSet<(int xi, int yj)> jetShiftedRocks;

                    if (nextJetShift == '>')
                    {
                        jetShiftedRocks = ShiftRight(rockLocations);
                    }
                    else
                    {
                        jetShiftedRocks = ShiftLeft(rockLocations);
                    }
                    _jetsLocation++;

                    if (IsRockInBounds(jetShiftedRocks))
                    {
                        rockLocations = jetShiftedRocks;
                    }

                    jetShiftedRocks = ShiftDown(rockLocations);
                    if (IsRockInBounds(jetShiftedRocks))
                    {
                        rockLocations = jetShiftedRocks;
                    }
                    else
                    {
                        SetRockDown(rockLocations, rockLocationInfo.height);
                        rockStillFalling = false;
                    }
                }
            }

            private void SetRockDown(HashSet<(int xi, int yj)> locations, int height)
            {
                foreach (var location in locations)
                {
                    _chamber[location.xi][location.yj] = 1;
                }
            }

            private bool IsRockInBounds(HashSet<(int xi, int yj)> locations)
            {
                var anyOverWall = locations.Any(((int xi, int yi) loc) => loc.xi < 0 || loc.xi >= _chamber.Count || loc.yi < 0);

                if (anyOverWall)
                {
                    return false;
                }

                foreach (var location in locations)
                {
                    if (_chamber[location.xi][location.yj] == 1)
                    {
                        return false;
                    }
                }

                return true;
            }

            private HashSet<(int xi, int yj)> ShiftRight(HashSet<(int xi, int yj)> locations)
            {
                return locations.Select(((int xi, int yi) loc) => (loc.xi + 1, loc.yi)).ToHashSet();
            }
            private HashSet<(int xi, int yj)> ShiftLeft(HashSet<(int xi, int yj)> locations)
            {
                return locations.Select(((int xi, int yi) loc) => (loc.xi - 1, loc.yi)).ToHashSet();
            }

            private HashSet<(int xi, int yj)> ShiftDown(HashSet<(int xi, int yj)> locations)
            {
                return locations.Select(((int xi, int yi) loc) => (loc.xi, loc.yi - 1)).ToHashSet();
            }


            private (int height, HashSet<(int xi, int yj)> locations) GetRockStartingLocations(RockType type)
            {
                var startingBottomHeightOfRock = GetHeight() + _numEmptyRowsBetweenNewDropped;

                switch (type)
                {
                    case RockType.HorizLine:
                        return (1, new HashSet<(int xi, int yj)>() { (2, startingBottomHeightOfRock), (3, startingBottomHeightOfRock), (4, startingBottomHeightOfRock), (5, startingBottomHeightOfRock) });
                    case RockType.Plus:
                        return (3, new HashSet<(int xi, int yj)>() { (2, startingBottomHeightOfRock + 1), (3, startingBottomHeightOfRock + 1), (4, startingBottomHeightOfRock + 1), (3, startingBottomHeightOfRock), (3, startingBottomHeightOfRock + 2) });
                    case RockType.L:
                        return (3, new HashSet<(int xi, int yj)>() { (2, startingBottomHeightOfRock), (3, startingBottomHeightOfRock), (4, startingBottomHeightOfRock), (4, startingBottomHeightOfRock + 1), (4, startingBottomHeightOfRock + 2) });
                    case RockType.VertLine:
                        return (4, new HashSet<(int xi, int yj)>() { (2, startingBottomHeightOfRock), (2, startingBottomHeightOfRock + 1), (2, startingBottomHeightOfRock + 2), (2, startingBottomHeightOfRock + 3) });
                    case RockType.Square:
                        return (2, new HashSet<(int xi, int yj)>() { (2, startingBottomHeightOfRock), (3, startingBottomHeightOfRock), (2, startingBottomHeightOfRock + 1), (3, startingBottomHeightOfRock + 1) });
                    default:
                        return (0, new HashSet<(int xi, int yj)>());
                }
            }

            public List<int> GetFullRows()
            {
                var fullRows = new List<int>();
                for (int j = 0; j < GetHeight(); j++)
                {
                    var hasZero = false;
                    for (int i = 0; i < _chamber.Count; i++)
                    {
                        if (_chamber[i][j] == 0)
                        {
                            hasZero = true;
                        }
                    }
                    if (!hasZero)
                    {
                        fullRows.Add(j);
                    }
                }

                return fullRows;
            }
        }
    }
}
