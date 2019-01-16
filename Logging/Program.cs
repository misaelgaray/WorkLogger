using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Logging
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] filesInDirectory = Directory.GetFiles(@"C:\Users\Misael\Documents\cleanVersion\sbeasap\.git\logs\refs\heads\");
            
            List<string> filesEditedToday = new List<string>();
            foreach(string file in filesInDirectory) {
                FileInfo directoryInfo = new FileInfo(file);
                int fileEditedDay = directoryInfo.LastWriteTime.Day;
                int fileEditedMonth = directoryInfo.LastWriteTime.Month;
                int currentDay = DateTime.Now.Day;
                int currentMonth = DateTime.Now.Month;
                if (currentDay == fileEditedDay && currentMonth == fileEditedMonth)
                    filesEditedToday.Add(file);
            }

            Console.WriteLine("Se guardaran los archivos de Log de las siguientes ramas en las que ha trabajado hoy.");
            Console.WriteLine("");

            foreach (string file in filesEditedToday) {
                string[] lines = File.ReadAllLines(file);
                List<string> logMessages =new List<string>();
                foreach (string line in lines) {
                    string committimestamp = line.Substring(line.IndexOf(">")+1,11).Trim();
                    DateTime commitDate = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Convert.ToInt64(committimestamp));
                    DateTime currentDate = DateTime.Now;
                    if (currentDate.Day == commitDate.Day && commitDate.Month == currentDate.Month) {
                        string commitmessage = line.Substring(line.IndexOf(":")+1);
                        logMessages.Add(commitmessage);
                    }
                }
                FileInfo fileInfo = new FileInfo(file);
                Console.WriteLine("# " + fileInfo.Name);
                string filePath = @"C:\Users\Misael\Documents\Loggear\" + fileInfo.Name + "-" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
                if (!File.Exists(filePath))
                {
                    using (StreamWriter streamWriter = new StreamWriter(filePath))
                    {
                        foreach (string logmessage in logMessages)
                        {
                            streamWriter.WriteLine(logmessage);
                        }
                    }
                }
            }
            Console.WriteLine("PRESIONE ENTER PARA TERMINAR");
            Console.ReadKey();
            Process.Start("explorer.exe", @"C:\Users\Misael\Documents\Loggear");
        }
    }
}
