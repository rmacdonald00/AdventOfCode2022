using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022.DaySolutions
{
    class Day15 : DaySolver
    {
        public Day15(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var sensors = ParseSensors();
            var setOfEmptyLocs = new HashSet<(int x, int y)>();

            foreach(var sensor in sensors)
            {
                var emptyLocs = sensor.GetEmptyLocations(2000000);
                foreach(var loc in emptyLocs)
                {
                    setOfEmptyLocs.Add(loc);
                }
            }


            return setOfEmptyLocs.Count.ToString();
        }

        public override string GetPart2Solution()
        {
            var sensors = ParseSensors();
            //sensors.Sort((x, y) => y._manhattandist - x._manhattandist);
            int minVal = 0;
            int maxVal = 4000000;

            var perimeterVals = new HashSet<(int x, int y)>();
            foreach (var sensor in sensors)
            {
                var perimeterLocs = sensor.GetPerimeterLocations();
                foreach (var loc in perimeterLocs)
                {
                    if(loc.x >= minVal && loc.x <= maxVal && loc.y >= minVal && loc.y <= maxVal)
                    {
                        perimeterVals.Add(loc);
                    }
                }
            }

            var perimList = perimeterVals.ToList();
            for (int i = 0; i < perimList.Count; i++) //made it to 3000 w/o answer //103400 @ 6pm
            {
                //for (int j = 0; j < 4000000; j++)
                //{

                var emptyFound = false;
                foreach (var sensor in sensors)
                {
                    if (sensor.LocationIsEmptyDueToMe(perimList[i].x, perimList[i].y))
                    {
                        emptyFound = true;
                        break;
                    }
                }
                if (!emptyFound)
                {
                    var item = perimList[i];
                    return ($"{item.x} * 4000000 + {item.y}");
                }
                //}
            }

            return "";
        }

        private List<Sensor> ParseSensors()
        {
            var sensors = new List<Sensor>();
            var rows = _rawInput.Split("\r\n");
            foreach(var row in rows)
            {
                var xMatches = Regex.Matches(row, @"x=-?[\d]+");
                var yMatches = Regex.Matches(row, @"y=-?[\d]+");
                var sensorXString = xMatches[0].Value;
                var beaconXString = xMatches[1].Value;
                var sensorYString = yMatches[0].Value;
                var beaconYString = yMatches[1].Value;

                sensors.Add(new Sensor(
                    int.Parse(sensorXString.Split("=")[1]),
                    int.Parse(sensorYString.Split("=")[1]),
                    int.Parse(beaconXString.Split("=")[1]),
                    int.Parse(beaconYString.Split("=")[1])));
            }
            return sensors;
        }

        public class Sensor
        {
            public readonly int _xLocation;
            public readonly int _yLocation;
            public readonly int _beaconXLocation;
            public readonly int _beaconYLocation;
            public readonly int _manhattandist;

            public Sensor(int myX, int myY, int bX, int bY)
            {
                _xLocation = myX;
                _yLocation = myY;
                _beaconXLocation = bX;
                _beaconYLocation = bY;
                _manhattandist = Math.Abs(_xLocation - _beaconXLocation) + Math.Abs(_yLocation - _beaconYLocation);
            }

            public HashSet<(int x, int y)> GetEmptyLocations(int yRowToCheck)
            {
                var locationsWithoutBeacon = new HashSet<(int x, int y)>();

                int yDist = Math.Abs(_yLocation - yRowToCheck);
                for(int i = _xLocation - _manhattandist; i < _xLocation + _manhattandist; i++)
                {

                    var manhattanDistToLocation = Math.Abs(_xLocation - i) + yDist;
                        if(manhattanDistToLocation <= _manhattandist)
                        {
                            locationsWithoutBeacon.Add((i, yRowToCheck));
                        }
                }
                if (_beaconYLocation == yRowToCheck)
                {

                    locationsWithoutBeacon.RemoveWhere(((int x, int y) item) => item.x == _beaconXLocation && item.y == _beaconYLocation);
                }
                return locationsWithoutBeacon;
            }

            public HashSet<(int x, int y)> GetPerimeterLocations()
            {
                var perimeterLocations = new HashSet<(int x, int y)>();
               
                for(var i = 0; i <= _manhattandist; i++)
                {
                    var diff = _manhattandist - i + 1;
                    perimeterLocations.Add((_xLocation - i, _yLocation - diff));
                    perimeterLocations.Add((_xLocation + i, _yLocation + diff));
                    perimeterLocations.Add((_xLocation + diff, _yLocation + i));
                    perimeterLocations.Add((_xLocation - diff, _yLocation - i));
                }

                return perimeterLocations;
            }

            public bool LocationIsEmptyDueToMe(int xLoc, int yLoc)
            {
                var distToSensor = Math.Abs(_xLocation - xLoc) + Math.Abs(_yLocation - yLoc);
                return distToSensor <= _manhattandist;
            }

            public (int minX, int minY, int maxX, int maxY) GetGridToCheck(int min, int max) {


                int minx = _xLocation - _manhattandist;
                int miny = _yLocation - _manhattandist;
                int maxx = _xLocation + _manhattandist;
                int maxy = _yLocation + _manhattandist;
                return (Math.Max(minx, min), Math.Max(min, miny), Math.Min(max, maxx), Math.Min(max, maxy));
            }

        }
    }
}
