using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;


namespace AP_GameDev_Project.Utils
{
    internal class FileSaver
    {
        public static void SaveFile(List<Byte> bytes)
        {
            try
            {
                File.WriteAllBytes(string.Format("Rooms\\{0}.room", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")), bytes.ToArray());
                Debug.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            } catch 
            { 
                throw new Exception(string.Format("Saving new room has failed, here are the raw bytes: {0}", bytes.ToArray().ToString()));
            }
        }
    }
}
