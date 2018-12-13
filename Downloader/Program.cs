

using System.IO;

namespace Downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            string URLs = args[0];
            string destination = args[1];

            //Class that includes our DownloadResponseToFile method 
            FileDownloader r = new FileDownloader();

            if (Path.GetExtension(URLs).Equals(".txt"))
                r.DownloadMultipleResponsesToFile(URLs, destination);
            else
                r.DownloadResponseToFile(URLs, destination);
        }
    }
}
