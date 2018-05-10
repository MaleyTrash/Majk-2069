using System;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;
namespace XamarinGameMikes
{
    public class GameSaver
    {
        private BinaryFormatter bf = new BinaryFormatter();
        private string RootFolder;
        public GameSaver()
        {
            RootFolder = DependencyService.Get<IFilePath>().getSaveFilePath("GameData.json");
        }
        public void saveGame(Game game)
        {
            GameData gameData = new GameData(game.Tiles, int.Parse(game.HighScoreLabel.Text), game.score);
            using (StreamWriter file = File.CreateText(RootFolder))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, gameData);
            }
        }
       
        public GameData LoadGame()
        {
            using (StreamReader file = File.OpenText(RootFolder))
            {
                JsonSerializer serializer = new JsonSerializer();
                GameData gameData = (GameData)serializer.Deserialize(file, typeof(GameData));
                return gameData;
            }
        }
        public bool saveExist()
        {
            if (File.Exists(RootFolder))
            {
                return true;
            }
            else
            { 
                return false;
            }
               
        }
        public string getPath()
        {
            return RootFolder;
        }
    }
}
