using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Linq;
using Android.Media;
using System;
using System.Threading.Tasks;
using SkiaSharp;
using System.Threading;
using System.IO;

namespace MuseX
{
    [Activity(Label = "MuseX", MainLauncher = true)]
    public class MainActivity : Activity
    {
        public Boolean recording = false;
        public Sequence notes;
        AudioRecord audRecorder;
        public static int SAMPLE_RATE = 48000;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Button record = FindViewById<Button>(Resource.Id.button1);
            record.Click += buttonRecordClick;
            notes = new Sequence();
            
        }

        private void buttonRecordClick(object sender, System.EventArgs e)
        {
            recording = !recording;
            TextView debug = FindViewById<TextView>(Resource.Id.debugText);

            if (recording)
            {   //debugging
                Button record = FindViewById<Button>(Resource.Id.button1);
                record.Text = "Recording";
                recordThread();
                //Thread t = new Thread(recordThread);
                //t.Start();
            }
            else
            {
                audRecorder.Stop();
                debug = FindViewById<TextView>(Resource.Id.debugText);
                debug.Text += "\nDone: " + notes.head();
                notes.compressSequence();
            }

        }

        void recordThread()
        {
            EditText bpm = FindViewById<EditText>(Resource.Id.bpm);
            double bufferTimeInSeconds = (60.0/(Int32.Parse(bpm.Text)))/4; //16th note is shortest possible subdivision atm
            int bufferSize = (int)(bufferTimeInSeconds * 2 * SAMPLE_RATE + 0.5);

            //Start buffer
            short[] audioBuffer = new short[bufferSize];
            audRecorder = new AudioRecord(
              // Hardware source of recording.
              AudioSource.Mic,
              // Frequency
              SAMPLE_RATE, //8000Hz
                           // Mono or stereo
              ChannelIn.Mono,
              // Audio encoding
              Android.Media.Encoding.Pcm16bit,
              // Length of the audio clip.
              audioBuffer.Length * 2
            );

            TextView debug = FindViewById<TextView>(Resource.Id.debugText);
            audRecorder.StartRecording();
            debug.Text = bufferTimeInSeconds + ", " + SAMPLE_RATE + ", "+ audRecorder.Read(audioBuffer, 0, audioBuffer.Length);
            ProcessBuffer(audioBuffer);
            /*
            Thread.CurrentThread.IsBackground = true;
            while (recording)
            {
                try
                {
                    audRecorder.Read(audioBuffer, 0, audioBuffer.Length);
                    ProcessBuffer(audioBuffer);
                    audioBuffer = new short[bufferSize];
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex.Message);
                    break;
                }
            }*/
        }


        void ProcessBuffer(short[] audioBuffer)
        {
            TextView debug = FindViewById<TextView>(Resource.Id.debugText);
            int frequency = ZeroCrossing.calculate(SAMPLE_RATE, audioBuffer);
            debug.Text += "\nFreq: " + frequency;
            Note n = new Note();
            n.frequencyToNote(frequency);
            n.duration = 1;
            notes.addNote(n);

            string sum = "";
            foreach(short i in audioBuffer)
            {
                sum += i+",";
            }

        }
    }
}

