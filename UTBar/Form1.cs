using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.IO;

namespace UTBar
{
    public partial class Form1 : Form
    {
        #region Declaration
        public const int WM_NCLBUTTONDOWN = 0xA1; //Enable form without border movement
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        string[] pathes = new string[7]; //Variables
        Button[] buttonArray = new Button[7];
        Label[] labelArray = new Label[7];
        Point newLoc = new Point(5, 5);
        string path;
        public static bool DisplayNames;
        int btnNumber;
        string progName;
        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog(); //Add app or file to bar
            ofd.Title = "Search for applications or files to add";
            ofd.ShowDialog();
            path = ofd.FileName;
            progName = Microsoft.VisualBasic.Interaction.InputBox("Enter application name:");
            
            if (path != "") //Null protection
            {
                for (int i = 0; i < pathes.Length; i++)
                {
                    if (pathes[i] == null) //Search for blank place in array
                    {
                        pathes[i] = path;
                        Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
                        if (DisplayNames == true) { //Add labels with names to bar
                            labelArray[i] = new Label();
                            labelArray[i].Name = i.ToString();
                            Point lbLoc = newLoc;
                            lbLoc.X -= 5;
                            lbLoc.Y += 40;
                            labelArray[i].Location = lbLoc;
                            labelArray[i].Text = progName;
                            labelArray[i].Size = new Size(53, 15);
                            Controls.Add(labelArray[i]);
                        }
                        buttonArray[i] = new Button(); //Add button to bar
                        buttonArray[i].Size = new Size(40, 40);
                        buttonArray[i].Name = i.ToString();
                        buttonArray[i].Click += StartProgram;
                        buttonArray[i].Location = newLoc;
                        buttonArray[i].FlatStyle = FlatStyle.Popup;
                        buttonArray[i].Image = icon.ToBitmap();
                        Controls.Add(buttonArray[i]);
                        newLoc.X += 50;
                        break;
                    }
                }

            }    
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture(); //Form movement
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void StartProgram(object sender, EventArgs e)
        {
            Button theButton = (Button)sender; //Get button number
            try { btnNumber = Convert.ToInt32(theButton.Name); }
            catch { MessageBox.Show("Whoaps! An error occurred.", "Unexpected error",MessageBoxButtons.OK , MessageBoxIcon.Exclamation); }
            System.Diagnostics.Process.Start(pathes[btnNumber]); //Start app by path
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = true; //Set form to always top
            if (File.Exists("UTBarSettings.cfg"))
            {
                if (File.Exists("UTBarItems.cfg"))
                {
                    LoadFromFiles();
                }
                else
                {
                    InitialConfiguration();
                }
            }
            else
            {
                InitialConfiguration();
            }
            
        }
        private void LoadFromFiles()
        {
            using (StreamReader sr = new StreamReader("UTBarSettings.cfg"))
            {
                string[] dispNames = sr.ReadLine().Split('=');
                if (dispNames[1] == "1")
                    DisplayNames = true;
                else
                    DisplayNames = false;
            }
        }
        private void InitialConfiguration()
        {
            DisplayNames = true;
            if (File.Exists("UTBarSettings.cfg"))
                File.Delete("UTBarSettings.cfg");
            if (File.Exists("UTBarItems.cfg"))
                File.Delete("UTBarItems.cfg");
            using(StreamWriter sw = new StreamWriter(@"UTBarSettings.cfg"))
            {
                sw.WriteLine("DisplayNames=1");
                sw.Flush();
            }
            File.Create("UTBarItems.cfg");
        }
        public static void ChangeConfiguration()
        {
            using (StreamWriter sw = new StreamWriter(@"UTBarSettings.cfg"))
            {
                sw.Write("DisplayNames=");
                if (DisplayNames == true)
                    sw.Write("1");
                else
                    sw.Write("0");
                sw.Flush();
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings(); //Setting form
            settings.StartPosition = FormStartPosition.CenterScreen;
            settings.Show();

        }
    }
}
