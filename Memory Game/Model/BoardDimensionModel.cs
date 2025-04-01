using Memory_Game.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game
{
    public class BoardDimensionsModel:BaseViewModel
    {
        public int Rows { get; set; }
        public int Columns { get; set; }

        public BoardDimensionsModel(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

       
        public bool IsValid()
        {
            return (Rows * Columns) % 2 == 0;
        }

        public override string ToString()
        {
            return $"{Columns} x {Rows}";
        }
    }
}
