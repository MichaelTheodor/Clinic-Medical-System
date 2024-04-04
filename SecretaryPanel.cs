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
    public partial class SecretaryPanel : Form
    {

        int account_id;
        public SecretaryPanel(int id)
        {
            InitializeComponent();
            account_id = id;
        }

        private void button1_Click(object sender, EventArgs e) // Edit profile
        {
            Hide();
            EditProfile editProfile = new EditProfile(account_id);
            editProfile.ShowDialog();
            Show();
        }

        private void button2_Click(object sender, EventArgs e)  // Patient Profile
        {
                Hide();
                PatientProfile patientProfiles = new PatientProfile();
                patientProfiles.ShowDialog();
                Show();
        }

         
        

        private void button3_Click_1(object sender, EventArgs e)   // Create Reservation
        {
            Hide();
            CreateReservation createReservation = new CreateReservation(account_id);
            createReservation.ShowDialog();
            Show();

        }

        private void button4_Click(object sender, EventArgs e)  // View  Reservation
        {
            Hide();
            ViewReservations viewReservations = new ViewReservations(account_id);
            viewReservations.ShowDialog();
            Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
            Login_form login_Form = new Login_form();
            login_Form.Show();
        }

        private void SecretaryPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
