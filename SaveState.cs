using System;
using System.Collections.Generic;
using System.Text;

namespace TrexRunner
{
    // LUU TRANG THAI GAME: LUU TRU DIEM CAO VA THOI DIEM DAT DIEM CAO
    [Serializable]
    public class SaveState
    {
        public int Highscore { get; set; }
        public DateTime HighscoreDate { get; set; }
    }
}