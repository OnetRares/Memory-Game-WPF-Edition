using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game
{
    public class GameState
    {
        public string Username { get; set; }
        public string Category { get; set; }
        public string GameMode { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int TimeRemaining { get; set; }
        public List<CardState> Cards { get; set; }
    }

    public class CardState
    {
        public string ImagePath { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsMatched { get; set; }
    }
}
