using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLINIC
{
    public partial class Starting_form : Form
    {
        public Starting_form()
        {
            InitializeComponent();
            player.uiMode = "none";  // important line
            player.settings.volume = 80;
            player.Visible = true;
            player.URL = @"mp4\DNA_3D.mp4";
            player.Ctlcontrols.play();
            player.settings.playCount = 1;
        }

        private void player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8) // important line
            {
                Loading_form f1 = new Loading_form();
                f1.Show();
                this.Hide();
            }
        }
    }
}
