using System.IO;
using Xamarin.Forms;
using Windows.Storage;
using XamarinGameMikes.UWP;

[assembly: Dependency(typeof(FileHelper))]
namespace XamarinGameMikes.UWP
{
    public class FileHelper : IFilePath
    {
        public string getSaveFilePath(string Filename)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, Filename);
        }
    }
}
