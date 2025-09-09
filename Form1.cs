using mkd2snesv2;
using System.Diagnostics;
using System.Text.Json;

namespace smk_tt_tool
{
    public partial class Form1 : Form
    {
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
        private int lastRaceOptions = 0xFF; //set to 0 for testing.

        public Form1()
        {
            InitializeComponent();

            Snessocket = new Snessocket();
            Snessocket.wsConnect();

            CheckJson();
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

        private void CheckJson()
        {
            //check if the jsons exist. if they dont, make them.

            foreach (var course in courses)
            {
                string filePath = $"data/{course}.json";
                if (!Directory.Exists("data/"))
                {
                    Directory.CreateDirectory("data");
                }

                if (!File.Exists(filePath))
                {
                    var defaultData = new Dictionary<string, CourseData>
                    {
                        [course] = new CourseData
                        {
                            finishedraces = 0,
                            attempts = 0,
                            pr = new PersonalRecords { fivelap = 0, flap = 0 },
                            bestlaps = new BestLaps { lap1 = 0, lap2 = 0, lap3 = 0, lap4 = 0, lap5 = 0 },
                            races = new List<Race>()
                        }
                    };

                    string json = JsonSerializer.Serialize(defaultData, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(filePath, json);
                }
            }
        }

        private void AddRaceToJson(string character, int racetime, int lap1, int lap2, int lap3, int lap4, int lap5)
        {
            /*
             currently this crashes on the lap pr check if the json doesnt have any recorded races.

             needs fixing.
             */

            var json = File.ReadAllText($"data/{currentCourse}.json");
            var data = JsonSerializer.Deserialize<Dictionary<string, CourseData>>(json);

            int[] laps = { lap1, lap2, lap3, lap4, lap5 };

            var courseData = data[currentCourse];
            var attempts = courseData.attempts;
            courseData.races.Add(new Race
            {
                id = ++attempts,
                character = character,
                date = DateTime.Now,
                racetime = racetime,
                lap1 = lap1,
                lap2 = lap2,
                lap3 = lap3,
                lap4 = lap4,
                lap5 = lap5
            });
            courseData.attempts = attempts;

            //check if race was finished to update count. and potential prs.
            if (lap5 > 0)
            {
                courseData.finishedraces++;

                //update best lap splits
                for (int i = 0; i < 5; i++)
                {
                    //if no best lap exists (=0) or is lower than new lap
                    if (courseData.bestlaps[i] == 0)
                    {
                        courseData.bestlaps[i] = attempts;
                    }
                    else if (laps[i] < courseData.races[courseData.bestlaps[i] - 1])
                    {
                        courseData.bestlaps[i] = attempts;
                    }
                }

                //update fivelap
                if (courseData.pr.fivelap != 0)
                {
                    if (racetime < courseData.races[courseData.pr.fivelap - 1].racetime)
                    {
                        courseData.pr.fivelap = attempts;
                    }
                } else
                {
                    courseData.pr.fivelap = attempts;
                }

                //update flap
                if (courseData.pr.flap != 0)
                {
                    //get all laps from the race with the flap
                    int[] lapsfromflaprace = { courseData.races[courseData.pr.flap - 1].lap1, courseData.races[courseData.pr.flap - 1].lap2, courseData.races[courseData.pr.flap - 1].lap3, courseData.races[courseData.pr.flap - 1].lap4, courseData.races[courseData.pr.flap - 1].lap5 };

                    if (laps.Min() < lapsfromflaprace.Min())
                    {
                        courseData.pr.flap = attempts;
                    }
                }
                else
                {
                    courseData.pr.flap = attempts;
                }
            }

            json = JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText($"data/{currentCourse}.json", json);
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
                currentCourse = TrackNames.Map[data[0]];

                //retry/end screen
                data = new byte[1];
                data = Snessocket.GetAddress((0xF50000 + MemoryAddresses.ntsc["InRaceOptions"]), (uint)1);
                int RaceOptions = data[0];

                if (lapreached < 6 && formatted5 == "0'00\"00")
                {
                    formatted5 = totalTime;
                }

                //check if at the retry screen is up and at least one lap was finished.
                if (lastRaceOptions == 0x00 && RaceOptions == 0xFF && lapreached > 1)
                {
                    int finishtime = cs5;
                    if (finishtime == 0)
                    {
                        finishtime = StrToCs(totalTime);
                    }

                    AddRaceToJson(racer, finishtime, lap1Split, lap2Split, lap3Split, lap4Split, lap5Split);
                }

                this.Invoke((Action)(() =>
                {
                    label4.Text = $"TRACK {currentCourse}";
                    label3.Text = $"RACER {racer}";
                    label2.Text = $"LAP {Math.Clamp(lapreached,0,5)}";
                    label1.Text =$"L1 {CsToStr(lap1Split)}\nL2 {CsToStr(lap2Split)}\nL3 {CsToStr(lap3Split)}\nL4 {CsToStr(lap4Split)}\nL5 {CsToStr(lap5Split)}\n\nTOTAL {formatted5}";
                }));

                lastRaceOptions = RaceOptions;
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

                    await Task.Delay(10);
                }
            });
        }
    }
}
