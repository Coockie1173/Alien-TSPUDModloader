using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace AlienModLoader.FileHandler
{
    public class Settings
    {
        public Dictionary<string, string> ProgramSettings { get; set; }

        public void LoadSettings()
        {
            string execPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if(!File.Exists(execPath + "\\Settings.cfg"))
            {
                //generate new file
                ProgramSettings.Add("TSPUDPATH", ".");
                SaveSettings();
            }
            else
            {
                string[] Lines = File.ReadAllLines(execPath + "\\Settings.cfg");

                foreach(string Line in Lines)
                {
                    ProgramSettings.Add(Line.Split('\t')[0], Line.Split('\t')[1]);
                }
            }
        }

        public void SaveSettings()
        {
            string execPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            List<string> OutSettings = new List<string>();

            foreach (KeyValuePair<string,string> Item in ProgramSettings)
            {
                OutSettings.Add(Item.Key + "\t" + Item.Value);
            }

            File.WriteAllLines(execPath + "\\Settings.cfg", OutSettings.ToArray());
        }

        internal Settings()
        {
            ProgramSettings = new Dictionary<string, string>();
        }
    }
}
