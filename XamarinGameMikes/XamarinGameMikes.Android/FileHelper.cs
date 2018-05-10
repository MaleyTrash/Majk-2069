using System;
using System.IO;
using Xamarin.Forms;
using XamarinGameMikes;
using XamarinGameMikes.Droid;
[assembly: Dependency(typeof(FileHelper))]
namespace XamarinGameMikes.Droid
{
    public class FileHelper : IFilePath
    {
        public string getSaveFilePath(string Filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, Filename);
        }
    }
}