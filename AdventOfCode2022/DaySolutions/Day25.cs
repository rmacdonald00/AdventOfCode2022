using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2022.DaySolutions
{
    class Day25 : DaySolver
    {
        public Day25(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var decimalValues = GetDecimalVersions();
            var total = decimalValues.Sum();
            return DecimalToSnafu(total);
        }

        public override string GetPart2Solution()
        {
            return "";
        }

        private List<long> GetDecimalVersions()
        {
            var decimalValues = new List<long>();

            var snafuStrings = _rawInput.Split("\r\n");

            foreach(var snafu in snafuStrings)
            {
                decimalValues.Add(SnafuToDecimal(snafu));
            }

            return decimalValues;
        }


        private long SnafuToDecimal(string snafu)
        {
            long value = 0;

            var numDigits = snafu.Length;
            for(int i = numDigits - 1; i >= 0; i--)
            {
                var digitPlace = (long) Math.Pow(5, i);
                var multiplier = snafu.ElementAt(numDigits - i - 1);
                //var multiplier = snafu[numDigits - i - 1];

                int intMult;
                
                if(multiplier == '-')
                {
                    intMult = -1;
                } else if (multiplier == '=')
                {
                    intMult = -2;
                }
                else
                {
                    intMult = multiplier - 48;
                }
                value += digitPlace * intMult;
            }

            return value;
        }

        private string DecimalToSnafu(long decimalVal)
        {
            var biggestPowerToConsider = 0;
            while(Math.Pow(5, biggestPowerToConsider) < decimalVal)
            {
                biggestPowerToConsider++;
            }

            var maxValueForDigits = new Dictionary<long, long>();
            maxValueForDigits.Add(0, 0);

            for(int i = 1; i <= biggestPowerToConsider; i ++)
            {
                maxValueForDigits.Add(i, (long) Math.Pow(5, i - 1) * 2 + maxValueForDigits[i-1]);
            }

            var snafuString = "";
            for(int i = biggestPowerToConsider; i >= 0; i --)
            {
                var placeValue = (long)Math.Pow(5, i);

                    //determine value to add
                    var nextValueInDecimal = 0;
                    if(decimalVal > maxValueForDigits[i])
                    {
                        decimalVal -= placeValue;
                        nextValueInDecimal++;
                        if (decimalVal > maxValueForDigits[i])
                        {
                            decimalVal -= placeValue;
                            nextValueInDecimal++;
                        }
                    } else if (Math.Abs(decimalVal ) > maxValueForDigits[i])
                    {
                        decimalVal += placeValue;
                        nextValueInDecimal--;
                        if (Math.Abs(decimalVal) > maxValueForDigits[i])
                        {
                            decimalVal += placeValue;
                            nextValueInDecimal--;
                        }
                    }

                    if(nextValueInDecimal == -2)
                    {
                        snafuString += "=";
                    } else if (nextValueInDecimal == -1)
                    {
                        snafuString += "-";
                    } else
                    {
                        snafuString += nextValueInDecimal.ToString();
                    }
            }

            return snafuString;
        }
    }
}
