using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace AP_GameDev_Project.Utils
{
    internal class FileSaver
    {
        public static void SaveFile(List<Byte> bytes)
        {
            try
            {
                string filename = string.Format("Rooms\\{0}.room", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                File.WriteAllBytes(filename, bytes.ToArray());
                Debug.WriteLine(string.Format("Saving new .room file to: {0}", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename)));
            } catch 
            { 
                throw new Exception(string.Format("Saving new room has failed, here are the raw bytes: {0}", bytes.ToArray().ToString()));
            }
        }
    }
}
