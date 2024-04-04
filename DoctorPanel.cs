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
    public partial class DoctorPanel : Form
    {

        int account_id;
        public DoctorPanel(int id)
        {
            InitializeComponent();
            account_id = id;
        }

        private void button1_Click(object sender, EventArgs e) //Edit profile
        {
         
                Hide();
                EditProfile editProfile = new EditProfile(account_id);
                editProfile.ShowDialog();
                Show();
            
        }

        private void button2_Click(object sender, EventArgs e)  //View Reservations
        {
            Hide();
            ViewReservations viewReservations = new ViewReservations(account_id);
            viewReservations.ShowDialog();
            Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Login_form login_Form = new Login_form();
            login_Form.Show();
        }

        private void button4_Click(object sender, EventArgs e) // Patient Profile
        {
            Hide();
            PatientProfile patientProfiles = new PatientProfile();
            patientProfiles.ShowDialog();
            Show();
        }
    }
}
