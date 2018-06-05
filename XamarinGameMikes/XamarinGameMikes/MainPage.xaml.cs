using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinGameMikes
{
    public partial class MainPage : ContentPage, ISwipeCallBack
    {
        public Game gameManager;
        public GameSaver gameSaver = new GameSaver();
        public MainPage()
        {
            
            InitializeComponent();
            GameCreator();
            SwipeListener swipeListener = new SwipeListener(MainView, this);

            Left.Command = new Command(async () => await gameManager.Left());
            Right.Command = new Command(async () => await gameManager.Right());
            Up.Command = new Command(async () => await gameManager.Up());
            Down.Command = new Command(async () => await gameManager.Down());

            Goback.Command = new Command(async () => await gameManager.GoBack());

            RestartGameButton.Command = new Command(async () => await RestartGameAsync());
            OverlayLost.Command = new Command(async () => await RestartGameAsync());
            GameWonNew.Command = new Command(async () => await RestartGameAsync());
            GameWonCont.Command = new Command(async () => await GameContinue());
            
        }
        public void GameCreator()
        {
            if (gameSaver.saveExist())
            {
                GameData gameData = gameSaver.LoadGame();
                if (gameData != null)
                {
                    Debug.WriteLine(gameData.HighScore);
                    Score.Text = gameData.Score.ToString();
                    HighScore.Text = gameData.HighScore.ToString();
                    gameManager = new Game(GameGrid, Score, HighScore, gameData.Tiles, OverLayGridWin, OverLayGridLost,MainGrid);
                }
                else
                {
                    gameManager = new Game(GameGrid, Score, HighScore, null, OverLayGridWin, OverLayGridLost, MainGrid);
                }
            }
            else
            {
                gameManager = new Game(GameGrid, Score, HighScore, null, OverLayGridWin, OverLayGridLost, MainGrid);
            }
        }
        public async Task onBottomSwipeAsync(View view)
        {
        //    Debug.WriteLine("Down");
            await gameManager.Down();
        }
        public async Task onLeftSwipeAsync(View view)
        {
         //   Debug.WriteLine("Left");
            await gameManager.Left();
        }

        public void onNothingSwiped(View view)
        {
            //Debug.WriteLine("NothingSwiped");
        }

        public async Task onRightSwipeAsync(View view)
        {
            //Debug.WriteLine("Right");
            await gameManager.Right();
        }

        public async Task onTopSwipeAsync(View view)
        {
        //    Debug.WriteLine("Up");
            await gameManager.Up();
        }
        public async Task RestartGameAsync()
        {
            await gameManager.NewGameAsync(GameGrid, Score, HighScore);
        }
        public async Task GameContinue()
        {
            await gameManager.gameAnimations.HideOverLay(OverLayGridWin);
        }
    }
}
