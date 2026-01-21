using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mkd2snesv2
{
    internal class MemoryAddresses
    {
        public static readonly Dictionary<string, uint> ntsc = new Dictionary<string, uint>
        {
            ["CurrentLapP1"] = 0x10C1, //going backwards lowers this value
            ["LapReachedP1"] = 0x10F9, //highest lap reached
            ["RaceTimer"] = 0x101, //highest lap reached
            ["CurrentCourse"] = 0x124,
            ["ttResults"] = 0x1D20, //final times on results screen
            ["ttLapTimes"] = 0xF33, //the race time as a lap was completed
            ["P1Racer"] = 0x1012, //what character player 1 is
            ["P2Racer"] = 0x1112, //what character player 2 is

            ["GameMode"] = 0x2C, //4 = TT
            ["ScreenMode"] = 0x36, //2 = in race
            ["pausa"] = 0x162, //00 = race, 02 = give up, 03 = pause menu
        };
    }
}
