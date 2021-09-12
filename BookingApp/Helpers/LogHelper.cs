//using Syroot.Windows.IO;
//using Syroot.Windows.IO;
using System;
using System.IO;

namespace BookingApp.Helpers
{
    public class LogHelper
    {
        //Install-Package Syroot.Windows.IO.KnownFolders
        // new KnownFolder(KnownFolderType.Downloads).Path
        public static void Log(string logMessage)
        {
            //using (StreamWriter w = File.AppendText(new KnownFolder(KnownFolderType.Downloads).Path + "\\Agenda.txt"))
            //{
            //    w.Write("\r\nLog Entry : ");
            //    w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
            //        DateTime.Now.ToLongDateString());
            //    w.WriteLine("  :");
            //    w.WriteLine("  :{0}", logMessage);
            //    w.WriteLine("-------------------------------");
            //}
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
}
