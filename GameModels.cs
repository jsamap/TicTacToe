using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TicTacToeApp
{
    // Mark position in Board
    public struct BoardPosition
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public BoardPosition(int row, int col)
        {
            Row = row;
            Column = col;
        }
    }

    // Scores saved as one with StructLayout
    [StructLayout(LayoutKind.Explicit)]
    public struct ScoreUnion
    {
        [FieldOffset(0)] public int TotalWins;
        [FieldOffset(0)] public int RawPoints;
    }

    // Player class with basic attributes and scores
    public class Player
    {
        public string Name { get; set; }
        public char Symbol { get; set; }
        public ScoreUnion Score;

        public Player(string name, char symbol)
        {
            Name = name;
            Symbol = symbol;
            Score = new ScoreUnion();
        }

        public string GetPlayerInfo()
        {
            return $"{Name} ({Symbol}) - Wins: {Score.TotalWins}";
        }
    }

    // File read and write logic for scores
    public static class ScoreManager
    {
        private static readonly string FilePath = "scores.txt";

        // Save scores to file: Format "Player1Wins,Player2Wins"
        public static void SaveScores(int playerXWins, int playerOWins)
        {
            try
            {
                string content = $"{playerXWins},{playerOWins}";
                File.WriteAllText(FilePath, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving scores: {ex.Message}");
            }
        }

        // Load scores from file
        public static (int playerXWins, int playerOWins) LoadScores()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string content = File.ReadAllText(FilePath);
                    string[] parts = content.Split(',');
                    
                    if (parts.Length == 2 && 
                        int.TryParse(parts[0], out int xWins) && 
                        int.TryParse(parts[1], out int oWins))
                    {
                        return (xWins, oWins);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading scores: {ex.Message}");
            }

            return (0, 0); // Default scores if file fails to load
        }
    }
}