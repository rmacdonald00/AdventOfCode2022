using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2022.DaySolutions
{
    class Day2 : DaySolver
    {
        public Day2(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var rounds = ParseRounds();
            var totalPoints = rounds.Select(x => x.GetScore()).Sum();
            return totalPoints.ToString();
        }

        public override string GetPart2Solution()
        {
            var rounds = ParseRoundsPart2();
            var totalPoints = rounds.Select(x => x.GetScore()).Sum();
            return totalPoints.ToString();
        }

        private static List<(Move opp, Move me, Result result)> allGameOptions = new List<(Move opp, Move me, Result result)>()
            {
                (Move.Rock, Move.Rock, Result.Tie),
                (Move.Rock, Move.Paper, Result.Win),
                (Move.Rock, Move.Scissors, Result.Loss),
                (Move.Paper, Move.Rock, Result.Loss),
                (Move.Paper, Move.Paper, Result.Tie),
                (Move.Paper, Move.Scissors, Result.Win),
                (Move.Scissors, Move.Rock, Result.Win),
                (Move.Scissors, Move.Paper, Result.Loss),
                (Move.Scissors, Move.Scissors, Result.Tie),
            };

        private List<RpsRound> ParseRounds()
        {
            var roundData = _rawInput.Split("\r\n");
            return roundData.Select(x =>
            {
                var plays = x.Split(" ").Select(y => y.ToCharArray()[0]).ToList();
                return new RpsRound(AbcToMove(plays[0]), XyzToMove(plays[1]));
            }).ToList();
        }

        private List<RpsRound> ParseRoundsPart2()
        {
            var roundData = _rawInput.Split("\r\n");
            return roundData.Select(x =>
            {
                var plays = x.Split(" ").Select(y => y.ToCharArray()[0]).ToList();
                return new RpsRound(AbcToMove(plays[0]), XyzToResult(plays[1]));
            }).ToList();
        }

        private Move AbcToMove(char move)
        {
            switch (move)
            {
                case 'A': return Move.Rock;
                case 'B': return Move.Paper;
                default: return Move.Scissors;
            }
        }

        private Move XyzToMove(char move)
        {
            switch (move)
            {
                case 'X': return Move.Rock;
                case 'Y': return Move.Paper;
                default: return Move.Scissors;
            }
        }

        private Result XyzToResult(char move)
        {
            switch (move)
            {
                case 'X': return Result.Loss;
                case 'Y': return Result.Tie;
                default: return Result.Win;
            }
        }

        private enum Result
        {
            Tie,
            Win,
            Loss
        }
        private enum Move
        {
            Rock,
            Paper,
            Scissors
        }

        private class RpsRound
        {
            private Move _opponent;
            private Move _me;
            private Result _gameResult;

            public RpsRound(Move opponent, Move me)
            {
                _opponent = opponent;
                _me = me;
                _gameResult = allGameOptions.First(x => x.opp == opponent && x.me == me).result;
            }

            public RpsRound(Move opponent, Result result)
            {
                _opponent = opponent;
                _gameResult = result;
                _me = allGameOptions.First(x => x.opp == opponent && x.result == result).me;
            }

            public int GetScore()
            {
                return GetScoreForMovePlayed() + GetScoreForResult(_gameResult);
            }

            private int GetScoreForMovePlayed()
            {
                switch (_me)
                {
                    case Move.Rock: return 1;
                    case Move.Paper: return 2;
                    case Move.Scissors: return 3;
                    default: return 0;
                }
            }

            private int GetScoreForResult(Result result)
            {
                switch (result)
                {
                    case Result.Win: return 6;
                    case Result.Tie: return 3;
                    default: return 0;
                }
            }
        }
    }
}
