using System.Collections.Generic;

namespace XamarinGameMikes
{
    public class GameData{
        public int HighScore;
        public int Score;
        public List<List<Tile>> Tiles = new List<List<Tile>>();
        public bool GameWon;
        public GameData(List<List<Tile>> tiles,int highscore,int score)
        {
            Tiles = tiles;
            HighScore = highscore;
            Score = score;
        }
    }
}
