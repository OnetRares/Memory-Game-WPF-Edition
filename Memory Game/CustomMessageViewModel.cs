using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Memory_Game
{
    public static class CustomMessageViewModel
    {
        
        public static void Show(string message, string title = "Message")
        {
            CustomMessageWindow customWindow = new CustomMessageWindow
            {
                Title = title,
                Message = message
            };
            customWindow.ShowDialog();
        }


        public static void ShowTimeMessage(int time)
        {
            string message = $"Game start with {time} seconds";
            Show(message, "Time");
        }

        public static void ShowTimeEroorMessage()
        {
            string message = "The entered time is not valid";
            Show(message, "Error Time");
        }

        public static void ShowTimeCancelMessage()
        {
            string message = "Time limit selection canceled.";
            Show(message, "Cancel Time");
        }

        public static void ShowSelectedCategoryMessage(string selectedCategory)
        {
            string message = $"Selected category: {selectedCategory}";
            Show(message, "Category");
        }
          public static void ShowSelectedCategoryErrorMessage()
        {
            string message = "You must select a category";
            Show(message, "Category Error");
        }

        public static void ShowStartNewGameErrorMessage()
        {
            string message = "Please select an image category and game mode (Standard or Custom) before starting.";
            Show(message, "StartGame Error");
        }

        public static void ShowCurrentSaveGameErrorMessage()
        {
            string message = "There is no game in progress to save..";
            Show(message,"SaveGame Error");
        }
        public static void ShowSaveGameMessage()
        {
            string message = "Game saved successfully.";
            Show(message, "SaveGame");
        }
        public static void ShowSaveErrorMessage(Exception ex)
        {
            string message = $"Error saving: {ex.Message}";
            Show(message, "SaveGame Error");
        }

        public static void ShowStandardGameMessage()
        {
            string message = "Standard mode selected: The board will be 4x4.";
            Show(message, "StandardMode");
        }
        public static void ShowCustomGameMessage(int selectedColumns, int selectedRows)
        {
            string message = $"Custom mode selected: The board will be {selectedColumns}x{selectedRows}.";
            Show(message, "CustomMode");
        }

        public static void ShowCustomErrorGameMessage()
        {
            string message = "The board dimensions have not been set.";
            Show(message, "CustomMode Error");
        }

        public static void ShowAboutInfoMessage()
        {
            string message = "Memory Game\nDeveloper: Onet Rares-Nicolae\nEmail: rares.onet@student.com\nGroup: 10LF233\nSpecialization: Computer Science";
            Show(message, "About");
        }


        
        public static void ShowInvalidDimensionMessage()
        {
            string message = "The product of rows and columns must be even.";
            Show(message, "Invalid Dimension");
        }



        public static void ShowDeleteUserErrorMessage(Exception ex)
        {
            string message = $"Error deleting user folder: {ex.Message}";
            Show(message, "Delete Error");
        }

        public static void ShowInvalidUserMessage()
        {
            string message = "Please enter a valid username.";
            Show(message, "Invalid Username");
        }
        public static void ShowUserAlreadyExistMessage()
        {
            string message = "This username already exists. Please choose another one.";
            Show(message, "User Already Exist");
        }
        public static void ShowProfileFolderErrorMessage(string profileFolder)
        {
            string message = $"Profile folder not found: {profileFolder}";
            Show(message, "Profile folder Error");
        }

        public static void ShowErrorSavingUsersMessage(Exception ex)
        {
            string message=$"Error saving users: {ex.Message}";
            Show(message, "Error");
        }


        public static void ShowLoseGameMessage()
        {
            string message = "Time is up! You lost the game.";
            Show(message, "Game Over");
        }

        public static void ShowWinGameMessage(string username,string timer )
        {
            string message = $"Congratulations {username} ! You finished the game in {timer}";
        }
    }

}
