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
    public class Note
    {
        public char note; //letter - if symbolizing rest then note = 'R'
        public int octave; //number from 1-8 (possible human vocal/listening range)
        public long duration; //number of midi subdivisions
        public int sharp; //-1 flat, 0 normal, 1 sharp

        public Note()
        {
            note = 'R';
            octave = 0;
            duration = 0;
            sharp = 0;
        }

        public Note(char _note, int _sharp, int _octave, long _duration)
        {
            note = _note;
            octave = _octave;
            duration = _duration;
            sharp = _sharp;
        }

        public override string ToString()
        {
            return note.ToString() +" " + octave + " " + sharp + "/L:" + duration;
        }
        
        /***
        Takes this note and sets the best note, octave, and sharp according to frequency
        ***/
        public void frequencyToNote(double fn)
        { //fn is frequency in Hz
          //fn = f0 * a^n where n is the amount of halfsteps from f0
            if (fn > 3400 | fn < 300) { note = 'R'; }
            else {
                double f0 = 440.0; //A4 is 440 Hz
                double a = Math.Pow(2.0, 1 / 12.0);
                double fn0 = fn / f0;
                double n = Math.Log((fn / f0), a);
                Console.WriteLine("a:" + a);
                int halfsteps = (int)Math.Round(n);
                int stepsFromOctave = halfsteps % 12;
                octave = 4 + (halfsteps - 3) / 12;
                switch (stepsFromOctave)
                {
                    case 0:
                        note = 'A'; sharp = 0; break;
                    case 1:
                        note = 'A'; sharp = 1; break;
                    case 2:
                        note = 'B'; sharp = 0; break;
                    case 3:
                        note = 'C'; sharp = 0; break;
                    case 4:
                        note = 'C'; sharp = 1; break;
                    case 5:
                        note = 'D'; sharp = 0; break;
                    case 6:
                        note = 'D'; sharp = 1; break;
                    case 7:
                        note = 'E'; sharp = 0; break;
                    case 8:
                        note = 'F'; sharp = 0; break;
                    case 9:
                        note = 'F'; sharp = 1; break;
                    case 10:
                        note = 'G'; sharp = 0; break;
                    case 11:
                        note = 'G'; sharp = 1; break;
                }
            }
        }

        public char getNote()
        {
            return note;
        }
    }
}