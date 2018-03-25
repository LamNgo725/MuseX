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
    public class Sequence
    {
        public List<Note> notes;

        public Sequence()
        {
            notes = new List<Note>();
        }

        public void addNote(Note n)
        {
            notes.Add(n);
        }

        public int getLength()
        {
            return notes.Count();
        }

        public Note returnNote(int location)
        {
            return notes[location];
        }

        public void compressSequence()
        {
            List<Note> compressed = new List<Note>();
            char c_note = 'R';
            int c_octave = 0;
            long duration = 0;
            int c_sharp = 0;
            foreach (var n in notes)
            {
                if (n.note != c_note || n.octave != c_octave || n.sharp != c_sharp)
                {
                    if (duration != 0)
                    {
                        compressed.Add(new Note(c_note, c_sharp, c_octave, duration));

                        duration = 0;
                    }
                    c_note = n.note;
                    c_octave = n.octave;
                    c_sharp = n.sharp;
                }
                duration++;
            }
            notes = compressed;
        }

        public void sendToMIDI()
        {
            compressSequence();
        }

        public string head()
        {
            /*String result = "";
            for (int i = 0; i < 6; i++)
            {
                result += notes[i].ToString() + ", ";
            }*/
            return notes[0].ToString();
        }
    }
}