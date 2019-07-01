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
        // Guarda mensagens do chat num ficheiro
        public static void SaveData(string fileName, string msg)
        {
            using (StreamWriter streamWriter = File.AppendText(fileName + ".txt"))
            {
                streamWriter.Write(msg + ";");
            }
        }

        // Le o log das mensagens do chat
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
