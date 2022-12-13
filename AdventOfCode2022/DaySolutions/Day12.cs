using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2022.DaySolutions
{
    class Day12 : DaySolver
    {
        public Day12(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var parsedVals = ParseToAdjacencyMatrix();
            var dists = Dijkstras(parsedVals.matrix, parsedVals.sourceIndex);
            return dists[parsedVals.destIndex].ToString();
        }

        public override string GetPart2Solution()
        {
            //main idea in case I come back confused
            // 1) flip adjacency matrix
            // 2) use dest as source
            // 3) find min of those
            //note: I did try to loop through all 100+ source indices and do dijkstras...didn't work
            var parsedVals = ParseToAdjacencyMatrixPart2();
            var dists = Dijkstras(parsedVals.matrix, parsedVals.destIndex);
            var minPathDist = int.MaxValue;
            foreach(var source in parsedVals.sourceIndices)
            {
                if(dists[source] < minPathDist)
                {
                    minPathDist = dists[source];
                }
            }
            
            return minPathDist.ToString();
        }

        private (List<List<int>> matrix, int sourceIndex, int destIndex) ParseToAdjacencyMatrix()
        {
            var inputRows = _rawInput.Split("\r\n").Select(x => x.ToCharArray()).ToList();
            var numIndices = inputRows.Count * inputRows[0].Length;
            var sourceIndex = 0;
            var destIndex = 0;
            var adjacencyMatrix = new List<List<int>>();
            for (int i = 0; i < numIndices; i++)
            {
                adjacencyMatrix.Add(new List<int>());
                for (int j = 0; j < numIndices; j++)
                {
                    adjacencyMatrix[i].Add(0);
                }
            }

            for (int i = 0; i < inputRows.Count; i++)
            {
                for (int j = 0; j < inputRows[i].Length; j++)
                {
                    var vertexIndex = i * inputRows[i].Length + j;
                    var cellValue = inputRows[i][j];
                    if (cellValue == 'S') {
                        sourceIndex = vertexIndex;
                        cellValue = 'a';
                        inputRows[i][j] = 'a';
                    } else if(cellValue == 'E')
                    {
                        destIndex = vertexIndex;
                        cellValue = 'z';
                        inputRows[i][j] = 'z';

                    }
                    if (i != 0 && inputRows[i - 1][j] >= cellValue - 1)
                    {
                        adjacencyMatrix[(i - 1) * inputRows[i].Length + j][vertexIndex] = 1;
                    }
                    if (i != inputRows.Count - 1 && inputRows[i + 1][j] >= cellValue - 1)
                    {
                        adjacencyMatrix[(i + 1) * inputRows[i].Length + j][vertexIndex] = 1;

                    }
                    if (j != 0 && inputRows[i][j - 1] >= cellValue - 1)
                    {
                        adjacencyMatrix[i * inputRows[i].Length + j - 1][vertexIndex] = 1;

                    }
                    if (j != inputRows[i].Length - 1 && inputRows[i][j + 1] >= cellValue - 1)
                    {
                        adjacencyMatrix[i * inputRows[i].Length + j + 1][vertexIndex] = 1;
                    }
                }
            }
            return (adjacencyMatrix, sourceIndex, destIndex);
        }

        private (List<List<int>> matrix, List<int> sourceIndices, int destIndex) ParseToAdjacencyMatrixPart2()
        {
            var inputRows = _rawInput.Split("\r\n").Select(x => x.ToCharArray()).ToList();
            var numIndices = inputRows.Count * inputRows[0].Length;
            var sourceIndex = new List<int>();
            var destIndex = 0;
            var adjacencyMatrix = new List<List<int>>();
            for (int i = 0; i < numIndices; i++)
            {
                adjacencyMatrix.Add(new List<int>());
                for (int j = 0; j < numIndices; j++)
                {
                    adjacencyMatrix[i].Add(0);
                }
            }

            for (int i = 0; i < inputRows.Count; i++)
            {
                for (int j = 0; j < inputRows[i].Length; j++)
                {
                    var vertexIndex = i * inputRows[i].Length + j;
                    var cellValue = inputRows[i][j];
                    if (cellValue == 'S' || cellValue == 'a')
                    {
                        sourceIndex.Add(vertexIndex);
                        cellValue = 'a';
                        inputRows[i][j] = 'a';
                    }
                    else if (cellValue == 'E')
                    {
                        destIndex = vertexIndex;
                        cellValue = 'z';
                        inputRows[i][j] = 'z';

                    }
                    if (i != 0 && inputRows[i - 1][j] >= cellValue - 1)
                    {
                        adjacencyMatrix[vertexIndex][(i - 1) * inputRows[i].Length + j] = 1;
                    }
                    if (i != inputRows.Count - 1 && inputRows[i + 1][j] >= cellValue - 1)
                    {
                        adjacencyMatrix[vertexIndex][(i + 1) * inputRows[i].Length + j] = 1;

                    }
                    if (j != 0 && inputRows[i][j - 1] >= cellValue - 1)
                    {
                        adjacencyMatrix[vertexIndex][i * inputRows[i].Length + j - 1] = 1;

                    }
                    if (j != inputRows[i].Length - 1 && inputRows[i][j + 1] >= cellValue - 1)
                    {
                        adjacencyMatrix[vertexIndex][i * inputRows[i].Length + j + 1] = 1;
                    }
                }
            }
            return (adjacencyMatrix, sourceIndex, destIndex);
        }


        private List<int> Dijkstras(List<List<int>> matrix, int sourceIndex)
        {
            GFG g = new GFG();
            var dists = g.dijkstra(matrix, sourceIndex, matrix.Count);
            return new List<int>(dists);
        }

        //from https://www.geeksforgeeks.org/dijkstras-shortest-path-algorithm-greedy-algo-7/
        class GFG
        {
            // A utility function to find the
            // vertex with minimum distance
            // value, from the set of vertices
            // not yet included in shortest
            // path tree
            int minDistance(int[] dist, bool[] sptSet, int numVertices)
            {
                // Initialize min value
                int min = int.MaxValue, min_index = -1;

                for (int v = 0; v < numVertices; v++)
                    if (sptSet[v] == false && dist[v] <= min)
                    {
                        min = dist[v];
                        min_index = v;
                    }

                return min_index;
            }


            // Function that implements Dijkstra's
            // single source shortest path algorithm
            // for a graph represented using adjacency
            // matrix representation
            public int[] dijkstra(List<List<int>> graph, int src, int numVertices)
            {
                int[] dist
                    = new int[numVertices]; // The output array. dist[i]
                                            // will hold the shortest
                                            // distance from src to i

                // sptSet[i] will true if vertex
                // i is included in shortest path
                // tree or shortest distance from
                // src to i is finalized
                bool[] sptSet = new bool[numVertices];

                // Initialize all distances as
                // INFINITE and stpSet[] as false
                for (int i = 0; i < numVertices; i++)
                {
                    dist[i] = int.MaxValue;
                    sptSet[i] = false;
                }

                // Distance of source vertex
                // from itself is always 0
                dist[src] = 0;

                // Find shortest path for all vertices
                for (int count = 0; count < numVertices - 1; count++)
                {
                    // Pick the minimum distance vertex
                    // from the set of vertices not yet
                    // processed. u is always equal to
                    // src in first iteration.
                    int u = minDistance(dist, sptSet, numVertices);

                    // Mark the picked vertex as processed
                    sptSet[u] = true;

                    // Update dist value of the adjacent
                    // vertices of the picked vertex.
                    for (int v = 0; v < numVertices; v++)

                        // Update dist[v] only if is not in
                        // sptSet, there is an edge from u
                        // to v, and total weight of path
                        // from src to v through u is smaller
                        // than current value of dist[v]
                        if (!sptSet[v] && graph[u][v] != 0
                            && dist[u] != int.MaxValue
                            && dist[u] + graph[u][v] < dist[v])
                            dist[v] = dist[u] + graph[u][v];
                }
                return dist;
            }
        }
        //end from https://www.geeksforgeeks.org/dijkstras-shortest-path-algorithm-greedy-algo-7/
    }
}
