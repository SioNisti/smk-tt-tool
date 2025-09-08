using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace smk_tt_tool
{
    public class CourseData
    {
        public int finishedraces { get; set; }
        public int attempts { get; set; }
        public PersonalRecords pr { get; set; } = new();
        public BestLaps bestlaps { get; set; } = new();
        public List<Race> races { get; set; } = new();
    }

    public class PersonalRecords
    {
        public int fivelap { get; set; }
        public int flap { get; set; }
    }

    public class BestLaps
    {
        public int lap1 { get; set; }
        public int lap2 { get; set; }
        public int lap3 { get; set; }
        public int lap4 { get; set; }
        public int lap5 { get; set; }
    }

    public class Race
    {
        public int id { get; set; }
        public string character { get; set; } = "";
        public DateTime date { get; set; }
        public int racetime { get; set; }
        public int lap1 { get; set; }
        public int lap2 { get; set; }
        public int lap3 { get; set; }
        public int lap4 { get; set; }
        public int lap5 { get; set; }
    }

}
