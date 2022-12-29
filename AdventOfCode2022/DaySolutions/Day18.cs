﻿using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022.DaySolutions
{
    class Day18 : DaySolver
    {
        public Day18(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var gridInfo = ParseCubes();
            return GetNumOfExposedSides(gridInfo.grid, gridInfo.cubes).ToString();
        }

        public override string GetPart2Solution()
        {
            var gridInfo = ParseCubes();
            return GetNumOfExternalExposedSides(gridInfo.grid, gridInfo.cubes).ToString();
        }

        private (List<List<List<int>>> grid, List<List<int>> cubes) ParseCubes ()
        {
            var rows = _rawInput.Split("\r\n");

            var allInts = new List<List<int>>();

            foreach(var row in rows)
            {
                var pieces = row.Split(",").Select(x => int.Parse(x)).ToList();
                allInts.Add(pieces);
            }

            var maxX = 0;
            var maxY = 0;
            var maxZ = 0;

            foreach(var cube in allInts)
            {
                if(cube[0] > maxX)
                {
                    maxX = cube[0];
                }

                if(cube[1] > maxY)
                {
                    maxY = cube[1];
                }

                if(cube[2] > maxZ)
                {
                    maxZ = cube[2];
                }
            }

            var grid = new List<List<List<int>>>();

            for(int i = 0; i <= maxX; i ++)
            {
                grid.Add(new List<List<int>>());
                for(int j = 0; j <= maxY; j++)
                {
                    grid[i].Add(new List<int>());
                    for ( int k = 0; k <= maxZ; k++)
                    {
                        grid[i][j].Add(0);
                    }
                }
            }

            foreach (var cube in allInts)
            {
                grid[cube[0]][cube[1]][cube[2]] = 1;
            }

            return (grid, allInts);
        }

        private int GetNumOfExposedSides(List<List<List<int>>> cubeGrid, List<List<int>> cubes)
        {
            //for each cube, for each of 6 directions, is the space empty, if so, add 1
            var totalEmptySides = 0;
            foreach(var cube in cubes)
            {
                //xdir
                if(cube[0] == 0 || (cube[0] > 0 && cubeGrid[cube[0] - 1][cube[1]][cube[2]] == 0)) // negative x
                {
                    totalEmptySides++;
                }
                if (cube[0] == cubeGrid.Count - 1 || (cube[0] < cubeGrid.Count - 1 && cubeGrid[cube[0] + 1][cube[1]][cube[2]] == 0)) // pos x
                {
                    totalEmptySides++;
                }

                //ydir
                if (cube[1] == 0 || (cube[1] > 0 && cubeGrid[cube[0]][cube[1] - 1][cube[2]] == 0)) // negative y
                {
                    totalEmptySides++;
                }
                if (cube[1] == cubeGrid[cube[0]].Count - 1 || (cube[1] < cubeGrid[cube[0]].Count - 1 && cubeGrid[cube[0]][cube[1] + 1][cube[2]] == 0)) // pos y
                {
                    totalEmptySides++;
                }

                //zdir
                if (cube[2] == 0 || (cube[2] > 0 && cubeGrid[cube[0]][cube[1]][cube[2] - 1] == 0)) // negative z
                {
                    totalEmptySides++;
                }
                if (cube[2] == cubeGrid[cube[0]][cube[1]].Count - 1 || (cube[2] < cubeGrid[cube[0]][cube[1]].Count - 1 && cubeGrid[cube[0]][cube[1]][cube[2] + 1] == 0)) // pos z
                {
                    totalEmptySides++;
                }
            }

            return totalEmptySides;
        }

        private int GetNumOfExternalExposedSides(List<List<List<int>>> cubeGrid, List<List<int>> cubes)
        {
            var numExternalSides = 0;

            /*
             * idea: 
             * look for each empty cube
             * recursively get touching empty cubes
             * get num of outer edges of that
             * remove from prev sum
             */
            var emptyCubes = new List<List<int>>();

            for(int i =0; i < cubeGrid.Count; i++)
            {
                for(int j = 0; j < cubeGrid[i].Count; j++)
                {
                    for(int k = 0; k < cubeGrid[i][j].Count; k++)
                    {
                        if(cubeGrid[i][j][k] == 0)
                        {
                            emptyCubes.Add(new List<int>() { i, j, k });
                        }
                    }
                }
            }

            return numExternalSides;
        }

    }
}
