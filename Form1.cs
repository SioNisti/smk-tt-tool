using Microsoft.VisualBasic.Devices;
using mkd2snesv2;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Xml.Linq;

namespace smk_tt_tool
{
    public partial class Form1 : Form
    {
        //stuff so you can move the window around
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        //initializing variables
        private Snessocket Snessocket;
        private bool isAttached = false;
        private string deviceUse = "";
        private string[] courses = {
            "MC1", "DP1", "GV1", "BC1", "MC2",
            "CI1", "GV2", "DP2", "BC2", "MC3",
            "KB1", "CI2", "VL1", "BC3", "MC4",
            "DP3", "KB2", "GV3", "VL2", "RR"
        };
        private string currentCourse = "MC3";
        private string currentRegion = "NTSC";
        private bool JsonLock = true;
        public string toClipboard = "";

        //session variables
        public Dictionary<string, SessionData> sessionData = new();

        public static string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMK-TT-Tool");
        public string jsonPath = Path.Combine(folder, "data.json");
        //public string jsonPath = @"attempt-data.json";

        public Form1()
        {
            InitializeComponent();
            foreach (var course in courses)
            {
                sessionData[course] = new SessionData();
            }
        }
        //thing to convert given value to 0 if it's 0xFF (empty lap time)
        int Normalize(int value)
        {
            return (value == 0xFF) ? 0 : value;
        }
        //convert the bytes into a nice string
        public string BytesToStr(int cs, int s, int m)
        {
            int lapcs = Normalize(cs);
            int laps = Normalize(s);
            int lapm = Normalize(m);
            return $"{lapm:X}'{laps:X2}\"{lapcs:X2}";
        }
        //converts given string into centiseconds
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
        //convert centiseconds to a nice string
        string CsToStr(int cs)
        {
            int minutes = cs / 6000;
            int seconds = (cs / 100) % 60;
            int cs2 = cs % 100;

            return $"{minutes}'{seconds:00}\"{cs2:00}";
        }
        //check if the json exists and that it's good.
        private void CheckJson()
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (!File.Exists(jsonPath))
            {
                File.WriteAllText(jsonPath, "{}");
            }

            //Dictionary<string, CourseData> allData;
            Dictionary<string, Dictionary<string, CourseData>> allData;

            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                //allData = JsonSerializer.Deserialize<Dictionary<string, CourseData>>(json) ?? new Dictionary<string, CourseData>();
                allData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, CourseData>>>(json) ?? new Dictionary<string, Dictionary<string, CourseData>>();

            }
            else
            {
                //allData = new Dictionary<string, CourseData>();
                allData = new Dictionary<string, Dictionary<string, CourseData>>();
            }

            //check that all the courses are in the json. if not, add them.
            bool update = false;
            string[] regions = { "NTSC", "PAL" };

            foreach (string region in regions)
            {
                if (!allData.ContainsKey(region))
                {
                    allData[region] = new Dictionary<string, CourseData>();
                    update = true;
                }

                foreach (var course in courses)
                {
                    if (!allData[region].ContainsKey(course))
                    {
                        allData[region][course] = new CourseData
                        {
                            Finishedraces = 0,
                            Attempts = 0,
                            Pr = new PersonalRecords { Fivelap = 0, Flap = 0 },
                            Bestlaps = [0, 0, 0, 0, 0],
                            Races = new List<Race>()
                        };
                        update = true;
                    }
                }
            }

            if (update)
            {
                string json = JsonSerializer.Serialize(allData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(jsonPath, json);
            }
        }

        private void AddRaceToJson(string character, int racetime, int lap1, int lap2, int lap3, int lap4, int lap5)
        {
            //SESSION
            var session = sessionData[currentCourse];
            session.Attempts++;

            var json = File.ReadAllText(jsonPath);
            //var data = JsonSerializer.Deserialize<Dictionary<string, CourseData>>(json);
            var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, CourseData>>>(json);

            int[] laps = { lap1, lap2, lap3, lap4, lap5 };
            int flap = laps.Where(l => l > 0).DefaultIfEmpty(0).Min();

            var courseData = data[currentRegion][currentCourse];
            var attempts = courseData.Attempts;
            courseData.Races.Add(new Race
            {
                Id = ++attempts,
                Character = character,
                Date = DateTime.Now,
                Racetime = racetime,
                Laps = [lap1, lap2, lap3, lap4, lap5]
            });
            courseData.Attempts = attempts;

            //session save
            if (lap5 > 0 && (racetime < session.FiveLap || session.FiveLap == 0))
            {
                session.FiveLap = racetime;
            }
            if (flap < session.Flap || session.Flap == 0)
            {
                session.Flap = flap;
            }

            //var debugInfo = JsonSerializer.Serialize(sessionData[currentCourse], new JsonSerializerOptions { WriteIndented = true });
            //Debug.WriteLine($"Stats for {currentCourse}: {debugInfo}");

            //check if race was finished to update count. and potential prs.
            if (lap5 > 0)
            {
                courseData.Finishedraces++;
                session.FinishedRaces++;

                //update best lap splits
                for (int i = 0; i < courseData.Bestlaps.Length; i++)
                {
                    //if no best lap exists (=0) or is lower than new lap
                    if (courseData.Bestlaps[i] == 0)
                    {
                        courseData.Bestlaps[i] = attempts;
                    }
                    else if (laps[i] < courseData.Races[courseData.Bestlaps[i] - 1].Laps[i])
                    {
                        courseData.Bestlaps[i] = attempts;
                    }
                }

                //update fivelap
                if (courseData.Pr.Fivelap != 0)
                {
                    if (racetime < courseData.Races[courseData.Pr.Fivelap - 1].Racetime)
                    {
                        courseData.Pr.Fivelap = attempts;
                    }
                }
                else
                {
                    courseData.Pr.Fivelap = attempts;
                }

                //update flap
                if (courseData.Pr.Flap != 0)
                {
                    if (laps.Min() < courseData.Races[courseData.Pr.Flap - 1].Laps.Min())
                    {
                        courseData.Pr.Flap = attempts;
                    }
                }
                else
                {
                    courseData.Pr.Flap = attempts;
                }
            }

            json = JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(jsonPath, json);
        }

        private void ReadMemory()
        {
            if (isAttached)
            {
                //lap times from ram
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
                int[] LapSplits = { Math.Max(0, cs1), Math.Max(0, cs2 - cs1), Math.Max(0, cs3 - cs2), Math.Max(0, cs4 - cs3), Math.Max(0, cs5 - cs4) };

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
                currentCourse = TrackNames.Map[data[0]];

                //get the addresses we check to determine if we want to save the json
                data = new byte[1];
                data = Snessocket.GetAddress((0xF50000 + MemoryAddresses.ntsc["GameMode"]), (uint)1);
                int gameMode = data[0];
                data = new byte[1];
                data = Snessocket.GetAddress((0xF50000 + MemoryAddresses.ntsc["ScreenMode"]), (uint)1);
                int screenMode = data[0];
                data = new byte[1];
                data = Snessocket.GetAddress((0xF50000 + MemoryAddresses.ntsc["pausa"]), (uint)1);
                int pauseMode = data[0];

                if (lapreached < 6 && formatted5 == "0'00\"00")
                {
                    formatted5 = totalTime;
                }

                if (gameMode == 0x04 && screenMode == 0x02 && (pauseMode == 0x03 || LapSplits[4] > 0 && lapreached == 6) && !JsonLock)
                {
                    JsonLock = true;
                    //Debug.WriteLine($"saved race and lock json ({JsonLock})");
                    int finishtime = cs5;
                    if (finishtime == 0)
                    {
                        finishtime = StrToCs(totalTime);
                    }

                    AddRaceToJson(racer, finishtime, LapSplits[0], LapSplits[1], LapSplits[2], LapSplits[3], LapSplits[4]);
                }
                else if (gameMode == 0x04 && screenMode == 0x02 && pauseMode == 0x00 && lapreached != 6 && JsonLock)
                {
                    JsonLock = false;
                    //Debug.WriteLine($"open json lock ({JsonLock})");
                }

                this.Invoke((Action)(() =>
                {
                    raceSplitsLabel.Text = $"L1 {CsToStr(LapSplits[0])}\nL2 {CsToStr(LapSplits[1])}\nL3 {CsToStr(LapSplits[2])}\nL4 {CsToStr(LapSplits[3])}\nL5 {CsToStr(LapSplits[4])}\nTOTAL {formatted5}";
                    courseLabel.Text = $"{currentCourse}";
                    attemptLabel.Text = getAttempts(currentCourse);
                    prLabel.Text = $"5lap: {get5lapPrInfo(currentCourse)}\nFlap: {getFlapPrInfo(currentCourse)}";

                    //session
                    sessionAttemptLabel.Text = $"{sessionData[currentCourse].FinishedRaces}/{sessionData[currentCourse].Attempts}";
                    sessionBestsLabel.Text = $"Now:\n5lap: {CsToStr(sessionData[currentCourse].FiveLap)}\nFlap: {CsToStr(sessionData[currentCourse].Flap)}";
                }));

                toClipboard = $"{CsToStr(LapSplits[0])} {CsToStr(LapSplits[1])} {CsToStr(LapSplits[2])} {CsToStr(LapSplits[3])} {CsToStr(LapSplits[4])}";
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Snessocket = new Snessocket();
            Snessocket.wsConnect();

            CheckJson();

            //thread memory reading and such so the gui wont get interrupted constantly.
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    ReadMemory();

                    await Task.Delay(1);
                }
            });
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
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
                    appContextMenu.Items[0].Enabled = false; //gray out "connect"
                    appContextMenu.Items[1].Enabled = true; //ungray out "copy times"
                }
            }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(toClipboard);
        }

        public bool validateCourse(string course)
        {
            int pos = Array.IndexOf(courses, course);
            if (pos > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string getAttempts(string course)
        {
            if (validateCourse(course))
            {
                var json = File.ReadAllText(jsonPath);
                //var data = JsonSerializer.Deserialize<Dictionary<string, CourseData>>(json);
                var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, CourseData>>>(json);

                return $"{data[currentRegion][course].Finishedraces}/{data[currentRegion][course].Attempts}";
            }
            else
            {
                return "0/0";
            }
        }

        public string get5lapPrInfo(string course)
        {
            if (validateCourse(course))
            {
                var json = File.ReadAllText(jsonPath);
                //var data = JsonSerializer.Deserialize<Dictionary<string, CourseData>>(json);
                var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, CourseData>>>(json);
                var courseData = data[currentRegion][course];
                int id = courseData.Pr.Fivelap;
                Race prRace = courseData.Races.FirstOrDefault(r => r.Id == id);

                if (prRace == null) return "0'00\"00";

                return CsToStr(prRace.Racetime);
            }
            else
            {
                return "0'00\"00";
            }
        }

        public string getFlapPrInfo(string course)
        {
            if (validateCourse(course))
            {
                var json = File.ReadAllText(jsonPath);
                //var data = JsonSerializer.Deserialize<Dictionary<string, CourseData>>(json);
                var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, CourseData>>>(json);
                var courseData = data[currentRegion][course];
                int id = courseData.Pr.Flap;
                Race prRace = courseData.Races.FirstOrDefault(r => r.Id == id);
                if (prRace == null) return "0'00\"00";
                List<int> prLaps = prRace.Laps.ToList();

                return CsToStr(prLaps.Min());
            }
            else
            {
                return "0'00\"00";
            }
        }

        private void palToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (palToolStripMenuItem.Checked)
            {
                currentRegion = "PAL";
            } else
            {
                currentRegion = "NTSC";
            }
        }
    }
}
