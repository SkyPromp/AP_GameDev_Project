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
                File.WriteAllBytes("Rooms\\NewRoom.room", bytes.ToArray());
                Debug.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            } catch 
            { 
                throw new Exception(string.Format("Saving new room has failed, here are the raw bytes: {0}", bytes.ToArray().ToString()));
            }
        }
    }
}
