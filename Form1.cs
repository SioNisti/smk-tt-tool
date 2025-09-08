using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;
using mkd2snesv2;
using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

/*
 json format for locally saved times.
ids for best laps and prs might be better for easy lookups.

{
    "MC1": {
        "races": [
            {
                "id": 1,
                "character": "bowser"
            },
            {
                "id": 2,
                "character": "toad"
            }
        ],
        "best laps": {
            "lap1": 22860,
            "lap2": 22860,
            "lap3": 22860,
            "lap4": 22860,
            "lap5": 22860
        },
        "pr": {
            "5lap": 97370,
            "flap": 1620
        }
    }
}
 */

namespace smk_tt_tool
{
    public partial class Form1 : Form
    {
        private Snessocket Snessocket;
        private bool isAttached = false;
        private string deviceUse = "";

        public Form1()
        {
            InitializeComponent();

            Snessocket = new Snessocket();
            Snessocket.wsConnect();
        }
        int Normalize(int value)
        {
            return (value == 0xFF) ? 0 : value;
        }

        public string BytesToStr(int cs, int s, int m)
        {
            int lapcs = Normalize(cs);
            int laps = Normalize(s);
            int lapm = Normalize(m);
            return $"{lapm:X}'{laps:X2}\"{lapcs:X2}";
        }
        int StrToCs(string timeString)
        {
            int total = 0;

            var parts = timeString.Split('\'');
            int minutes = Int32.Parse(parts[0]);
            var secParts = parts[1].Split('"');
            int seconds = Int32.Parse(secParts[0]);
            int centiseconds = Int32.Parse(secParts[1]);

            total += minutes * 6000;      // 1 minute = 60s = 6000cs
            total += seconds * 100;       // 1 second = 100cs
            total += centiseconds;

            return total;
        }

        string CsToStr(int cs)
        {
            int minutes = cs / 6000;
            int seconds = (cs / 100) % 60;
            int cs2 = cs % 100;

            return $"{minutes}'{seconds:00}\"{cs2:00}";
        }

        private void ReadMemory()
        {
            if (isAttached)
            {
                //lap times
                var data = new byte[30];
                data = Snessocket.GetAddress((0xF50000 + MemoryAddresses.ntsc["ttLapTimes"]), (uint)30);
                
                // Lap 1
                string formatted1 = BytesToStr(data[0], data[1], data[3]);
                int cs1 = StrToCs(formatted1);

                // Lap 2
                string formatted2 = BytesToStr(data[6], data[7], data[9]);
                int cs2 = StrToCs(formatted2);

                // Lap 3
                string formatted3 = BytesToStr(data[12], data[13], data[15]);
                int cs3 = StrToCs(formatted3);

                // Lap 4
                string formatted4 = BytesToStr(data[18], data[19], data[21]);
                int cs4 = StrToCs(formatted4);

                // Lap 5
                string formatted5 = BytesToStr(data[24], data[25], data[27]);
                int cs5 = StrToCs(formatted5);

                // Calculate split times (clamped at 0)
                int lap1Split = Math.Max(0, cs1);
                int lap2Split = Math.Max(0, cs2 - cs1);
                int lap3Split = Math.Max(0, cs3 - cs2);
                int lap4Split = Math.Max(0, cs4 - cs3);
                int lap5Split = Math.Max(0, cs5 - cs4);

                //lap count
                data = new byte[1];
                data = Snessocket.GetAddress((0xF50000 + MemoryAddresses.ntsc["LapReachedP1"]), (uint)1);

                int lapreached = data[0] - 127;

                //race timer
                data = new byte[10];
                data = Snessocket.GetAddress((0xF50000 + MemoryAddresses.ntsc["RaceTimer"]), (uint)10);
                string totalTime = BytesToStr(data[0], data[1], data[3]);

                //player 1 racer
                data = new byte[1];
                data = Snessocket.GetAddress((0xF50000 + MemoryAddresses.ntsc["P1Racer"]), (uint)1);
                string racer = DriverNames.Map[data[0]];

                //current course
                data = new byte[1];
                data = Snessocket.GetAddress((0xF50000 + MemoryAddresses.ntsc["CurrentCourse"]), (uint)1);
                string course = TrackNames.Map[data[0]];

                if (lapreached < 6 && formatted5 == "0'00\"00")
                {
                    formatted5 = totalTime;
                }

                this.Invoke((Action)(() =>
                {
                    label4.Text = $"TRACK {course}";
                    label3.Text = $"RACER {racer}";
                    label2.Text = $"LAP {Math.Clamp(lapreached,0,5)}";
                    label1.Text =$"L1 {CsToStr(lap1Split)}\nL2 {CsToStr(lap2Split)}\nL3 {CsToStr(lap3Split)}\nL4 {CsToStr(lap4Split)}\nL5 {CsToStr(lap5Split)}\n\nTOTAL {formatted5}";
                }));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Snessocket.Connected() && !isAttached)
            {
                string[] devices = Snessocket.GetDevices();
                deviceUse = devices[0];

                Snessocket.Attach(deviceUse);
                Snessocket.Name("smk-tt-tool");
                string[] infos = Snessocket.GetInfo();
                foreach (string info in infos)
                {
                    Debug.WriteLine(info);
                }
                if (infos.Length > 0)
                {
                    isAttached = true;
                    button1.Visible = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //thread memory reading and such so the gui wont get interrupted constantly.
            Task.Run(async () =>
            {
                while (true)
                {
                    ReadMemory();

                    await Task.Delay(1);
                }
            });
        }
    }
}
