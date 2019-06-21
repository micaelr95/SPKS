using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor
{
    class FileHandler
    {
        private const string fileName = "log.txt";

        public static void SaveData(string msg)
        {
            using (StreamWriter streamWriter = File.AppendText(fileName))
            {
                streamWriter.Write(msg + ";");
            }
        }

        public static string LoadData()
        {
            string log = "";
            try
            {
                log = File.ReadAllText(fileName);
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: LoadData");
            }
            return log;
        }
    }
}
