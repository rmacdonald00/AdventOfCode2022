using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.DaySolutions
{
    class Day14 : DaySolver
    {
        public Day14(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var map = BuildMapOfRocks();
            return GetNumSandThatRests(map, 500, 0).ToString();
        }

        public override string GetPart2Solution()
        {
            var map = BuildMapOfRocksPart2();
            return GetNumSandThatRestsPart2(map, 500, 0).ToString();
        }

        private List<List<char>> BuildMapOfRocks() //good luck to me... flipping directions
        {
            var rockPaths = _rawInput.Split("\r\n");
            var maxX = Regex.Matches(_rawInput, @"[\d]+,[\d]+").Select(x => int.Parse(x.Value.Split(",")[0])).Max();
            var maxY = Regex.Matches(_rawInput, @"[\d]+,[\d]+").Select(x => int.Parse(x.Value.Split(",")[1])).Max();
            var map = new List<List<char>>();
            for(int i = 0; i < maxX + 1; i ++)
            {
                map.Add(new List<char>());
                for(int j = 0; j < maxY + 1; j++)
                {
                    map[i].Add('.');
                }
            }

            foreach(var rock in rockPaths)
            {
                List<(int x, int y)> points = Regex.Matches(rock, @"[\d]+,[\d]+").Select(x => (int.Parse(x.Value.Split(",")[0]), int.Parse(x.Value.Split(",")[1]))).ToList();
                for(int i = 0; i < points.Count - 1; i++)
                {
                    if(points[i].x == points[i + 1].x) //vert line
                    {
                        var minHeight = Math.Min(points[i].y, points[i + 1].y);
                        var length = Math.Abs(points[i].y - points[i + 1].y);
                        for(int j = minHeight; j <= minHeight + length; j++)
                        {
                            map[points[i].x][j] = '#';
                        }
                    }
                    else //horiz line
                    {
                        var minWidth = Math.Min(points[i].x, points[i + 1].x);
                        var length = Math.Abs(points[i].x - points[i + 1].x);
                        for (int j = minWidth; j <= minWidth + length; j++)
                        {
                            map[j][points[i].y] = '#';
                        }
                    }
                }
            }

            return map;
        }

        private int GetNumSandThatRests(List<List<char>> map, int startingX, int startingY)
        {
            var numSandDropped = 0;

            var lastSandRested = true;
            while(lastSandRested)
            {
                // see if sand rests
                var nextLocation = GetSandLocation(map, startingX, startingY);
                if(nextLocation.x == -1 || nextLocation.y == -1)
                {
                    lastSandRested = false;
                } else
                {
                    numSandDropped++;
                    map[nextLocation.x][nextLocation.y] = 'o';
                }
            }

            return numSandDropped;
        }


        private List<List<char>> BuildMapOfRocksPart2() //good luck to me... flipping directions
        {
            var rockPaths = _rawInput.Split("\r\n");
            var maxX = Regex.Matches(_rawInput, @"[\d]+,[\d]+").Select(x => int.Parse(x.Value.Split(",")[0])).Max();
            var maxY = Regex.Matches(_rawInput, @"[\d]+,[\d]+").Select(x => int.Parse(x.Value.Split(",")[1])).Max();
            var map = new List<List<char>>();
            for (int i = 0; i < maxX + 1; i++)
            {
                map.Add(new List<char>());
                for (int j = 0; j < maxY + 1; j++)
                {
                    map[i].Add('.');
                }
                map[i].Add('.');
                map[i].Add('#');
            }

            foreach (var rock in rockPaths)
            {
                List<(int x, int y)> points = Regex.Matches(rock, @"[\d]+,[\d]+").Select(x => (int.Parse(x.Value.Split(",")[0]), int.Parse(x.Value.Split(",")[1]))).ToList();
                for (int i = 0; i < points.Count - 1; i++)
                {
                    if (points[i].x == points[i + 1].x) //vert line
                    {
                        var minHeight = Math.Min(points[i].y, points[i + 1].y);
                        var length = Math.Abs(points[i].y - points[i + 1].y);
                        for (int j = minHeight; j <= minHeight + length; j++)
                        {
                            map[points[i].x][j] = '#';
                        }
                    }
                    else //horiz line
                    {
                        var minWidth = Math.Min(points[i].x, points[i + 1].x);
                        var length = Math.Abs(points[i].x - points[i + 1].x);
                        for (int j = minWidth; j <= minWidth + length; j++)
                        {
                            map[j][points[i].y] = '#';
                        }
                    }
                }
            }

            return map;
        }

        private int GetNumSandThatRestsPart2(List<List<char>> map, int startingX, int startingY)
        {
            var numSandDropped = 0;

            var lastSandRested = true;
            while (lastSandRested)
            {
                Console.WriteLine($"{numSandDropped}-----------------------------------");
                PrintMap(map);
                // see if sand rests
                var nextLocation = GetSandLocation(map, startingX, startingY);
                if (nextLocation.x == -1 || nextLocation.y == -1)
                {
                    lastSandRested = false;
                }
                else
                {
                    numSandDropped++;
                    map[nextLocation.x][nextLocation.y] = 'o';
                }
            }

            return numSandDropped;
        }

        private (int x, int y) GetSandLocation(List<List<char>> map, int startingX, int startingY)
        {
            var currentX = startingX;
            var currentY = startingY;
            //var i = 0;
            //One check
            while (true)
            {
                //i++;
                //Console.WriteLine($"{i}-----------------------------------");
                //PrintMap(map);
                if(OkayIndex(map, currentX, currentY+1) && map[currentX][currentY + 1] == '.') //move down
                {
                    currentY++;
                    continue;
                } else if (OkayIndex(map, currentX - 1, currentY + 1) && map[currentX - 1][currentY + 1] == '.') //move down/left
                {
                    currentX--;
                    currentY++;
                    continue;
                }
                else if (OkayIndex(map, currentX + 1, currentY + 1) && map[currentX + 1][currentY + 1] == '.') //move down/right 
                {
                    currentX++;
                    currentY++;
                    continue;
                }
                else if (!OkayIndex(map, currentX, currentY + 1) && !OkayIndex(map, currentX - 1, currentY + 1) && !OkayIndex(map, currentX + 1, currentY + 1))
                {
                    return (-1, -1);
                }
                else if(OkayIndex(map, currentX, currentY) && currentX != startingX || currentY != startingY)
                {
                    return (currentX, currentY);
                } else
                {
                    return (-1, -1);
                }
            }
            //
        }

        private bool OkayIndex(List<List<char>> map, int startingX, int startingY)
        {
            return startingX >= 0 && startingY >= 0 && startingX < map.Count && startingY < map[startingX].Count;
        }

        private void PrintMap(List<List<char>> map)
        {
            for(int i = 490; i < map.Count; i++)
            {
                var str = (new string(map[i].ToArray()));
                Console.WriteLine(str);
            }
        }
    }
}
