using Memory_Game.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game.Model
{
    public class PlayerStatisticsModel:BaseViewModel
    {
        public string Username { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }

        public override string ToString()
        {
            return $"{Username} – {GamesPlayed} games played – {GamesWon} games won";
        }
    }
}
