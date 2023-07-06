using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKTecoFingerPrintScanner_Implementation
{
    public static class Log
    {
       

        private static readonly string logFileName = "log.txt";
        private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFileName);



        public static void WriteLog(string message)
        {
            string logMessage = $"{DateTime.Now}: {message}";

            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al escribir en el archivo de registro: {ex.Message}");
            }
        }
    }
}
