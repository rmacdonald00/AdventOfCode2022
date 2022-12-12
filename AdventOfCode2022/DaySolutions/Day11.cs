using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2022.DaySolutions
{
    class Day11 : DaySolver
    {
        public Day11(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var monkeys = ParseMonkeys1();
            var monkeyExecutions = ExecuteRoundsOnMonkeys(monkeys, 20).Select(x => x.GetCountOfInspectionsExecuted()).OrderByDescending(x => x).ToList();
            return (monkeyExecutions[0] * monkeyExecutions[1]).ToString();
        }

        public override string GetPart2Solution()
        {
            var monkeys = ParseMonkeys2();
            var monkeyExecutions = ExecuteRoundsOnMonkeys(monkeys, 10000).Select(x => x.GetCountOfInspectionsExecuted()).OrderByDescending(x => x).ToList();
            long total2 = monkeyExecutions[0] * (long) monkeyExecutions[1];
            return $"{monkeyExecutions[0] * (long)monkeyExecutions[1]}";
        }

        private List<Monkey> ParseMonkeys1()
        {
            List<Monkey> monkeysToReturn = BasicMonkeyParsing();

            foreach (var monkey in monkeysToReturn)
            {
                monkey.SetWorryUpdater((BigInteger value) => { return value / 3; });
            }
            return monkeysToReturn;
        }

        private List<Monkey> BasicMonkeyParsing()
        {
            var monkeySections = _rawInput.Split("\r\n\r\n");
            var monkeysToReturn = new List<Monkey>();
            foreach (var monkeyText in monkeySections)
            {
                var lines = monkeyText.Trim().Split("\r\n");
                var split = lines[0].Trim().Split(" ")[1];
                int id = int.Parse(lines[0].Trim().Split(" ")[1].Trim().Split(':')[0]);
                List<BigInteger> worryLevels = lines[1].Trim().Split(":")[1].Trim().Split(',').Select(x => BigInteger.Parse(x.Trim())).ToList();
                var divisor = int.Parse(lines[3].Trim().Split(" ")[3]);
                var trueMonkey = int.Parse(lines[4].Trim().Split(" ")[5]);
                var falseMonkey = int.Parse(lines[5].Trim().Split(" ")[5]);

                var operationPieces = lines[2].Trim().Split(" ");
                var intOperator = operationPieces[4].Trim();
                var secondOperand = operationPieces[5].Trim();
                Func<BigInteger, BigInteger> operationFunc;
                if (secondOperand == "old")
                {
                    if (intOperator == "*")
                    {
                        operationFunc = (BigInteger value) => { return value * value; };
                    }
                    else
                    {
                        operationFunc = (BigInteger value) => { return value + value; };

                    }
                }
                else
                {
                    if (intOperator == "*")
                    {
                        operationFunc = (BigInteger value) => { return value * BigInteger.Parse(secondOperand); };
                    }
                    else
                    {
                        operationFunc = (BigInteger value) => { return value + BigInteger.Parse(secondOperand); };

                    }
                }
                monkeysToReturn.Add(new Monkey(id, worryLevels, operationFunc, divisor, trueMonkey, falseMonkey));
            }

            return monkeysToReturn;
        }

        private List<Monkey> ParseMonkeys2()
        {

            List<Monkey> monkeysToReturn = BasicMonkeyParsing();

            var groupDivisor = 1;
            foreach( var monkey in monkeysToReturn)
            {
                groupDivisor *= monkey._divisor;
            }

            foreach (var monkey in monkeysToReturn)
            {
                monkey.SetWorryUpdater((BigInteger value) => { return value % groupDivisor; });
            }
            return monkeysToReturn;
        }

        private List<Monkey> ExecuteRoundsOnMonkeys(List<Monkey> monkeys, int numberOfRounds)
        {
            for (int i = 0; i < numberOfRounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    List<(int monkeyToThrowTo, BigInteger worryLevel)> moves = monkey.GetThrowsToMakeForTurn();
                    foreach (var move in moves)
                    {
                        monkeys.First(x => x._id == move.monkeyToThrowTo).AddItem(move.worryLevel);
                    }
                }
            }
            return monkeys;
        }

        private class Monkey
        {
            private List<BigInteger> _itemWorryLevels;
            private Func<BigInteger, BigInteger> _inspectionOperation;
            public readonly int _divisor;
            private readonly int _trueMonkey;
            private readonly int _falseMonkey;
            public readonly int _id;
            private int _countOfInspectionsExecuted = 0;
            private Func<BigInteger, BigInteger> _worryUpdater;

            public Monkey(int id, List<BigInteger> worryLevels, Func<BigInteger, BigInteger> opDivisor, int testFunc, int trueMonkey, int falseMonkey)
            {
                _id = id;
                _itemWorryLevels = worryLevels;
                _inspectionOperation = opDivisor;
                _divisor = testFunc;
                _trueMonkey = trueMonkey;
                _falseMonkey = falseMonkey;
            }

            public void SetWorryUpdater(Func<BigInteger, BigInteger> worryUpdater)
            {
                _worryUpdater = worryUpdater;
            }

            public int GetCountOfInspectionsExecuted()
            {
                return _countOfInspectionsExecuted;
            }

            public List<(int monkeyToThrowTo, BigInteger worryLevel)> GetThrowsToMakeForTurn()
            {
                var movesToMake = new List<(int monkeyToThrowTo, BigInteger worryLevel)>();
                _countOfInspectionsExecuted += _itemWorryLevels.Count;
                foreach (var item in _itemWorryLevels)
                {
                    var worryItem = _inspectionOperation(item);
                    worryItem = _worryUpdater(worryItem);
                    var monkeyToThrowTo = (worryItem % _divisor) == 0 ? _trueMonkey : _falseMonkey;
                    movesToMake.Add((monkeyToThrowTo, worryItem));
                }
                _itemWorryLevels.Clear();
                return movesToMake;
            }

            public void AddItem(BigInteger worryLevel)
            {
                _itemWorryLevels.Add(worryLevel);
            }
        }
    }
}
