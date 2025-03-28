using Memory_Game.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game.Model
{
    public class GameStateModel:BaseViewModel
    {
        public string Username { get; set; }
        public string Category { get; set; }
        public string GameMode { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int TimeRemaining { get; set; }
        public List<CardState> Cards { get; set; }
    }

    public class CardState:BaseViewModel
    {
        public string ImagePath { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsMatched { get; set; }
    }
    public class SavedGameItem:BaseViewModel
    {
        public string Filename { get; set; }
        public string FilePath { get; set; }
    }
}
