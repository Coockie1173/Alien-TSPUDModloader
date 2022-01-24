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
            PathBox.Text = FileHandlerContainer.GetInstance().HandlerSettings.ProgramSettings["AlienPath"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
            cofd.IsFolderPicker = true;
            CommonFileDialogResult res = cofd.ShowDialog();
            if(res == CommonFileDialogResult.Ok)
            {
                //Sanity checking is for nerds
                fileHandlerContainer.HandlerSettings.ProgramSettings["AlienPath"] = cofd.FileName;
                PathBox.Text = FileHandlerContainer.GetInstance().HandlerSettings.ProgramSettings["AlienPath"];
                fileHandlerContainer.HandlerSettings.SaveSettings();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void GenerateChecksumsBut_Click(object sender, EventArgs e)
        {
            List<string> extensions = new List<string> { ".TXT", ".PAK" };
            string AIPath = fileHandlerContainer.HandlerSettings.ProgramSettings["AlienPath"];
            string[] files = Directory.GetFiles(AIPath + "\\DATA", "*.*",SearchOption.AllDirectories).Where(x => extensions.IndexOf(Path.GetExtension(x)) >= 0).ToArray();

            CheckSumBox.Items.Clear();

            foreach(string file in files)
            {                
                CheckSumBox.Items.Add(file.Replace(AIPath,""));
                string CRC = CalcCRC(file);
                CheckSumBox.Items.Add(CRC);
                fileHandlerContainer.CRCValues.Add(file.Replace(AIPath, ""), CRC);
            }
        }
        private string CalcCRC(string filename)
        {
            byte[] Data = File.ReadAllBytes(filename);
            return String.Format("0x{0:X}", Crc32CAlgorithm.Compute(Data));
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
    }
}
