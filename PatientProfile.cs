using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CLINIC
{
    public partial class PatientProfile : Form
    {
        public PatientProfile()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(Properties.Resources.connectionString);
        SqlCommand command;

        private void updateList(string query)
        {

            
            SqlCommand command = conn.CreateCommand();
            
            command.CommandText = "SELECT account_id,account_name,account_type FROM account WHERE account_type=2 AND (account_name LIKE @query OR account_phone LIKE @query)";
            conn.Open();
            command.Parameters.AddWithValue("@query", query +" % ");
                                 
            SqlDataReader reader = command.ExecuteReader();
            Patients_list.Items.Clear();


            while(reader.Read())
            {
                Patients_list.Items.Add(new account(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
            }
            conn.Close();
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            updateList(textBox12.Text);
        }

        private void PatientProfile_Load(object sender, EventArgs e)
        {
            updateList("");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

           //Inputs Validation
            if (textBox10.Text == "" || textBox11.Text == "")
            {
                MessageBox.Show("Please check the inputs!");
                return;
            }

        //Account Creation
            command = conn.CreateCommand();
            command.CommandText = "INSERT INTO account (account_name, account_phone, account_notes, account_type,account_dob, account_creation_date) VALUES (@name, @phone, @notes, 2,@date, @date)";

            command.Parameters.AddWithValue("@name", textBox10.Text);
            command.Parameters.AddWithValue("@phone", textBox11.Text);
            command.Parameters.AddWithValue("@notes", richTextBox1.Text);
            command.Parameters.AddWithValue("@date", DateTime.Today);
            conn.Open();

           
            
                if (command.ExecuteNonQuery() > 0)
                    MessageBox.Show("Account was created!");
                else
                    MessageBox.Show("Failed to create the account!");
            
                conn.Close();
                updateList("");
        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        if (Patients_list.SelectedIndex < 0 || Patients_list.SelectedIndex >= Patients_list.Items.Count)
            return;
        int account_id = ((account)Patients_list.SelectedItem).getID();
        command = conn.CreateCommand();
        command.CommandText = "SELECT account_name, account_dob, account_phone, account_notes, account_creation_date FROM account WHERE account_id =@id";
        command.Parameters.AddWithValue("@id", account_id);

        conn.Open();
        SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            textBox4.Text = account_id.ToString();
            textBox6.Text = reader.GetString(0);
            DateTime dob = new DateTime();
            if (DateTime.TryParse(reader.GetValue(1).ToString(), out dob))
                dateTimePicker1.Value = dob;
            textBox8.Text = reader.GetString(2);
            richTextBox2.Text = reader.GetString(3);
            textBox9.Text = reader.GetValue(4).ToString();
        }
        conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Inputs Validation
            if (textBox6.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("Please check the inputs!");
                return;
            }

            //Editing the account
            command = conn.CreateCommand();
            command.CommandText = "UPDATE account SET account_name = @name, account_phone = @phone, account_dob = @dob, account_notes = @notes WHERE account_id = @id";
            command.Parameters.AddWithValue("@name", textBox6.Text);
            command.Parameters.AddWithValue("@phone", textBox8.Text);
            command.Parameters.AddWithValue("@dob", dateTimePicker1.Value.ToString()); 
            command.Parameters.AddWithValue("@notes", richTextBox2.Text);
            command.Parameters.AddWithValue("@id", textBox4.Text);

            conn.Open();

            if (command.ExecuteNonQuery() > 0)
                MessageBox.Show("Account was updated!");
            else
                MessageBox.Show("Failed to update the account!");

            conn.Close();
            updateList("");
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (Patients_list.SelectedIndex < 0 || Patients_list.SelectedIndex >= Patients_list.Items.Count)
                return;
            int account_id = ((account)Patients_list.SelectedItem).getID();
            command = conn.CreateCommand();
            command.CommandText = "SELECT account_name, account_dob, account_phone, account_notes, account_creation_date FROM account WHERE account_id =@id";
            command.Parameters.AddWithValue("@id", account_id);

            conn.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                textBox4.Text = account_id.ToString();
                textBox6.Text = reader.GetString(0);
                DateTime dob = new DateTime();
                if (DateTime.TryParse(reader.GetValue(1).ToString(), out dob)) 
                    dateTimePicker1.Value = dob;
                textBox8.Text = reader.GetString(2);
                richTextBox2.Text = reader.GetString(3);
                textBox9.Text = reader.GetValue(4).ToString();
            }
            conn.Close();
        }
    }
}
