using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game
{
        public class CardModel:BaseViewModel
        {
           
            public string ImagePath { get; set; }

         
            public bool IsRevealed { get; set; }

         
            public bool IsMatched { get; set; }

            public string BackImagePath { get; set; } = "Images/back.jpg";

          
            public string DisplayImage => (IsRevealed || IsMatched) ? ImagePath : BackImagePath;
        }
    }

