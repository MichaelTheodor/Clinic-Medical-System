using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLINIC
{
    public partial class Loading_form : Form
    {
        int startpos = 0;
        public Loading_form()
        {
            InitializeComponent();
            player.settings.volume = 25;
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

                Login_form f2 = new Login_form();
                f2.Show();
                this.Hide();
            }
        }
        private void Loading_form_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
