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
        public static void SaveData(string fileName, string msg)
        {
            using (StreamWriter streamWriter = File.AppendText(fileName + ".txt"))
            {
                streamWriter.Write(msg + ";");
            }
        }

        public static string LoadData(string fileName)
        {
            string log = "";
            try
            {
                log = File.ReadAllText(fileName + ".txt");
            }
            catch (Exception)
            {
                return "";
            }
            return log;
        }
    }
}
