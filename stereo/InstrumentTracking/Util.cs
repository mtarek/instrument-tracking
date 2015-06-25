using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentTracking
{
    static class Util
    {
        public static int MaxIndex<T>(this IEnumerable<T> sequence)
    where T : IComparable<T>
        {
            int maxIndex = -1;
            T maxValue = default(T); // Immediately overwritten anyway

            int index = 0;
            foreach (T value in sequence)
            {
                if (value.CompareTo(maxValue) > 0 || maxIndex == -1)
                {
                    maxIndex = index;
                    maxValue = value;
                }
                index++;
            }
            return maxIndex;
        }

        public static int AbsMaxIndex(double[] sequence)
        {
            int maxIndex = 0;
            double maxValue = Math.Abs(sequence[0]);
            double tmp;

            int index = 0;
            for (int i = 1; i < sequence.Length; i++)
            {
                tmp = Math.Abs(sequence[i]);
                if ( tmp > maxValue)
                {
                    maxValue = tmp;
                    maxIndex = i;
                }

            }
                return maxIndex;
        }

        public static int[] GetZeroCrossings(double[] signal)
        {
            List<int> crossings = new List<int>();
            double avg = 0;
            bool negative = false;
            int limit = 0;
            int offset = 0;
            for (int i = 0; i < signal.Length; i++)
            {
                if (signal[i] < - 0.01)
                {
                    negative = true;
                    continue;
                }
                else if(signal[i] > 0.01)
                {
                    if (negative)
                    {

                        if (crossings.Count > 0) limit = crossings[crossings.Count - 1];
                        
                        for (int j = i; j > limit; j--)
                        {
                            if (signal[j] < 0)
                            {
                                if (limit == 0)
                                {
                                    offset = j;
                                }
                                crossings.Add((j + 1 - offset));
                                break;
                            }
                        }
                            
                    }

                    negative = false;
                }
            }

                 if ( crossings.Count == 0)
                {
                    /* probably dc  no  ZXs*/
                    for (int k = 0; k < 10; k++)
                    {
                        crossings.Add(k);

                    }

                }



            return crossings.ToArray();
        }
    }
}
