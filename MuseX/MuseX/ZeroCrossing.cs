using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MuseX
{
    public class ZeroCrossing
    {

        /**
         * calculate frequency using zero crossings
         */
        public static int calculate(int sampleRate, short[] audioData)
        {
            int numSamples = audioData.Length;
            int numCrossing = 0;
            for (int p = 0; p < numSamples - 1; p++)
            {
                if ((audioData[p] > 0 && audioData[p + 1] <= 0) ||
                    (audioData[p] < 0 && audioData[p + 1] >= 0))
                {
                    numCrossing++;
                }
            }

            float numSecondsRecorded = (float)numSamples / (float)sampleRate;
            float numCycles = numCrossing / 2;
            float frequency = numCycles / numSecondsRecorded;

            return (int)frequency;
        }
    }
}