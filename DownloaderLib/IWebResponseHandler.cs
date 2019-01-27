using System;
using System.Collections.Generic;

namespace Downloader
{ 
    public interface IWebResponseHandler
    {
        //Get Webresponses from a collection of URI's
        void GetResponse(IEnumerable<Uri> uriList, Action<Uri, byte[]> onResponseGathered);

        //Download Webresponses to Directory 
        void DownloadResponsesToDirectory(IEnumerable<Uri> uriList, string Directory);

        //Download a single WebResponse to File 
        void DownloadResponseToFile(Uri url, string Destination);
    }
}
