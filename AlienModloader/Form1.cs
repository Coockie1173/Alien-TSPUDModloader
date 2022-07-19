using AlienModLoader.FileHandler;
using Force.Crc32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlienModloader
{
    public partial class Form1 : Form
    {
        FileHandlerContainer fileHandlerContainer = FileHandlerContainer.GetInstance();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PathBox.Text = FileHandlerContainer.GetInstance().HandlerSettings.ProgramSettings["TSPUDPATH"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
            cofd.IsFolderPicker = true;
            CommonFileDialogResult res = cofd.ShowDialog();
            if(res == CommonFileDialogResult.Ok)
            {
                //Sanity checking is for nerds
                fileHandlerContainer.HandlerSettings.ProgramSettings["TSPUDPATH"] = cofd.FileName;
                PathBox.Text = FileHandlerContainer.GetInstance().HandlerSettings.ProgramSettings["TSPUDPATH"];
                fileHandlerContainer.HandlerSettings.SaveSettings();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void GenerateChecksumsBut_Click(object sender, EventArgs e)
        {
            string AIPath;
            string[] files;
            fileHandlerContainer.GetFiles(out AIPath, out files);

            CheckSumBox.Items.Clear();
            fileHandlerContainer.CRCValues.Clear();

            foreach (string file in files)
            {
                CheckSumBox.Items.Add(file.Replace(AIPath, ""));
                string CRC = fileHandlerContainer.CalcCRC(file);
                CheckSumBox.Items.Add(CRC);
                fileHandlerContainer.CRCValues.Add(file.Replace(AIPath, ""), CRC);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fileHandlerContainer.SaveCRCValues();
        }

        private void LoadChecksumsButton_Click(object sender, EventArgs e)
        {
            CheckSumBox.Items.Clear();

            foreach (KeyValuePair<string, string> Item in fileHandlerContainer.CRCValues)
            {
                CheckSumBox.Items.Add(Item.Key);
                CheckSumBox.Items.Add(Item.Value);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult res =  MessageBox.Show("WARNING: THIS IS NOT NEEDED UNLESS YOU WANT TO MAKE MODS. PLEASE ENSURE YOU ARE BACKING UP UNMODIFIED FILES! IF THESE FILES ARENT UNMODIFIED YOUR MODPACK WILL NOT WORK", "WARNING", MessageBoxButtons.OKCancel);
            if(res == DialogResult.OK)
            {
                string AIPath;
                string[] files;
                fileHandlerContainer.GetFiles(out AIPath, out files);
                //got files!
                string execPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                execPath += "\\FileBackup";
                if(Directory.Exists(execPath))
                {
                    Directory.Delete(execPath, true);
                }
                //clear out dir and make clean copy
                Directory.CreateDirectory(execPath);

                foreach(string file in files)
                {
                    string FullPath = "";
                    string NewLoc = execPath + file.Replace(AIPath, "");
                    /*foreach(string s in NewLoc.Split('\\'))
                    {
                        //not a file
                        if(!s.Contains('.'))
                        {
                            FullPath += s + "\\";
                            if(!Directory.Exists(FullPath))
                            {
                                Directory.CreateDirectory(FullPath);
                            }
                        }
                    }*/
                    string[] buf = NewLoc.Split('\\');
                    for(int i = 0; i < buf.Length - 1; i++)
                    {
                        FullPath += buf[i] + '\\';
                        if (!Directory.Exists(FullPath))
                        {
                            Directory.CreateDirectory(FullPath);
                        }
                    }
                    File.Copy(file, NewLoc, true);                    
                }

                MessageBox.Show("Backup complete!");
            }
        }

        private void GenerateModButton_Click(object sender, EventArgs e)
        {
            fileHandlerContainer.GenerateModPack();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.TSPUDMOD|*.TSPUDMOD";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                bool Apply = fileHandlerContainer.ApplyMod(ofd.FileName, false);
                if(Apply)
                {
                    MessageBox.Show("Mod Applied!");
                }
                else
                {
                    MessageBox.Show("Mod Failed to install! Please verify integrity of games files and try again! To ensure your game works, verify the integrity anyways. Some files still may have been altered.");
                }                
            }            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            /*OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.AIMOD|*.AIMOD";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileHandlerContainer.ApplyMod(ofd.FileName, true);
                MessageBox.Show("Mod Removed!");
            } */           
        }
    }
}
