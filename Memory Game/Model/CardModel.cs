﻿using Memory_Game.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_Game.Model
{
        public class CardModel:BaseViewModel
        {
           
            public string ImagePath { get; set; }

         
            public bool IsRevealed { get; set; }

         
            public bool IsMatched { get; set; }


            public string BackImagePath { get; set; }

          
            public string DisplayImage => IsRevealed || IsMatched ? ImagePath : BackImagePath;
        }
    }

