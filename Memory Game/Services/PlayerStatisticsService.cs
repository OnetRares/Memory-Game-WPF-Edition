using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Memory_Game.Model;
using System.IO;


namespace Memory_Game
{
    class PlayerStatisticsService
    {
        private static readonly string statsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PlayerStats.json");

        public static List<PlayerStatisticsModel> LoadStatistics()
        {
            if (!File.Exists(statsFilePath))
                return new List<PlayerStatisticsModel>();

            string json = File.ReadAllText(statsFilePath);
            return JsonSerializer.Deserialize<List<PlayerStatisticsModel>>(json) ?? new List<PlayerStatisticsModel>();
        }

        
        public static void SaveStatistics(List<PlayerStatisticsModel> statistics)
        {
            string json = JsonSerializer.Serialize(statistics, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(statsFilePath, json);
        }

      
        public static void UpdateStatistics(string username, bool won)
        {
            var stats = LoadStatistics();
            var playerStats = stats.FirstOrDefault(s => s.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (playerStats == null)
            {
                playerStats = new PlayerStatisticsModel { Username = username, GamesPlayed = 0, GamesWon = 0 };
                stats.Add(playerStats);
            }

            playerStats.GamesPlayed++;
            if (won)
                playerStats.GamesWon++;

            SaveStatistics(stats);
        }
        public static void DeleteUserStatistics(string username)
        {
            if (string.IsNullOrEmpty(username))
                return; 

            var stats = LoadStatistics();
            var playerStats = stats.FirstOrDefault(s => s.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (playerStats != null)
            {
                stats.Remove(playerStats);
                SaveStatistics(stats);
            }
        }

    }
}
