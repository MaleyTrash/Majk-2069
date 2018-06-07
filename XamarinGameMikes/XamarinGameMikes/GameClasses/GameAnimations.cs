using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinGameMikes
{
    public class GameAnimations
    {
        private Grid MainGrid;
        private Grid MainView;
        public GameAnimations(Grid main,Grid mainView)
        {
            MainGrid = main;
            MainView = mainView;
        }
        public async Task TileMove(int x, int y, int size, int NewX, int NewY, bool merge, List<List<Image>> GameTiles,Grid GameGrid,string SkinName)
        {
            if (size != 0)
            {
                GameTiles[y][x].Opacity = 0;
                Image AnimLabel = new Image()
                {
                    TranslationX = GameTiles[y][x].X,
                    TranslationY = GameTiles[y][x].Y,
                    BackgroundColor = Color.LightGray,
                    Source = "Tile_" + SkinName + size.ToString() + ".jpg",
                };
                GameGrid.Children.Add(AnimLabel);
                if (merge)
                {
                    await AnimLabel.TranslateTo(NewX + (int)GameTiles[y][x].X, NewY + (int)GameTiles[y][x].Y, 150);
                    await AnimLabel.ScaleTo(1.25, 100);
                    await AnimLabel.ScaleTo(1, 50);
                    await AnimLabel.FadeTo(0, 50);
                }
                else
                {
                    await AnimLabel.TranslateTo(NewX + (int)GameTiles[y][x].X, NewY + (int)GameTiles[y][x].Y, 150);
                }
                GameGrid.Children.Remove(AnimLabel);
            }
        }
        public async Task SpawnAnim(Image label)
        {
            await label.ScaleTo(1.25, 100);
            await label.ScaleTo(1, 50);
        }

        public async Task ShowOverlay(Grid Overlay)
        {
            Overlay.IsEnabled = true;
            MainGrid.RaiseChild(Overlay);
            await Overlay.FadeTo(1, 500);

        }
        public async Task HideOverLay(Grid Overlay)
        {
            await Overlay.FadeTo(0, 500);
            MainGrid.RaiseChild(MainView);
            Overlay.IsEnabled = false;
        }
    }
}
