using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlienModLoader.FileHandler
{
    public class FileHandlerContainer
    {
        public Settings HandlerSettings { get; set; }
        public Dictionary<string, string> CRCValues { get; set; }

        private static FileHandlerContainer Instance { get; set; }

        public static FileHandlerContainer GetInstance()
        {
            if (Instance == null)
            {
                Instance = new FileHandlerContainer();
            }
            return Instance;
        }

        private FileHandlerContainer()
        {
            HandlerSettings = new Settings();
            HandlerSettings.LoadSettings();
            CRCValues = new Dictionary<string, string>();
            LoadCRCValues();
        }

        public void SaveCRCValues()
        {
            string execPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            List<string> OutSettings = new List<string>();

            foreach (KeyValuePair<string, string> Item in CRCValues)
            {
                OutSettings.Add(Item.Key + "\t" + Item.Value);
            }

            File.WriteAllLines(execPath + "\\CRCData", OutSettings.ToArray());
        }

        public void LoadCRCValues()
        {
            string execPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (File.Exists(execPath + "\\CRCData"))
            {
                //load in CRC values
                CRCValues.Clear();
                string[] Lines = File.ReadAllLines(execPath + "\\CRCData");

                foreach (string Line in Lines)
                {
                    CRCValues.Add(Line.Split('\t')[0], Line.Split('\t')[1]);
                }
            }
        }
    }
}
