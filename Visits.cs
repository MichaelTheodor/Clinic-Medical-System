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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CLINIC
{
    public partial class Visits : Form
    {
        int account_id, patient_id, reservation_id;



        public Visits(int account_id, int patient_id, int reservation_id)
        {
            InitializeComponent();
            this.account_id = account_id;
            this.patient_id = patient_id;
            this.reservation_id = reservation_id;

            command = con.CreateCommand();
            command.CommandText = "SELECT visit_id FROM visit WHERE visit_reservation_id=@reservation_id"; command.Parameters.AddWithValue("@reservation_id", reservation_id);
            con.Open();
            var result = command.ExecuteScalar();
            if (result == null) 
                groupBox1.Enabled = true;
            else
                groupBox1.Enabled = false;
            con.Close();
            updateList();
        }

        SqlConnection con = new SqlConnection(Properties.Resources.connectionString);

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Visits_List.SelectedIndex < 0 || Visits_List.SelectedIndex >= Visits_List.Items.Count)
            {
                MessageBox.Show("Please select a visit!");
                return;
            }

            visit v = (visit)Visits_List.SelectedItem;
            textBox1.Text = v.visit_id.ToString();
            textBox2.Text = v.patient.ToString();
            textBox3.Text = v.secretary.ToString();
            textBox4.Text = v.doctor.ToString();
            textBox5.Text = v.date.ToString();
            richTextBox4.Text = v.reasons;
            richTextBox5.Text = v.diagnosis;
            richTextBox6.Text = v.notes;


        }
        private void button1_Click(object sender, EventArgs e)
         {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Please enter reasons and diagnosis!");
                    return;
                }
                SqlCommand command = con.CreateCommand();
                con.Open();

                command.CommandText = "INSERT INTO visit (visit_id, visit_reasons, visit_diagnosis, visit_notes, visit_doctor_id,, visit_date) VALUES (@id, @reasons, @diagnosis, @notes, @doctor_id,@date)";

                command.Parameters.AddWithValue("@id", reservation_id); 
                command.Parameters.AddWithValue("@reasons", richTextBox1.Text); 
                command.Parameters.AddWithValue("@diagnosis", richTextBox2.Text); 
                command.Parameters.AddWithValue("@notes", richTextBox3.Text); 
                command.Parameters.AddWithValue("@doctor_id", account_id); 
                command.Parameters.AddWithValue("@date", DateTime.Now);
            
                if (command.ExecuteNonQuery() > 0) 
                    MessageBox.Show("Visit was added!");
                else
                    MessageBox.Show("Failed to add the visit!"); 
               
                con.Close();
                updateList();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (richTextBox4.Text == "" || richTextBox5.Text == "")
            {
                MessageBox.Show("Please enter reasons and diagnosis!");
                return;
            }

            SqlCommand command = con.CreateCommand();

            command.CommandText = "UPDATE visit SET visit_reasons = @reasons, visit_diagnosis = @diagnosis, visit_notes = @notes WHERE visit_id = @id ";



            command.Parameters.AddWithValue("@reasons", richTextBox4.Text);
            command.Parameters.AddWithValue("@diagnosis", richTextBox5.Text);
            command.Parameters.AddWithValue("@notes", richTextBox6.Text);
            command.Parameters.AddWithValue("@id", textBox1.Text);
            command.Parameters.AddWithValue("@date", DateTime.Now);

            con.Open();

            if (command.ExecuteNonQuery() > 0)
                MessageBox.Show("Visit was added!");
            else
                MessageBox.Show("Failed to add the visit!");

            con.Close();
            updateList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "" || richTextBox2.Text == "")
            {
                MessageBox.Show("Please enter reasons and diagnosis!");
                return;
            }

            SqlCommand command = con.CreateCommand();
            con.Open();

            command.CommandText = "INSERT INTO visit (visit_reasons, visit_diagnosis, visit_notes, visit_doctor_id, visit_date, visit_reservation_id) VALUES (@reasons, @diagnosis, @notes, @doctor_id, @date, @reservation_id)";

            // Assuming reservation_id is a valid variable that holds the reservation id you want to insert
            command.Parameters.AddWithValue("@reservation_id", reservation_id);
            command.Parameters.AddWithValue("@reasons", richTextBox1.Text);
            command.Parameters.AddWithValue("@diagnosis", richTextBox2.Text);
            command.Parameters.AddWithValue("@notes", richTextBox3.Text);
            command.Parameters.AddWithValue("@doctor_id", account_id);
            command.Parameters.AddWithValue("@date", DateTime.Now);

            if (command.ExecuteNonQuery() > 0)
                MessageBox.Show("Visit was added!");
            else
                MessageBox.Show("Failed to add the visit!");

            con.Close();
            updateList();
            groupBox1.Enabled = false;
        }

        SqlCommand command;

        private void updateList()
        {
            command = con.CreateCommand();
            command.CommandText = "SELECT visit_id, patient.account_id, patient.account_name, secretary.account_id, secretary.account_name ,doctor.account_id,doctor.account_name, visit_date, visit_reasons, visit_diagnosis, visit_notes FROM visit, reservation, account as patient, account as secretary, account as doctor WHERE visit_reservation_id = reservation_id AND reservation_patient_id = patient.account_id AND reservation_secretary_id= secretary.account_id AND visit_doctor_id = doctor.account_id AND patient.account_id = @patient_id ";
            command.Parameters.AddWithValue("@patient_id", patient_id);
            con.Open();
            SqlDataReader reader = command.ExecuteReader();
            Visits_List.Items.Clear();

            while (reader.Read())
            {
                int visit_id = reader.GetInt32(0);
                int patient_id = reader.GetInt32(1);
                string patient_name = reader.GetString(2);
                int secretary_id = reader.GetInt32(3);
                string secretary_name = reader.GetString(4); int doctor_id = reader.GetInt32(5);
                string doctor_name = reader.GetString(6);

                DateTime date = new DateTime();
                DateTime.TryParse(reader.GetValue(7).ToString(), out date);
                string reasons = reader.GetString(8);
                string diagnosis = reader.GetString(9);
                string notes = reader.GetString(10);
                Visits_List.Items.Add(new visit(visit_id, patient_id, patient_name, secretary_id, secretary_name, doctor_id, doctor_name, date, reasons,diagnosis,notes));
            }
            con.Close();

        }
    }     
}

