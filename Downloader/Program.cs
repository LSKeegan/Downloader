using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            BlockingCollection<Byte[]> items = new BlockingCollection<byte[]>(1);

            //Class that includes our DownloadResponseToFile method 
            FileDownloader fD = new FileDownloader();
            ResponseDownloader rD = new ResponseDownloader();

            Uri test1 = new Uri("https://www.reddit.com/r/Saints/comments/a7610k/official_week_15_game_thread_new_orleans_saints/");
            Uri test2 = new Uri("https://www.sfspca.org/adoptions/pet-details/40393563");
            List<Uri> myList = new List<Uri>();
            myList.Add(test1);
            myList.Add(test2);

            //fD.DownloadResponsesToFile(myList, "nine");

            var timeout = TimeSpan.FromSeconds(90);
            items.Add(rD.GetResponse(myList));
            
            Console.WriteLine(items.Count);


            Console.ReadKey();
        }
    }
}