using Downloader;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DownloaderTestMain
{
    class DownloaderTest
    {
        static void Main(string[] args)
        {
            ResponseHandler responseHandler = new ResponseHandler();

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
          

            responseHandler.GetResponse(uriList, onDownloadCompleted);
           

        }
    }
}
