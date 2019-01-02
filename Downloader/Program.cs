using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Downloader
{

    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            //Class that manages retrieving URI responses
            ResponseDownloader downloadResponse = new ResponseDownloader();
            //Class that manages downloading responses to file 
            FileDownloader downloadFile = new FileDownloader();

            //Check for command line input 
            if ( !(args == null) && !(args.Length == 0))
            {
                try
                {
                    string Url = args[0];
                    Console.WriteLine(Url);
                    string destination = args[1];
                    Console.WriteLine(destination);
                    downloadFile.DownloadArgInput(Url, destination);
                }
                catch
                {
                    logger.Error("Failed to get contents of args[1] and/or args[2]");
                }
            }

            //Our list that will hold our responses
            BlockingCollection<Byte[]> byteList = new BlockingCollection<byte[]>(2);

            Uri test1 = new Uri("https://www.reddit.com/r/Saints/comments/a7610k/official_week_15_game_thread_new_orleans_saints/");
            Uri test2 = new Uri("https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/blockingcollection-overview");
            Uri test3 = new Uri("https://twitter.com/");
            List<Uri> uriList = new List<Uri>();
            uriList.Add(test1);
            uriList.Add(test2);
            uriList.Add(test3);
            
            Action<Uri, byte[]> onDownloadCompleted = (Uri, data) =>
            {
                Console.WriteLine("Downloaded: {0}  Here's my data: {1}", Uri, data);
                byteList.Add(data);
                Console.WriteLine("Added: {0} to my List", Uri);

                byteList.TryTake(out data);
            };

            downloadResponse.GetResponse(uriList, onDownloadCompleted);
           
            Console.ReadKey();
        }
    }
}