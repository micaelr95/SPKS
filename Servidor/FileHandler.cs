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
                streamWriter.WriteLine(msg);
            }
            //File.WriteAllText(fileName, msg);
        }

        public static List<string> LoadData()
        {
            List<string> fData = new List<string>();
            try
            {
                fData = File.ReadLines(fileName).ToList<string>();
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: LoadData");
            }
            return fData;
        }
    }
}
