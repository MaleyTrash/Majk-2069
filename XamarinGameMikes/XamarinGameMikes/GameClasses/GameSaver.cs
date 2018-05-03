using System;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
namespace XamarinGameMikes
{
    public class GameSaver
    {
        private BinaryFormatter bf = new BinaryFormatter();
        private FileStream saveFile;
        private string RootFolder = 
        public void saveGame(Game game)
        {
            if (!File.Exists(RootFolder + "/GameData.dat"))
            {
               File.Create(RootFolder + "/GameData.dat");
            }

            GameData gameData = new GameData(game.Tiles, int.Parse(game.HighScoreLabel.Text), game.score);
            saveFile = File.Open(RootFolder + "/GameData.dat", FileMode.Open);

            bf.Serialize(saveFile, gameData);
            Debug.WriteLine("GameSaved");
        }
       
        public GameData LoadGame()
        {
            saveFile = File.Open(RootFolder + "/GameData.dat", FileMode.Open);
            GameData gameData = (GameData)bf.Deserialize(saveFile);
            Debug.WriteLine("Game Loaded");
            return gameData;
        }
        public bool saveExist()
        {
            if (File.Exists(RootFolder + "/GameData.dat"))
            {
                Debug.WriteLine("FileExists true");
                return true;
            }
            else
            {
                Debug.WriteLine("FileExists false");
                return false;
            }
               
        }
        public string getPath()
        {
            return RootFolder;
        }
    }
}
