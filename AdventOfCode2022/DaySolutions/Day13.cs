using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.DaySolutions
{
    class Day13 : DaySolver
    {
        public Day13(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var packets = ParsePackets();
            var inOrderIndices = new List<int>();
            for(int i = 0; i < packets.Count; i++)
            {
                if(GetInOrder(packets[i].leftItem, packets[i].rightItem) < 0)
                {
                    inOrderIndices.Add(i + 1);
                }
            }
            return inOrderIndices.Sum().ToString();
        }

        
        public override string GetPart2Solution()
        {
            return "";
        }


        private abstract class InfiniteObject
        {
            public readonly ComparableFunkyList _parent;

            public InfiniteObject(ComparableFunkyList parent)
            {
                _parent = parent;
            }

            public abstract ComparableFunkyList GetAsFunkyList();
        }
        private class ComparableInt : InfiniteObject
        {
            public readonly int _value;
            public ComparableInt(int value, ComparableFunkyList parent) : base(parent)
            {
                _value = value;
            }

            public override ComparableFunkyList GetAsFunkyList()
            {
                var myList = new ComparableFunkyList(_parent);
                myList.AddToList(this);
                return myList;
            }
        }

        private class ComparableFunkyList : InfiniteObject
        {
            public readonly List<InfiniteObject> _values;

            public ComparableFunkyList(ComparableFunkyList parent) : base(parent)
            {
                _values = new List<InfiniteObject>();
            }

            public void AddToList(InfiniteObject newObj)
            {
                _values.Add(newObj);
            }

            public override ComparableFunkyList GetAsFunkyList()
            {
                return this;
            }

            public int Count()
            {
                return _values.Count;
            }

            public InfiniteObject GetObjectAtIndex(int index)
            {
                return _values[index];
            }
        }

        private List<(InfiniteObject leftItem, InfiniteObject rightItem)> ParsePackets()
        {
            var packets = new List<(InfiniteObject leftItem, InfiniteObject rightItem)>();

            var groups = _rawInput.Split("\r\n\r\n");
            foreach (var group in groups)
            {
                var pieces = group.Split("\r\n");
                var left = pieces[0];
                var right = pieces[1];
                var leftList = GetListObj(left);
                var rightList = GetListObj(right);
                packets.Add((leftList, rightList));
            }

            return packets;
        }

        private ComparableFunkyList GetListObj(string left)
        {
            var rootList = new ComparableFunkyList(null);
            var currLeftList = rootList;
            var i = 1; //ignore first [
            while (i < left.Length - 1)// ignore last ]
            {
                if (left[i] == ',')
                {
                    i++;
                }
                else if (left[i] == '[')
                {
                    var newList = new ComparableFunkyList(currLeftList);
                    currLeftList.AddToList(newList);
                    currLeftList = newList;
                    i++;
                }
                else if (left[i] == ']')
                {
                    currLeftList = currLeftList._parent;
                    i++;
                }
                else //number
                {
                    var firstMatch = Regex.Matches(left.Substring(i), @"[\d]+")[0].Value;
                    currLeftList.AddToList(new ComparableInt(int.Parse(firstMatch), currLeftList));
                    i += firstMatch.Length;
                }
            }
            return rootList;
        }

        private int GetInOrder(InfiniteObject leftObj, InfiniteObject rightObj) //-1 in order, 0 equal, 1 out of order
        {
            if(leftObj.GetType() == typeof(ComparableInt) && rightObj.GetType() == typeof(ComparableInt))
            {
                return ((ComparableInt)leftObj)._value - ((ComparableInt)rightObj)._value;
            } 
            else if (leftObj.GetType() == typeof(ComparableFunkyList) && rightObj.GetType() == typeof(ComparableFunkyList))
            {
                var order = 0;
                var indexAt = 0;
                while(order == 0)
                {
                    if(indexAt >= ((ComparableFunkyList)leftObj).Count() && indexAt < ((ComparableFunkyList)rightObj).Count())
                    {
                        return -1;
                    }
                    if (indexAt >= ((ComparableFunkyList)rightObj).Count() && indexAt < ((ComparableFunkyList)leftObj).Count())
                    {
                        return 1;
                    }
                    if (indexAt >= ((ComparableFunkyList)rightObj).Count() && indexAt >= ((ComparableFunkyList)leftObj).Count())
                    {
                        return 0;
                    }
                    order = GetInOrder(((ComparableFunkyList)leftObj).GetObjectAtIndex(indexAt), ((ComparableFunkyList)rightObj).GetObjectAtIndex(indexAt));
                    indexAt++;
                }
                return order;
            } else
            {
                if (leftObj.GetType() == typeof(ComparableInt))
                {
                    return GetInOrder(leftObj.GetAsFunkyList(), rightObj);
                } else
                {
                    return GetInOrder(leftObj, rightObj.GetAsFunkyList());
                }
            }
        }
        
    }
}
