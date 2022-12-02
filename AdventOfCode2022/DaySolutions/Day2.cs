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

        private List<RpsRound> ParseRounds()
        {
            var roundData = _rawInput.Split("\r\n");
            return roundData.Select(x =>
            {
                var plays = x.Split(" ").Select(y => y.ToCharArray()[0]).ToList();
                return new RpsRound(plays[0], plays[1]);
            }).ToList();
        }

        private List<RpsRound> ParseRoundsPart2()
        {
            var roundData = _rawInput.Split("\r\n");
            return roundData.Select(x =>
            {
                var plays = x.Split(" ").Select(y => y.ToCharArray()[0]).ToList();
                var myPlay = GetMyPlay(plays[0], plays[1]);
                return new RpsRound(plays[0], myPlay);
            }).ToList();
        }

        private char GetMyPlay(char opponent, char result)
        {
            Move oppMove = Move.Rock;
            Result gameResult = Result.Tie;

            switch (opponent)
            {
                case 'A': oppMove = Move.Rock; break;
                case 'B': oppMove = Move.Paper; break;
                case 'C': oppMove = Move.Scissors; break;
            }

            switch (result)
            {
                case 'X': gameResult = Result.Loss; break;
                case 'Y': gameResult = Result.Tie; break;
                case 'Z': gameResult = Result.Win; break;
            }

            if(gameResult == Result.Tie)
            {
                return opponent;
            } else if(gameResult == Result.Win)
            {
                if(oppMove == Move.Rock)
                {
                    return 'Y';
                } else if (oppMove == Move.Paper)
                {
                    return 'Z';
                } else //scissors
                {
                    return 'X';
                }
            } else //loss
            {
                if (oppMove == Move.Rock)
                {
                    return 'Z';
                }
                else if (oppMove == Move.Paper)
                {
                    return 'X';
                }
                else //scissors
                {
                    return 'Y';
                }
            }

            return 'A';
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
            // A=Rock, B=Paper, C=Scissors
            // X=Rock, Y=Paper, Z=Scissors
            private readonly Move _opponent;
            private readonly Move _me;

            public RpsRound(char opponent, char me)
            {
                switch (me)
                {
                    case 'X': _me = Move.Rock; break;
                    case 'Y': _me = Move.Paper; break;
                    case 'Z': _me = Move.Scissors; break;
                    //part 2 mess:
                    case 'A': _me = Move.Rock; break;
                    case 'B': _me = Move.Paper; break;
                    case 'C': _me = Move.Scissors; break;
                }

                switch (opponent)
                {
                    case 'A': _opponent = Move.Rock; break;
                    case 'B': _opponent = Move.Paper; break;
                    case 'C': _opponent = Move.Scissors; break;
                }
            }

            public int GetScore()
            {
                return GetScoreForMovePlayed() + GetScoreForResult(GetResult());
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

            private Result GetResult()
            {
                if(_me == _opponent)
                {
                    return Result.Tie;
                }
                if (_me == Move.Rock)
                {
                    return _opponent == Move.Scissors ? Result.Win : Result.Loss;
                }
                if (_me == Move.Paper)
                {
                    return _opponent == Move.Rock ? Result.Win : Result.Loss;
                }
                if (_me == Move.Scissors)
                {
                    return _opponent == Move.Paper ? Result.Win : Result.Loss;
                }
                return Result.Loss; // for zero points
            }

            private int GetScoreForResult(Result result)
            {
                switch (result)
                {
                    case Result.Win: return 6;
                    case Result.Tie: return 3;
                    //case Result.Loss: return 0;
                    default: return 0;
                }
            }
        }
    }
}
