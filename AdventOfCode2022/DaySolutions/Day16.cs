using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2022.DaySolutions
{
    class Day16 : DaySolver
    {
        public Day16(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var valves = ParseValves();

            var matrixInfo = CreateAdjMatrix();
            var count = matrixInfo.matrix.Count;

            var idsThatStartZero = valves.Where(x => x._flowRate == 0 && x._id != "AA").Select(x => x._id).ToList();

            foreach (var id in idsThatStartZero)
            {
                FlattenIndex(matrixInfo.matrix, matrixInfo.indexMap[id]);
            }

            FlloydWarshall(matrixInfo.matrix, 30);



            var matrix = matrixInfo.matrix;
            var indexMap = matrixInfo.indexMap;
            var currentValveId = "AA";

            //var totalReleased = 0;

            //var timeRemaining = 30;
            //while(timeRemaining > 0)
            //{
            //    Console.WriteLine($"on valve {currentValveId}");
            //    var currentValve = valves.First(x => x._id == currentValveId);
            //    if(!currentValve._open && currentValve._flowRate != 0)
            //    {
            //        timeRemaining--;
            //        totalReleased += (currentValve._flowRate * timeRemaining);
            //        currentValve._open = true;
            //        currentValve._flowRate = 0; //revisit if this is ok spot
            //    } 
            //    //foreach other valve, divide A's row by FR, then pick max as next
            //    var newList = new List<float>();

            //    foreach( var value in matrix[indexMap[currentValveId]])
            //    {
            //        newList.Add(value);
            //    }


            //    foreach (var valve in valves)
            //    {
            //        if(valve._id != currentValveId)
            //        {
            //            if(newList[indexMap[valve._id]] < timeRemaining - 2)
            //            {

            //            newList[indexMap[valve._id]] = (float)(valve._flowRate * (timeRemaining - 1 - newList[indexMap[valve._id]]) / Math.Pow(newList[indexMap[valve._id]], 2));
            //            //newList[indexMap[valve._id]] = (float)(valve._flowRate / newList[indexMap[valve._id]]) * (timeRemaining - 1 - newList[indexMap[valve._id]]);
            //            } else
            //            {
            //                newList[indexMap[valve._id]] = 0;
            //            }
            //        }
            //    }

            //    var maxValue = newList.Where(x => x < int.MaxValue / 2).Max();

            //    if (maxValue == 0)
            //    {
            //        //done
            //        Console.WriteLine($"Done with {timeRemaining} minutes left");
            //        break;
            //    }
            //    var maxIndex = 0;
            //    for(var i = 0; i < newList.Count; i++)
            //    {
            //        if(newList[i] == maxValue)
            //        {
            //            maxIndex = i;
            //            break;
            //        }
            //    }


            //    var timeToGetThere = matrix[indexMap[currentValveId]][maxIndex];
            //    timeRemaining -= timeToGetThere; //move to next valve

            //    currentValveId = indexMap.First(x => x.Value == maxIndex).Key;

            //    //todo remove based on time & total up
            //}

            /*
             * idea: dfs from each vertex we visit (non repetitive)
             * check all options when at leaf & under time
             * ...then max is path?
             */



            //Graph graph = new Graph(matrix.Count);

            //for(int i =0; i < matrix.Count; i++)
            //{
            //    for(int j =0; j < matrix.Count; j++)
            //    {
            //        if(matrix[i][j] != int.MaxValue)
            //        {
            //            graph.AddEdge(i, j);
            //        }
            //    }
            //}

            //graph.DFS(0);

            for (int i = 0; i < matrix.Count; i++)
            {
                Console.WriteLine("");
                for (int j = 0; j < matrix.Count; j++)
                {
                    //if (matrix[i][j] != int.MaxValue) // bad bc some cant connect in full one?
                    //{
                    //    // graph.AddEdge(i, j);
                    //}
                    Console.Write($" {matrix[i][j]}");
                }
            }
            Console.WriteLine("");


            return "";
            //1341 too low (&1350 lol)
        }

        public override string GetPart2Solution()
        {
            return "";
        }




        private List<Valve> ParseValves()
        {
            var rows = _rawInput.Split("\r\n");
            var valves = new List<Valve>();

            foreach(var row in rows)
            {
                var halves = row.Split("; ");
                var firstHalfPieces = halves[0].Split(" ");
                var secondHalfPieces = halves[1].Replace(",", "").Split(" ");
                var connectedIds = new List<string>(secondHalfPieces).GetRange(4, secondHalfPieces.Length - 4);
                var id = firstHalfPieces[1];
                var flowRate = int.Parse(firstHalfPieces[4].Split("=")[1]);

                valves.Add(new Valve(id, flowRate, connectedIds));
            }

            foreach(var valve in valves)
            {
                foreach(var connection in valve._connectedIds)
                {
                    var otherValve = valves.Where(x => x._id == connection).FirstOrDefault();
                    valve.AddValve(otherValve);
                }
            }

            return valves;
        }

        private void FlloydWarshall(List<List<int>> matrix, int numSteps)
        {
            for (var i = 0; i < matrix.Count; i++)
            {
                for (var j = 0; j < matrix.Count; j++)
                {
                    for (var k = 0; k < matrix.Count; k++)
                    {
                        if(matrix[i][k] != int.MaxValue && matrix[k][j] != int.MaxValue && matrix[i][j] > matrix[i][k] + matrix[k][j])
                        {
                            matrix[i][j] = matrix[i][k] + matrix[k][j];
                        }
                    }
                }
            }
        }

        public Dictionary<string, int> GetIndexMap(List<string> valveIds)
        {
            var indexMap = new Dictionary<string, int>();
            valveIds.Sort();
            for(var i = 0; i < valveIds.Count; i++)
            {
                indexMap.Add(valveIds[i], i);
            }

            return indexMap;
        }

        private void FlattenIndex(List<List<int>> matrix, int index)
        {
            var myRow = matrix[index];

            for(var i = 0; i < matrix.Count; i++)
            {
                if(myRow[i] != int.MaxValue)
                {
                    for(var j = 0; j < matrix.Count; j++)
                    {
                        if(i != j && myRow[j] != int.MaxValue && i != index && j != index)
                        {
                            if(matrix[i][j] == int.MaxValue)
                            {
                                matrix[i][j] = myRow[j] + myRow[i];
                                matrix[j][i] = myRow[j] + myRow[i];
                                //matrix[j][i] = 1;
                            } else
                            {
                                //matrix[i][j]++;
                                //matrix[j][i]++;
                            }
                            //matrix[j][i] ++;
                        }
                    }
                }
            }

            for(var i = 0; i< matrix.Count; i++)
            {
                matrix[index][i] = int.MaxValue;
                matrix[i][index] = int.MaxValue;
            }

            //return matrix;
        }

        private (List<List<int>> matrix, Dictionary<string, int> indexMap) CreateAdjMatrix()
        {
            var rows = _rawInput.Split("\r\n");
            var size = rows.Length;

            var matrix = new List<List<int>>();

            for(var i = 0; i < size; i ++) {
                matrix.Add(new List<int>());
                for(var j = 0; j < size; j ++) {
                    matrix[i].Add(int.MaxValue);
                }
            }

            var valves = new List<Valve>();

            foreach (var row in rows)
            {
                var halves = row.Split("; ");
                var firstHalfPieces = halves[0].Split(" ");
                var secondHalfPieces = halves[1].Replace(",", "").Split(" ");
                var connectedIds = new List<string>(secondHalfPieces).GetRange(4, secondHalfPieces.Length - 4);
                var id = firstHalfPieces[1];
                var flowRate = int.Parse(firstHalfPieces[4].Split("=")[1]);

                valves.Add(new Valve(id, flowRate, connectedIds));
            }

            var ids = valves.Select(x => x._id).ToList();
            //var idsToIgnore = valves.Where(x => x._flowRate == 0).Select(x => x._id).ToList();

            var indexMap = GetIndexMap(ids);

            foreach (var valve in valves)
            {
                //if(valve._flowRate != 0)
                //{
                    var index1 = indexMap[valve._id];
                    //add to list
                    foreach (var connection in valve._connectedIds)
                    {
                       //if(!idsToIgnore.Contains(connection))
                       // {
                            var index2 = indexMap[connection];
                            matrix[index1][index2] = 1;
                            matrix[index2][index1] = 1;
                        //}
                    }
                matrix[index1][index1] = 0;
                //} else
                //{
                //    for(var i = 0; i < valve._connectedIds.Count; i++)
                //    {
                //        var index1 = indexMap[valve._connectedIds[i]];
                //        for (var j = 0; j < valve._connectedIds.Count; j++)
                //        {
                //            if(i != j)
                //            {
                //                var index2 = indexMap[valve._connectedIds[j]];
                //                matrix[index1, index2] = 2;
                //                matrix[index2, index1] = 2;
                //            }
                //        }
                //    }
                //    //add joined paths to list
                //}
            }

            return (matrix, indexMap);
        }

        //DFS: https://www.geeksforgeeks.org/depth-first-search-or-dfs-for-a-graph/
        //public class Graph
        //{
        //    private int V; // No. of vertices

        //    // Array of lists for
        //    // Adjacency List Representation
        //    private List<int>[] adj;

        //    // Constructor
        //    public Graph(int v)
        //    {
        //        V = v;
        //        adj = new List<int>[v];
        //        for (int i = 0; i < v; ++i)
        //            adj[i] = new List<int>();
        //    }

        //    // Function to Add an edge into the graph
        //    public void AddEdge(int v, int w)
        //    {
        //        adj[v].Add(w); // Add w to v's list.
        //    }

        //    // A function used by DFS
        //    public void DFSUtil(int v, bool[] visited)
        //    {
        //        // Mark the current node as visited
        //        // and print it
        //        visited[v] = true;
        //        Console.Write(v + " ");

        //        // Recur for all the vertices
        //        // adjacent to this vertex
        //        List<int> vList = adj[v];
        //        foreach (var n in vList)
        //        {
        //            if (!visited[n])
        //                DFSUtil(n, visited);
        //        }
        //    }

        //    // The function to do DFS traversal.
        //    // It uses recursive DFSUtil()
        //    public void DFS(int v)
        //    {
        //        // Mark all the vertices as not visited
        //        // (set as false by default in c#)
        //        bool[] visited = new bool[V];

        //        // Call the recursive helper function
        //        // to print DFS traversal
        //        DFSUtil(v, visited);
        //    }
        //}
            //end DFS


            public class Valve
        {
            public readonly string _id;
            public int _flowRate;
            public bool _open;
            public readonly List<string> _connectedIds;
            public List<Valve> _connectedValves;

            public Valve(string id, int flowRate, List<string> connectedIds)
            {
                _id = id;
                _flowRate = flowRate;
                _connectedIds = connectedIds;
                _open = false;
                _connectedValves = new List<Valve>();
            }

            public void AddValve(Valve connectedValve)
            {
                _connectedValves.Add(connectedValve);
            }

            public int GetFlowRate()
            {
                return _open ? 0 : _flowRate;
            }

            public int GetTimeToPassThrough()
            {
                return _open || _flowRate == 0 ? 0 : 1;
            }

            public string GetNextValve(int timeRemaining)
            {
                var maxOptionId = "";
                var maxOption = 0;


                foreach (var valve in _connectedValves)
                {
                    var newList = new List<string> { _id };
                    var valveAddition = valve.GetMaxPossibleChoosingRoute(timeRemaining, newList, _id).totalPossible;
                    if (valveAddition > maxOption)
                    {
                        maxOption = valveAddition;
                        maxOptionId = valve._id;
                    }
                }

                return maxOptionId;
            }

            public (int totalPossible, string id) GetMaxPossibleChoosingRoute(int timeRemaining, List<string> idsVisited, string firstId)
            {
                var timeForMeToBeOpen = timeRemaining - 1 - (_open ? 0 : 1);
                var myFlowProvided = timeForMeToBeOpen * _flowRate;

                var maxOption = 0;
                var maxOptionId = "";

                var newList = new List<string>(idsVisited);
                newList.Add(_id);

                foreach(var valve in _connectedValves)
                {
                    if(valve._id == firstId)
                    {
                        continue;
                        return (myFlowProvided, _id);

                    }
                    if (!idsVisited.Contains(valve._id))
                    {
                        var valveAddition = valve.GetMaxPossibleChoosingRoute(timeForMeToBeOpen, newList, firstId).totalPossible;
                        if(valveAddition > maxOption)
                        {
                            maxOption = valveAddition;
                            maxOptionId = valve._id;
                        }
                    }
                }


                return (maxOption + myFlowProvided, maxOptionId);
            }
        }
    }
}
