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
            var packets = ParsePacketsPart2();
            packets.Add(GetListObj("[[2]]", true));
            packets.Add(GetListObj("[[6]]", true));
            packets.Sort();

            var product = 1;
            for (int i = 0; i < packets.Count; i++){
                if (packets[i]._identifiable)
                {
                    product *= (i + 1);
                }
            }
            return product.ToString();
        }

        private List<ComparableFunkyList> ParsePacketsPart2()
        {
            var packets = new List<ComparableFunkyList>();

            var groups = _rawInput.Replace("\r\n\r\n", "\r\n").Split("\r\n");
            foreach (var group in groups)
            {

                packets.Add(GetListObj(group));
            }

            return packets;
        }


        private abstract class InfiniteObject
        {
           public abstract ComparableFunkyList GetAsFunkyList();
        }
        private class ComparableInt : InfiniteObject
        {
            public readonly int _value;
            public ComparableInt(int value)
            {
                _value = value;
            }

            public override ComparableFunkyList GetAsFunkyList()
            {
                var myList = new ComparableFunkyList(null); //parent doesn't matter here
                myList.AddToList(this);
                return myList;
            }
        }

        private class ComparableFunkyList : InfiniteObject, IComparable
        {
            private readonly List<InfiniteObject> _values;
            public readonly bool _identifiable;
            public readonly ComparableFunkyList _parent;

            public ComparableFunkyList(ComparableFunkyList parent, bool identifiable = false)
            {
                _identifiable = identifiable;
                _parent = parent;
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

            public int CompareTo(object obj)
            {
                return GetInOrder(this, (ComparableFunkyList) obj);
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

        private ComparableFunkyList GetListObj(string left, bool identifiable = false)
        {
            var rootList = new ComparableFunkyList(null, identifiable);
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
                    var newList = new ComparableFunkyList(currLeftList, identifiable);
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
                    currLeftList.AddToList(new ComparableInt(int.Parse(firstMatch)));
                    i += firstMatch.Length;
                }
            }
            return rootList;
        }

        private static int GetInOrder(InfiniteObject leftObj, InfiniteObject rightObj) //-1 in order, 0 equal, 1 out of order
        {
            if(leftObj.GetType() == typeof(ComparableInt) && rightObj.GetType() == typeof(ComparableInt))
            {
                return ((ComparableInt)leftObj)._value - ((ComparableInt)rightObj)._value;
            } 
            else if (leftObj.GetType() == typeof(ComparableFunkyList) && rightObj.GetType() == typeof(ComparableFunkyList))
            {
                var order = 0;
                var indexAt = 0;

                var leftFunkyList = ((ComparableFunkyList)leftObj);
                var rightFunkyList = ((ComparableFunkyList)rightObj);
                while (order == 0)
                {
                    if(indexAt >= leftFunkyList.Count() && indexAt < rightFunkyList.Count())
                    {
                        return -1;
                    }
                    if (indexAt >= rightFunkyList.Count() && indexAt < leftFunkyList.Count())
                    {
                        return 1;
                    }
                    if (indexAt >= rightFunkyList.Count() && indexAt >= leftFunkyList.Count())
                    {
                        return 0;
                    }
                    order = GetInOrder(leftFunkyList.GetObjectAtIndex(indexAt), rightFunkyList.GetObjectAtIndex(indexAt));
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
