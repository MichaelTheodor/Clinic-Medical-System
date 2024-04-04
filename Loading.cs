using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MedicalCareSystem
{
    public partial class Loading : Form
    {
        Model1Container context = new Model1Container();
        int startpos = 0;
        


        public Loading()
        {
            
            InitializeComponent();
            player.settings.volume = 5;
            player.Visible = false;
            player.URL = @"mp3\01.Bitter Sweet Symphony.mp3";
            player.Ctlcontrols.play();
            player.settings.playCount = 199;
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            startpos += 1;
            MyprogressBar.Value = startpos;
            if (MyprogressBar.Value == 100)
            {
                MyprogressBar.Value = 100;
                timer1.Stop();
                
                Login f2 = new Login();
                f2.Show();
                this.Hide();
            }

        }

        private void Loading_Load(object sender, EventArgs e)
        {
            timer1.Start();
           
        }

        private void MyprogressBar_Click(object sender, EventArgs e)
        {

        }
    }
}
