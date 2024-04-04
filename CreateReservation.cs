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

namespace CLINIC
{
    public partial class CreateReservation : Form
    {

        int secretary_id;
        public CreateReservation(int id)
        {
            InitializeComponent();
            secretary_id = id;
        }

        SqlConnection conn = new SqlConnection(Properties.Resources.connectionString);
        SqlConnection command;

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            updateList(textBox12.Text);
        }


       

        public void updateList(string query)
        {
            SqlCommand command = conn.CreateCommand();
            command.CommandText = "SELECT account_id, account_name, account_type FROM account WHERE account_type = 2 AND(account_name LIKE @query OR account_phone LIKE @query)";

            command.Parameters.AddWithValue("@query", query + "%");

            conn.Open();
            Account_List.Items.Clear();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Account_List.Items.Add(new account(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));

            conn.Close();
        }

   


        private void CreateReservation_Load(object sender, EventArgs e)
        {
            updateList("");
            updateSlots();
            dateTimePicker1.MinDate = DateTime.Today;
        }


        private void updateSlots()
        {
            SqlCommand command = conn.CreateCommand();
            command.CommandText = "SELECT reservation_visit_slot FROM reservation WHERE reservation_visit_date=@date";
            command.Parameters.AddWithValue("@date", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            conn.Open();

            SqlDataReader reader = command.ExecuteReader();

            Dictionary<int, string> slots = Utils.getSlots();

            while (reader.Read())
            {
                slots.Remove(reader.GetInt32(0));
            }

            comboBox1.Items.Clear();

            foreach (object slot in slots.ToArray())
                comboBox1.Items.Add(slot);
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
            conn.Close();

        }

        private void CreateReservation_Load_1(object sender, EventArgs e)
        {
            updateList("");
            updateSlots();
            dateTimePicker1.MinDate = DateTime.Today;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Inputs Validation
            if (Account_List.SelectedIndex < 0 || Account_List.SelectedIndex >= Account_List.Items.Count)
            {
                MessageBox.Show("Please select a patient!");
                return;
            }
            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a slot!");
                return;
            }



            //Perform the reservation

            if (Account_List.SelectedItem == null)
            {
                MessageBox.Show("Please select a patient!");
                return;
            }
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a slot!");
                return;
            }

           
            int patient_id = ((account)Account_List.SelectedItem).getID();
            int slot = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;

            SqlCommand command = conn.CreateCommand();
            command.CommandText = "INSERT INTO reservation (reservation_secretary_id, reservation_patient_id, reservation_visit_date, reservation_visit_slot ,reservation_date) VALUES (@secretary_id, @patient_id, @visit_date, @visit_slot, @date)";

            command.Parameters.AddWithValue("@secretary_id", secretary_id);
            command.Parameters.AddWithValue("@patient_id", patient_id);
            command.Parameters.AddWithValue("@visit_date", dateTimePicker1.Value.ToString());
            command.Parameters.AddWithValue("@visit_slot", slot);
            command.Parameters.AddWithValue("@date", DateTime.Now);

            conn.Open();

            if (command.ExecuteNonQuery() > 0)
            {
                // Reset the @date parameter to the visit date
                command.Parameters["@date"].Value = dateTimePicker1.Value.ToString();

                command.CommandText = "SELECT reservation_id FROM reservation WHERE reservation_visit_date =@date AND reservation_visit_slot =@visit_slot AND reservation_patient_id =@patient_id AND reservation_secretary_id =@secretary_id";

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    int reservation_id = (int)result;
                    MessageBox.Show("Reservation was made!");
                    MessageBox.Show("Reservation ID: " + reservation_id.ToString());
                }
                else
                {
                    MessageBox.Show("Failed to get the reservation ID.");
                }

                conn.Close();
                updateSlots();
            }



        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            updateSlots();
           
        }
    }
}
