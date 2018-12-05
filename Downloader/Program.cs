

namespace Downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            //Class that includes our DownloadResponseToFile method 
            ReadInput r = new ReadInput();

            r.ManageIncomingURLs(args[0], args[1]);
        }
    }
}
