using System.Collections.Generic;

namespace AdventOfCode2022.DaySolutions
{
    class Day8 : DaySolver
    {
        public Day8(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var trees = GetForest();
            var countVisible = GetCountVisible(trees);
            return countVisible.ToString();
        }

        public override string GetPart2Solution()
        {
            var trees = GetForest();
            var highestScenicCount = GetHighestScenicCount(trees);
            return highestScenicCount.ToString();
        }

        private List<List<int>> GetForest()
        {
            var rows = _rawInput.Split("\r\n");
            var forest = new List<List<int>>();
            for(int i = 0; i < rows.Length; i++)
            {
                forest.Add(new List<int>());
                for(int j = 0; j < rows[i].Length; j ++)
                {
                    forest[i].Add(int.Parse(rows[i][j].ToString()));
                }
            }
            return forest;
        }

        private int GetCountVisible(List<List<int>> trees)
        {
            var totalCount = 0;
            totalCount += trees.Count * 2; // side edges
            totalCount += (trees[0].Count - 2) * 2;
            for (int i = 0; i < trees.Count - 1; i++)
            {
                for(int j = 0; j < trees[i].Count - 1; j++)
                {
                    if (GetIsVisible(trees, i, j)) totalCount++;
                }
            }
            return totalCount;
        }

        private bool GetIsVisible(List<List<int>> trees, int index1, int index2)
        {
            var myValue = trees[index1][index2];
            if(index1 == 0 || index2 == 0 || index1 >= trees.Count || index2 >= trees[0].Count)
            {
                return true;
            }
            //left
            var tallestBetweenLeftEdge = 0;
            for(int i = 0; i < index2; i++)
            {
                var valToCompare = trees[index1][i];
                if(valToCompare > myValue)
                {
                    tallestBetweenLeftEdge = 10;
                    break;
                }
                if (tallestBetweenLeftEdge < trees[index1][i])
                {
                    tallestBetweenLeftEdge = trees[index1][i];
                }
            }
            if (tallestBetweenLeftEdge < myValue)
            {
                return true;
            }
            //right
            var tallestBetweenRightEdge = 0;
            for (int i = index2 + 1; i < trees[0].Count; i++)
            {
                var valToCompare = trees[index1][i];
                if (valToCompare > myValue)
                {
                    tallestBetweenRightEdge = 10;
                    break;
                }
                if (tallestBetweenRightEdge < trees[index1][i])
                {
                    tallestBetweenRightEdge = trees[index1][i];
                }
            }
            if (tallestBetweenRightEdge < myValue)
            {
                return true;
            }
            //top
            var tallestBetweenTopEdge = 0;
            for (int i = 0; i < index1; i++)
            {
                var valToCompare = trees[i][index2];
                if (valToCompare > myValue)
                {
                    tallestBetweenTopEdge = 10;
                    break;
                }
                if (tallestBetweenTopEdge < trees[i][index2])
                {
                    tallestBetweenTopEdge = trees[i][index2];
                }
            }
            if (tallestBetweenTopEdge < myValue)
            {
                return true;
            }
            //right
            var tallestBetweenBottomEdge = 0;
            for (int i = index1 + 1; i < trees.Count; i++)
            {
                var valToCompare = trees[i][index2];
                if (valToCompare > myValue)
                {
                    tallestBetweenBottomEdge = 10;
                    break;
                }
                if (tallestBetweenBottomEdge < trees[i][index2])
                {
                    tallestBetweenBottomEdge = trees[i][index2];
                }
            }
            if (tallestBetweenBottomEdge < myValue)
            {
                return true;
            }
            return false;
        }


        private int GetHighestScenicCount(List<List<int>> trees)
        {
            var maxScenicScore = 0;
            for (int i = 0; i < trees.Count - 1; i++)
            {
                for (int j = 0; j < trees[i].Count - 1; j++)
                {
                    var scenicScore = GetScenicScore(trees, i, j);
                    if (scenicScore > maxScenicScore)
                    {
                        maxScenicScore = scenicScore;
                    }
                }
            }
            return maxScenicScore;
        }

        private int GetScenicScore(List<List<int>> trees, int index1, int index2)
        {
            var myValue = trees[index1][index2];
            if (index1 == 0 || index2 == 0 || index1 >= trees.Count || index2 >= trees[0].Count)
            {
                return 0;
            }
            //left
            var countVisibleToLeft = 0;
            for (int i = index2 - 1; i >= 0; i--)
            {
                var valToCompare = trees[index1][i];
                countVisibleToLeft++;
                if (valToCompare >= myValue)
                {
                    break;
                }
            }
            //right
            var countVisibleToRight = 0;
            for (int i = index2 + 1; i < trees[0].Count; i++)
            {
                var valToCompare = trees[index1][i];
                countVisibleToRight++;
                if (valToCompare >= myValue)
                {
                    break;
                }
            }
            //top
            var countVisibleToTop = 0;
            for (int i = index1 - 1; i >= 0; i--)
            {
                var valToCompare = trees[i][index2];
                countVisibleToTop++;
                if (valToCompare >= myValue)
                {
                    break;
                }
            }
            //right
            var countVisibleToBottom = 0;
            for (int i = index1 + 1; i < trees.Count; i++)
            {
                var valToCompare = trees[i][index2];
                countVisibleToBottom++;
                if (valToCompare >= myValue)
                {
                    break;
                }
            }
            return countVisibleToBottom * countVisibleToLeft * countVisibleToRight * countVisibleToTop;
        }
    }
}
