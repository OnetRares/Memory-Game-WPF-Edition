using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Memory_Game
{
    public static class GameStateStorage
    {
        private static string GetUserSaveFolder(string username)
        {
            string baseFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MemoryGame", username);
            if (!Directory.Exists(baseFolder))
                Directory.CreateDirectory(baseFolder);

            string savedGamesFolder = Path.Combine(baseFolder, "SavedGames");
            if (!Directory.Exists(savedGamesFolder))
                Directory.CreateDirectory(savedGamesFolder);

            return savedGamesFolder;
        }

        public static void SaveGame(GameState state)
        {
            string folder = GetUserSaveFolder(state.Username);
            string filename = $"Game_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            string fullPath = Path.Combine(folder, filename);
            string json = JsonConvert.SerializeObject(state, Formatting.Indented);

            File.WriteAllText(fullPath, json);

            string txtFilename = $"SavedGame_{state.Username}.txt";
            string txtFilePath = Path.Combine(folder, txtFilename);
            string entry = $"Filename: {filename}{Environment.NewLine}{json}{Environment.NewLine}---------------------{Environment.NewLine}";
            File.AppendAllText(txtFilePath, entry);
        }

        public static List<string> GetSavedGameFiles(string username)
        {
            string folder = GetUserSaveFolder(username);
            if (Directory.Exists(folder))
                return Directory.GetFiles(folder, "*.json").ToList();
            return new List<string>();
        }

        public static GameState LoadGame(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<GameState>(json);
        }
    }
}
