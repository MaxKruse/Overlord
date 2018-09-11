using System;
using System.IO;

namespace Overlord.Utils
{
    class Logger
    {
        private string filepath;

        private string formated;

        public Logger()
        {
            filepath = Path.Combine(Directory.GetCurrentDirectory(), @"Logs\");

            if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);

            File.SetAttributes(filepath, FileAttributes.Normal);

            filepath += DateTime.Now.Year + "-" + DateTime.Now.Month.ToString("d2") + "-" + DateTime.Now.Day.ToString("d2") + " [" + DateTime.Now.Hour.ToString("d2") + "-" + DateTime.Now.Minute.ToString("d2") + "-" + DateTime.Now.Second.ToString("d2") + "].txt";
            FileStream fileHandle = File.Create(filepath);

            fileHandle.Dispose();
        }


        public void Log(string prefix = "DEBUG", string msg = "")
        {
            formated = "[" + prefix + "] => " + msg + "\r\n";
            System.Console.Write(formated);
            var writer = File.AppendText(filepath);
            writer.Write(formated);
            writer.Close();
        }
    }
}
