using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Downloader
{

    class Program
    {

        static void Main(string[] args)
        {
            BlockingCollection<Byte[]> items = new BlockingCollection<byte[]>(2);

            //Class that includes our DownloadResponseToFile method 
            FileDownloader fD = new FileDownloader();
            ResponseDownloader rD = new ResponseDownloader();

            Uri test1 = new Uri("https://www.reddit.com/r/Saints/comments/a7610k/official_week_15_game_thread_new_orleans_saints/");
            //Uri test2 = new Uri("https://www.sfspca.org/adoptions/pet-details/40393563");
            Uri test2 = new Uri("https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/blockingcollection-overview");
            List<Uri> myList = new List<Uri>();
            myList.Add(test1);
            myList.Add(test2);
            
            Action<Uri, byte[]> onDownloadCompleted = (Uri, data) =>
            {
                Console.WriteLine("Downloaded: {0}  Here's my data: {1}", Uri, data);
                items.Add(data);
                //Console.WriteLine(items.Count);
            };




            //fD.DownloadResponsesToFile(myList, "nine");

            //items.Add(rD.GetResponse(myList,onDownloadCompleted));
            rD.GetResponse(myList, onDownloadCompleted);
           // Console.WriteLine("size: " + items.Count);
            


            Console.ReadKey();
        }
    }
}