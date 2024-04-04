using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace CLINIC
{
    public partial class ViewReservations : Form
    {


        int account_id, account_type;
        public ViewReservations(int id)
        {
            InitializeComponent();
            account_id = id;
        }

        SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
        SqlCommand command;

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            updateList();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            updateList();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }



        private void updateList()
        {
            SqlCommand command = con.CreateCommand();
            if (radioButton1.Checked)
                command.CommandText = "SELECT reservation_id, reservation_patient_id, patient.account_name, reservation_secretary_id, secretary.account_name ,reservation_visit_date, reservation_visit_slot,reservation_date FROM reservation , account as patient , account as secretary WHERE reservation_patient_id = patient.account_id AND reservation_secretary_id = secretary.account_id AND reservation_visit_date =@date";
            else if (radioButton2.Checked)
                command.CommandText = "SELECT reservation_id, reservation_patient_id, patient.account_name, reservation_secretary_id, secretary.account_name ,reservation_visit_date, reservation_visit_slot,reservation_date FROM reservation , account as patient , account as secretary WHERE reservation_patient_id = patient.account_id AND reservation_secretary_id = secretary.account_id AND (patient.account_name LIKE @query OR patient.account_phone Like @query OR reservation_id LIKE @query)";
            else
                command.CommandText = "SELECT reservation_id, reservation_patient_id, patient.account_name, reservation_secretary_id, secretary.account_name ,reservation_visit_date, reservation_visit_slot,reservation_date FROM reservation , account as patient , account as secretary WHERE reservation_patient_id = patient.account_id AND reservation_secretary_id = secretary.account_id AND (patient.account_name LIKE @query OR patient.account_phone Like @query OR reservation_id LIKE @query) AND reservation_visit_date =@date";


            command.Parameters.AddWithValue("@date", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@query", textBox11.Text + "%");

            con.Open();

            SqlDataReader reader = command.ExecuteReader();
            Reservation_List.Items.Clear();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int patient_id = reader.GetInt32(1);
                string patient_name = reader.GetString(2);
                int secretary_id = reader.GetInt32(3);
                string secretary_name = reader.GetString(4);
                DateTime visit_date = new DateTime();
                DateTime.TryParse(reader.GetValue(5).ToString(), out visit_date);

                int slot = reader.GetInt32(6);

                DateTime date = new DateTime();
                DateTime.TryParse(reader.GetValue(7).ToString(), out date);

                Reservation_List.Items.Add(new reservation(id, patient_id, patient_name, secretary_id, secretary_name,slot,visit_date, date));
            }

            con.Close();


        }

        private void ViewReservations_Load(object sender, EventArgs e)
        {
            updateList();
            command = con.CreateCommand();
            command.CommandText = "SELECT account_type FROM account WHERE account_id = @id";
            command.Parameters.AddWithValue("@id", account_id);
            con.Open();
            account_type = (int)command.ExecuteScalar();
            con.Close();
        }





        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            updateList();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            updateList();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            updateList();
        }

        private void radioButton3_CheckedChanged_1(object sender, EventArgs e)
        {
            updateList();
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            updateForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (Reservation_List.SelectedIndex < 0 || Reservation_List.SelectedIndex >= Reservation_List.Items.Count)
            {
                MessageBox.Show("Please select a reservation!");
                return;
            }

            reservation res = (reservation)Reservation_List.SelectedItem;
            Hide();
            EditReservation editReservation = new EditReservation(res);    
            editReservation.ShowDialog();
            updateList();
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Reservation_List.SelectedIndex < 0 || Reservation_List.SelectedIndex >= Reservation_List.Items.Count)
            {
                MessageBox.Show("Please select a reservation!");
                return;
            }

            reservation res = (reservation)Reservation_List.SelectedItem;
            Hide();
            Visits visits = new Visits(account_id, res.patient.Key, res.id);
            visits.ShowDialog();
            Show();
        }

        private void updateForm()
        {

            if (Reservation_List.SelectedIndex < 0 || Reservation_List.SelectedIndex >= Reservation_List.Items.Count)
            {
                MessageBox.Show("Please select a reservation!");
                return;
            }

            reservation res = (reservation)Reservation_List.SelectedItem;
            textBox1.Text = res.id.ToString();
            textBox2.Text = res.patient.ToString();
            textBox3.Text = res.secretary.ToString();
            textBox4.Text = res.visit_date.Date.ToString();
            textBox5.Text = Utils.getSlots()[res.slot];
            textBox6.Text = res.date.ToString();

            if (account_type == 0 && res.visit_date >= DateTime.Today)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }

            if(account_type == 1)
                button2.Enabled = true;
            else
                button2.Enabled = false;

        }

    }
}
