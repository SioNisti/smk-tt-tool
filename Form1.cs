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

namespace smk_tt_tool
{
    public partial class Form1 : Form
    {
        private Snessocket Snessocket;
        private Timer _update_timer;
        private bool isAttached = false;
        private string deviceUse = "";

        public Form1()
        {
            InitializeComponent();

            _update_timer = new Timer() { Interval = 11 };
            _update_timer.Tick += (sender, args) => ReadMemory();
            _update_timer.Enabled = true;

            Snessocket = new Snessocket();
            Snessocket.wsConnect();
        }
        int Normalize(int value)
        {
            return (value == 0xFF) ? 0 : value;
        }

        private void ReadMemory()
        {
            if (isAttached)
            {
                Debug.WriteLine("reading memory");
                var data = new byte[64];
                data = Snessocket.GetAddress((0xF50000 + MemoryAddresses.ntsc["ttLapTimes"]), (uint)30);

                // Lap 1
                int lap1cs = Normalize(data[0]);
                int lap1s = Normalize(data[1]);
                int lap1m = Normalize(data[3]);
                string formatted1 = $"{lap1m:X2}'{lap1s:X2}\"{lap1cs:X2}";

                // Lap 2
                int lap2cs = Normalize(data[6]);
                int lap2s = Normalize(data[7]);
                int lap2m = Normalize(data[9]);
                string formatted2 = $"{lap2m:X2}'{lap2s:X2}\"{lap2cs:X2}";

                // Lap 3
                int lap3cs = Normalize(data[12]);
                int lap3s = Normalize(data[13]);
                int lap3m = Normalize(data[15]);
                string formatted3 = $"{lap3m:X2}'{lap3s:X2}\"{lap3cs:X2}";

                // Lap 4
                int lap4cs = Normalize(data[18]);
                int lap4s = Normalize(data[19]);
                int lap4m = Normalize(data[21]);
                string formatted4 = $"{lap4m:X2}'{lap4s:X2}\"{lap4cs:X2}";

                // Lap 5
                int lap5cs = Normalize(data[24]);
                int lap5s = Normalize(data[25]);
                int lap5m = Normalize(data[27]);
                string formatted5 = $"{lap5m:X2}'{lap5s:X2}\"{lap5cs:X2}";

                // Convert formatted strings into centiseconds
                int cs1 = StrToCs(formatted1);
                int cs2 = StrToCs(formatted2);
                int cs3 = StrToCs(formatted3);
                int cs4 = StrToCs(formatted4);
                int cs5 = StrToCs(formatted5);

                // Calculate split times (clamped at 0)
                int lap1Split = Math.Max(0, cs1);
                int lap2Split = Math.Max(0, cs2 - cs1);
                int lap3Split = Math.Max(0, cs3 - cs2);
                int lap4Split = Math.Max(0, cs4 - cs3);
                int lap5Split = Math.Max(0, cs5 - cs4);

                label1.Text =
                    $"L1 {CsToStr(lap1Split)}\n" +
                    $"L2 {CsToStr(lap2Split)}\n" +
                    $"L3 {CsToStr(lap3Split)}\n" +
                    $"L4 {CsToStr(lap4Split)}\n" +
                    $"L5 {CsToStr(lap5Split)}\n\n" +
                    $"LA {formatted5}";
            }
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
            // Convert centiseconds into minutes, seconds, centiseconds
            int centiseconds = cs % 100;
            int totalSeconds = cs / 100;
            int seconds = totalSeconds % 60;
            int minutes = totalSeconds / 60;

            // Format minutes (always 2 digits)
            string minS = minutes.ToString("00") + "'";

            // Format seconds (always 2 digits)
            string secS = seconds.ToString("00") + "\"";

            // Format centiseconds (always 2 digits)
            string csS = centiseconds.ToString("00");

            return minS + secS + csS;
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

    }
}
