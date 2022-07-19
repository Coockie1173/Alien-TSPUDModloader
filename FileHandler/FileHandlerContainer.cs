using Force.Crc32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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

        public void GenerateModPack()
        {
            string AIPath;
            string[] files;
            GetFiles(out AIPath, out files);

            List<string> ChangedFiles = new List<string>();

            foreach(string file in files)
            {
                string MyFileKey = file.Replace(AIPath, "");
                //check CRC
                string MyCRC = CalcCRC(file);
                if(MyCRC != CRCValues[MyFileKey])
                {
                    //not the same!
                    ChangedFiles.Add(MyFileKey);
                }
            }
            //generate folders
            string ProgramFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string execPath = ProgramFolder;
            execPath += "\\TempModFolder";
            if (Directory.Exists(execPath))
            {
                Directory.Delete(execPath, true);
            }
            //clear out dir and make clean copy
            Directory.CreateDirectory(execPath);
            
            foreach (string ModifyPath in ChangedFiles)
            {
                string FullPath = "";
                string NewLoc = execPath + ModifyPath;
                foreach (string s in NewLoc.Split('\\'))
                {
                    //not a file
                    if (!s.Contains('.'))
                    {
                        FullPath += s + "\\";
                        if (!Directory.Exists(FullPath))
                        {
                            Directory.CreateDirectory(FullPath);
                        }
                    }
                }
            }
            //directories generated, now iterate over changed files with xdelta
            string BackupFolder = ProgramFolder + "\\FileBackup";
            string PatchFolder = execPath;
            string GameFolder = HandlerSettings.ProgramSettings["AlienPath"];

            foreach(string s in ChangedFiles)
            {
                Process p = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = ProgramFolder + "\\Tools\\xdelta3-3.0.11-x86_64.exe";
                startInfo.Arguments = "-e -S lzma -s \"" + BackupFolder + s + "\" \"" + GameFolder + s + "\" \"" + PatchFolder + s + "_patch.xdelta\"";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                p.StartInfo = startInfo;
                p.Start();

                p.WaitForExit();
                /*
                p = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.FileName = ProgramFolder + "\\Tools\\xdelta3-3.0.11-x86_64.exe";
                startInfo.Arguments = "-e -S lzma -s \"" + GameFolder + s + "\" \"" + BackupFolder + s + "\" \"" + PatchFolder + s + "_reverse.xdelta\"";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                p.StartInfo = startInfo;
                p.Start();

                p.WaitForExit();*/
            }

            if(File.Exists(ProgramFolder + "\\Mod.AIMOD"))
            {
                File.Delete(ProgramFolder + "\\Mod.AIMOD");
            }

            //ZIP but with different extension hehe
            ZipFile.CreateFromDirectory(PatchFolder, ProgramFolder + "\\Mod.AIMOD");
            //clean up when done
            Directory.Delete(execPath, true);
        }

        public string CalcCRC(string filename)
        {
            byte[] Data = File.ReadAllBytes(filename);
            return String.Format("0x{0:X}", Crc32CAlgorithm.Compute(Data));
        }

        public void GetFiles(out string AIPath, out string[] files)
        {
#if !DEBUG
            List<string> extensions = new List<string> { ".TXT", ".PAK", ".BIN", ".BML", ".XML", ".XSD", ".WEM", ".PCK", ".CACHED", ".PKG", ".USM", ".BNK"};
#endif
#if DEBUG
            List<string> extensions = new List<string> { ".TXT" };
#endif
            AIPath = HandlerSettings.ProgramSettings["AlienPath"];
            files = Directory.GetFiles(AIPath + "\\DATA", "*.*", SearchOption.AllDirectories).Where(x => extensions.IndexOf(Path.GetExtension(x)) >= 0).ToArray();
        }

        public bool ApplyMod(string ModPackPath, bool Reversed = false)
        {
            string ProgramFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string PatchFolder = ProgramFolder;
            PatchFolder += "\\TempModFolder";
            
            if (Directory.Exists(PatchFolder))
            {
                Directory.Delete(PatchFolder, true);
            }
            //clear out dir and make clean copy
            Directory.CreateDirectory(PatchFolder);

            ZipFile.ExtractToDirectory(ModPackPath, PatchFolder);

            //List<string> ModifiedFiles = new List<string>();

            string ReversedText = (Reversed == true ? "_reverse" : "_patch");
            string GameFolder = HandlerSettings.ProgramSettings["AlienPath"];

            string[] Files = Directory.GetFiles(PatchFolder, "*" + ReversedText + ".xdelta", SearchOption.AllDirectories);
            int AmmTries = 0;
            int MaxTries = Files.Length * 5;

            foreach(string file in Files)
            {
                bool Success = false;
                while(!Success)
                {
                    string ShortFile = file.Replace(PatchFolder, "");
                    Process p = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = ProgramFolder + "\\Tools\\xdelta3-3.0.11-x86_64.exe";
                    startInfo.Arguments = "-d -s \"" + GameFolder + ShortFile.Replace(ReversedText + ".xdelta", "") + "\" \"" + PatchFolder + ShortFile + "\" \"" + GameFolder + ShortFile.Replace(ReversedText + ".xdelta", "") + "_MODIFIED\"";
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.CreateNoWindow = true;
                    p.StartInfo = startInfo;
                    p.Start();

                    while (!p.HasExited) { };

                    if (File.Exists(GameFolder + ShortFile.Replace(ReversedText + ".xdelta", "") + "_MODIFIED"))
                    {
                        File.Delete(GameFolder + ShortFile.Replace(ReversedText + ".xdelta", ""));
                        File.Copy(GameFolder + ShortFile.Replace(ReversedText + ".xdelta", "") + "_MODIFIED", GameFolder + ShortFile.Replace(ReversedText + ".xdelta", ""));
                        File.Delete(GameFolder + ShortFile.Replace(ReversedText + ".xdelta", "") + "_MODIFIED");
                        Success = true;
                    }
                    AmmTries++;
                    if(AmmTries > MaxTries)
                    {
                        //haha WHOOPS
                        Directory.Delete(PatchFolder, true);
                        return false;
                    }
                }
            }

            Directory.Delete(PatchFolder, true);

            return true;
        }
    }
}
