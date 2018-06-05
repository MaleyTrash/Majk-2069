using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Reflection;
namespace XamarinGameMikes
{
    public class Game
    {
        //Field
        public List<List<Tile>> Tiles = new List<List<Tile>>();
        public List<List<Image>> GameTiles = new List<List<Image>>();
        public List<List<Tile>> OldTiles = new List<List<Tile>>();
        //Elements
        private Grid MainGrid;
        private Grid OverlayGridLost;
        private Grid OverlayGridWon;
        private Grid GameGrid;
        public Label ScoreLabel;
        public Label HighScoreLabel;
        //
        public GameSaver gameSaver = new GameSaver();
        private CanMakeMove CanMove = new CanMakeMove();
        public GameAnimations gameAnimations;
        //var
        public int OldScore;
        public int score;
        private int HighScore;
        public string SkinName = "";
        public bool GameWon = false;
        //cords
        int x = 0;
        int y = 0;

        public Game(Grid grid, Label Labelscore, Label LabelHighScore, List<List<Tile>> tiles,Grid overlayWon,Grid overlayLost, Grid Main)
        {
            gameAnimations = new GameAnimations(Main);
            MainGrid = Main;
            OverlayGridLost = overlayLost;
            OverlayGridWon = overlayWon;
            GameGrid = grid;
            HighScore = int.Parse(LabelHighScore.Text);
            ScoreLabel = Labelscore;
            HighScoreLabel = LabelHighScore;

            CreateGameField();

            if (tiles != null)
            {
                Tiles = tiles;
            }
            else
            {
                CreateTileList();
            }

            SpawnRandomTileAsync();
            RenderGame(true);

        }
        public async Task NewGameAsync(Grid grid, Label Labelscore, Label LabelHighScore)
        {
            GameWon = false;
            if (OverlayGridWon.IsEnabled == true)
            {
                OverlayGridWon.Opacity = 0;
                OverlayGridWon.IsEnabled = false;
            }
            if (OverlayGridLost.IsEnabled == true)
            {
                OverlayGridLost.Opacity = 0;
                OverlayGridLost.IsEnabled = false;
            }
            //Resets Render grid
            foreach (List<Image> Tiles in GameTiles)
            {
                foreach (Image image in Tiles)
                {
                    GameGrid.Children.Remove(image);
                }
            }
            GameGrid = grid;
            HighScore = score;
            score = 0;
            ScoreLabel.Text = 0.ToString();
            CreateGameField();
            CreateTileList();
            await SpawnRandomTileAsync();
            await RenderGame(true);
        }

        public void CreateTileList()
        {
            Tiles = new List<List<Tile>>();
            for (int i = 0; i < 4; i++)
            {
                Tiles.Add(new List<Tile>());
            }
            foreach (var TileList in Tiles)
            {
                for (int i = 0; i < 4; i++)
                {
                    TileList.Add(new Tile());
                }
            }
        }
        private void CreateOldTileList()
        {
            OldTiles = new List<List<Tile>>();
            int counter = 0;
            for (int i = 0; i < 4; i++)
            {
                OldTiles.Add(new List<Tile>());
            }
            foreach (var OldTilesList in OldTiles)
            {
                for (int i = 0; i < 4; i++)
                {
                    OldTilesList.Add(new Tile()
                    {
                        size = Tiles[counter][i].size,
                    });
                }
                counter++;
            }
        }
        public void CreateGameField()
        {
            GameTiles = new List<List<Image>>();
            for (int i = 0; i < 4; i++)
            {
                GameTiles.Add(new List<Image>());
            }
            foreach (var LabelList in GameTiles)
            {
                for (int i = 0; i < 4; i++)
                {
                    LabelList.Add(new Image()
                    {
                        BackgroundColor = Color.Gray,
                    });
                }
            }

            int c = 0;
            int b = 0;
            foreach (var LabelList in GameTiles)
            {
                foreach (var Label in LabelList)
                {
                    GameGrid.Children.Add(Label, b, c);
                    c++;
                }
                c = 0;
                b++;
            }
        }

        public async Task RenderGame(bool reRender)
        {
            x = 0;
            y = 0;
            foreach (List<Tile> Tiles in Tiles)
            {
                foreach (Tile tile in Tiles)
                {
                    if (tile.size != 0)
                    {
                        GameTiles[y][x].Opacity = 100;
                        if (OldTiles[y][x].size != tile.size)
                        {
                            GameTiles[y][x].Source = "Tile_" + SkinName + tile.size.ToString() + ".jpg";
                        }
                    }
                    else
                    {
                        GameTiles[y][x].Opacity = 0;
                        GameTiles[y][x].Source = null;
                    }
                    if (reRender)
                    {
                        if (tile.size != 0)
                        {
                            GameTiles[y][x].Opacity = 100;
                            GameTiles[y][x].Source = "Tile_" + SkinName + tile.size.ToString() + ".jpg";
                        }
                        else
                        {
                            GameTiles[y][x].Opacity = 0;
                            GameTiles[y][x].Source = null;
                        }
                    }
                    if (tile.size == 8 && GameWon == false)
                    {
                        GameWon = true;
                        await ShowOverlay(true);
                    }
                    x++;
                }
                x = 0;
                y++;
            }
            ScoreLabel.Text = score.ToString();
            if (score > HighScore)
            {
                HighScoreLabel.Text = score.ToString();
            }
            gameSaver.saveGame(this);
        }
        public async Task SpawnRandomTileAsync()
        {
            Random RandTile = new Random();
            List<TilePos> FreeTiles = new List<TilePos>();
            int TileSize = 2;
            x = 0;
            y = 0;

            foreach (List<Tile> TileList in Tiles)
            {
                x = 0;
                foreach (Tile Tile in TileList)
                {
                    if (Tile.size == 0)
                    {

                        FreeTiles.Add(new TilePos()
                        {
                            PosX = x,
                            PosY = y,
                        });
                    }
                    x++;
                }
                y++;
            }
            if (RandTile.Next(0, 4) >= 3)
            {
                TileSize = 4;
            }
            if (FreeTiles.Count != 0)
            {
                int rand = RandTile.Next(0, FreeTiles.Count - 1);
                Tiles[FreeTiles[rand].PosY][FreeTiles[rand].PosX].size = TileSize;
                score += TileSize;
                await gameAnimations.SpawnAnim(GameTiles[FreeTiles[rand].PosY][FreeTiles[rand].PosX]);
            }
            await RenderGame(false);
        }

        public async Task Right()
        {
            OldScore = score;
            CreateOldTileList();
            List<Task> animations = new List<Task>();

            x = 0;
            y = 2;

            int counter = 0;

            int StartY = 0;
            int StartSize = 0;
            bool Moved = false;

            while (x < 4)
            {
                animations = new List<Task>();
                while (y >= 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartY = y;
                        StartSize = Tiles[y][x].size;
                        while (y < 3)
                        {
                            if (Tiles[y + 1][x].size == 0)
                            {
                                Tiles[y + 1][x].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                GameTiles[y][x].Source = null;
                                counter++;
                                y++;
                                Moved = true;
                            }
                            else
                            {
                                y--;
                                break;
                            }
                        }
                        y -= counter;
                        if (counter > 0)
                        {
                            animations.Add(gameAnimations.TileMove(x, StartY, StartSize, 76 * counter, 0, false,GameTiles,GameGrid,SkinName));
                            counter = 0;
                        }
                    }
                    else
                    {
                        y--;
                    }
                }
                Task.WhenAll(animations);
                animations = new List<Task>();
                counter = 0;
                y = 2;

                while (y >= 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartY = y;
                        StartSize = Tiles[y][x].size;
                        if (Tiles[y][x].size == Tiles[y + 1][x].size)
                        {
                            Tiles[y + 1][x].size *= 2;
                            Tiles[y][x].size = 0;
                            GameTiles[y][x].Source = null;
                            score += Tiles[y + 1][x].size;
                            counter++;
                            y--;
                            y--;
                            animations.Add(gameAnimations.TileMove(x, StartY, StartSize, 76 * counter, 0, true,GameTiles,GameGrid,SkinName));
                            counter = 0;
                            Moved = true;
                        }
                        else
                        {
                            y--;
                        }
                    }
                    else
                    {
                        y--;
                    }
                }
                Task.WhenAll(animations);
                animations = new List<Task>();
                counter = 0;
                y = 2;

                while (y >= 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartY = y;
                        StartSize = Tiles[y][x].size;
                        while (y < 3)
                        {
                            if (Tiles[y + 1][x].size == 0)
                            {
                                Tiles[y + 1][x].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                GameTiles[y][x].Source = null;
                                counter++;
                                y++;
                                Moved = true;
                            }
                            else
                            {
                                y--;
                                break;
                            }
                        }
                        y -= counter;
                        if (counter > 0)
                        {
                            animations.Add(gameAnimations.TileMove(x, StartY, StartSize, 76 * counter, 0, false,GameTiles,GameGrid,SkinName));
                            counter = 0;
                        }
                    }
                    else
                    {
                        y--;
                    }
                }
                counter = 0;
                y = 2;
                x++;
                if (x < 3)
                {
                    Task.WhenAll(animations);
                }
            }

            await Task.WhenAll(animations);
            if (Moved)
            {
                await SpawnRandomTileAsync();
            }
            else if (!CanMove.CanMove(Tiles))
            {
                await ShowOverlay(false);
            }
            else
            {
                await RenderGame(false);
            }
        }
        public async Task Left()
        {
            OldScore = score;
            CreateOldTileList();
            List<Task> animations = new List<Task>();
            x = 0;
            y = 3;

            int counter = 0;

            int StartY = 0;
            int StartSize = 0;

            bool Moved = false;

            while (x < 4)
            {
                animations = new List<Task>();
                while (y > 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartY = y;
                        StartSize = Tiles[y][x].size;
                        while (y > 0)
                        {
                            if (Tiles[y - 1][x].size == 0)
                            {
                                Tiles[y - 1][x].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                GameTiles[y][x].Source = null;
                                counter++;
                                y--;
                                Moved = true;
                            }
                            else
                            {
                                y--;
                                break;
                            }
                        }
                        animations.Add(gameAnimations.TileMove(x, StartY, StartSize, -76 * counter, 0, false,GameTiles,GameGrid,SkinName));
                        counter = 0;
                    }
                    else
                    {
                        y--;
                    }
                }
                Task.WhenAll(animations);
                animations = new List<Task>();
                counter = 0;
                y = 3;

                while (y > 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartY = y;
                        StartSize = Tiles[y][x].size;
                        if (Tiles[y][x].size == Tiles[y - 1][x].size)
                        {
                            Tiles[y - 1][x].size *= 2;
                            Tiles[y][x].size = 0;
                            GameTiles[y][x].Source = null;
                            score += Tiles[y - 1][x].size;
                            counter++;
                            y--;
                            y--;
                            animations.Add(gameAnimations.TileMove(x, StartY, StartSize, -76 * counter, 0, true,GameTiles,GameGrid,SkinName));
                            counter = 0;
                            Moved = true;
                        }
                        else
                        {
                            y--;
                        }
                    }
                    else
                    {
                        y--;
                    }
                }
                Task.WhenAll(animations);
                animations = new List<Task>();
                counter = 0;
                y = 3;

                while (y > 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartY = y;
                        StartSize = Tiles[y][x].size;
                        while (y > 0)
                        {
                            if (Tiles[y - 1][x].size == 0)
                            {
                                Tiles[y - 1][x].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                GameTiles[y][x].Source = null;
                                counter++;
                                y--;
                                Moved = true;
                            }
                            else
                            {
                                y--;
                                break;
                            }
                        }
                        animations.Add(gameAnimations.TileMove(x, StartY, StartSize, -76 * counter, 0, false, GameTiles, GameGrid, SkinName));
                        counter = 0;
                    }
                    else
                    {
                        y--;
                    }
                }

                counter = 0;
                y = 3;
                x++;
                if (x < 3)
                {
                    Task.WhenAll(animations);
                }
            }
            await Task.WhenAll(animations);
            if (Moved)
            {
                await SpawnRandomTileAsync();
            }
            else if (!CanMove.CanMove(Tiles))
            {
                await ShowOverlay(false);
            }
            else
            {
                await RenderGame(false);
            }


        }
        public async Task Up()
        {
            OldScore = score;
            CreateOldTileList();
            List<Task> animations = new List<Task>();

            y = 0;
            x = 3;

            bool Moved = false;
            int counter = 0;

            int StartX = 0;
            int StartSize = 0;

            while (y < 4)
            {
                animations = new List<Task>();
                while (x > 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartX = x;
                        StartSize = Tiles[y][x].size;
                        while (x > 0)
                        {
                            if (Tiles[y][x - 1].size == 0)
                            {
                                Tiles[y][x - 1].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                GameTiles[y][x].Source = null;
                                Moved = true;
                                counter++;
                                x--;
                            }
                            else
                            {
                                x--;
                                break;
                            }
                        }
                        x -= counter;
                        if (counter > 0)
                        {
                            animations.Add(gameAnimations.TileMove(StartX, y, StartSize, 0, -76 * counter, false, GameTiles, GameGrid, SkinName));
                            counter = 0;
                        }
                    }
                    else
                    {
                        x--;
                    }
                }
                Task.WhenAll(animations);
                animations = new List<Task>();
                counter = 0;
                x = 3;

                while (x > 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartX = x;
                        StartSize = Tiles[y][x].size;
                        if (Tiles[y][x].size == Tiles[y][x - 1].size)
                        {
                            Tiles[y][x - 1].size *= 2;
                            Tiles[y][x].size = 0;
                            GameTiles[y][x].Source = null;
                            score += Tiles[y][x - 1].size;
                            counter++;
                            x--;
                            x--;
                            animations.Add(gameAnimations.TileMove(StartX, y, StartSize, 0, -76 * counter, true, GameTiles, GameGrid, SkinName));
                            counter = 0;
                            Moved = true;
                        }
                        else
                        {
                            x--;
                        }
                    }
                    else
                    {
                        x--;
                    }
                }
                Task.WhenAll(animations);
                animations = new List<Task>();
                counter = 0;
                x = 3;

                while (x > 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartX = x;
                        StartSize = Tiles[y][x].size;
                        while (x > 0)
                        {
                            if (Tiles[y][x - 1].size == 0)
                            {
                                Tiles[y][x - 1].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                GameTiles[y][x].Source = null;
                                counter++;
                                x--;
                                Moved = true;
                            }
                            else
                            {
                                x--;
                                break;
                            }
                        }
                        x -= counter;
                        if (counter > 0)
                        {
                            animations.Add(gameAnimations.TileMove(StartX, y, StartSize, 0, -76 * counter, false, GameTiles, GameGrid, SkinName));
                            counter = 0;
                        }
                    }
                    else
                    {
                        x--;
                    }
                }
                counter = 0;
                x = 3;
                y++;
                if (y < 3)
                {
                    Task.WhenAll(animations);
                }
            }
            await Task.WhenAll(animations);
            if (Moved)
            {
                await SpawnRandomTileAsync();
            }
            else if (!CanMove.CanMove(Tiles))
            {
                await ShowOverlay(false);
            }
            else
            {
                await RenderGame(false);
            }
        }
        public async Task Down()
        {
            OldScore = score;
            CreateOldTileList();
            List<Task> animations = new List<Task>();

            y = 0;
            x = 2;

            int counter = 0;

            int StartX = 0;
            int StartSize = 0;
            bool Moved = false;

            while (y < 4)
            {
                animations = new List<Task>();
                while (x >= 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartX = x;
                        StartSize = Tiles[y][x].size;
                        while (x < 3)
                        {
                            if (Tiles[y][x + 1].size == 0)
                            {
                                Tiles[y][x + 1].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                GameTiles[y][x].Source = null;
                                counter++;
                                x++;
                                Moved = true;

                            }
                            else
                            {
                                x--;
                                break;
                            }
                        }
                        x -= counter;
                        if (counter > 0)
                        {
                            animations.Add(gameAnimations.TileMove(StartX, y, StartSize, 0, 76 * counter, false, GameTiles, GameGrid, SkinName));
                            counter = 0;
                        }
                    }
                    else
                    {
                        x--;
                    }
                }
                Task.WhenAll(animations);
                animations = new List<Task>();
                counter = 0;
                x = 2;

                while (x >= 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartX = x;
                        StartSize = Tiles[y][x].size;
                        if (Tiles[y][x].size == Tiles[y][x + 1].size)
                        {
                            Tiles[y][x + 1].size *= 2;
                            Tiles[y][x].size = 0;
                            GameTiles[y][x].Source = null;
                            score += Tiles[y][x + 1].size;
                            counter++;
                            x--;
                            x--;
                            animations.Add(gameAnimations.TileMove(StartX, y, StartSize, 0, 76 * counter, true, GameTiles, GameGrid, SkinName));
                            counter = 0;
                            Moved = true;

                        }
                        else
                        {
                            x--;
                        }
                    }
                    else
                    {
                        x--;
                    }
                }
                Task.WhenAll(animations);
                animations = new List<Task>();
                counter = 0;
                x = 2;

                while (x >= 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartX = x;
                        StartSize = Tiles[y][x].size;
                        while (x < 3)
                        {
                            if (Tiles[y][x + 1].size == 0)
                            {
                                Tiles[y][x + 1].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                GameTiles[y][x].Source = null;
                                counter++;
                                x++;
                                Moved = true;

                            }
                            else
                            {
                                x--;
                                break;
                            }
                        }
                        x -= counter;
                        if (counter > 0)
                        {
                            animations.Add(gameAnimations.TileMove(StartX, y, StartSize, 0, 76 * counter, false, GameTiles, GameGrid, SkinName));
                            counter = 0;
                        }
                    }
                    else
                    {
                        x--;
                    }
                }
                counter = 0;
                x = 2;
                y++;
                if (y < 3)
                {
                    Task.WhenAll(animations);
                }
            }
            await Task.WhenAll(animations);
            if (Moved)
            {
                await SpawnRandomTileAsync();
            }
            else if (!CanMove.CanMove(Tiles))
            {
                await ShowOverlay(false);
            }
            else
            {
                await RenderGame(false);
            }
        }
    
        public async Task GoBack()
        {
            score = OldScore;
            Tiles = OldTiles;
            await RenderGame(true);
        }
        
        public async Task ShowOverlay(bool Won)
        {
            if (Won)
            {
                await gameAnimations.ShowOverlay(OverlayGridWon);
            }
            else
            {
                await gameAnimations.ShowOverlay(OverlayGridLost);
            }
        }
    }

}
