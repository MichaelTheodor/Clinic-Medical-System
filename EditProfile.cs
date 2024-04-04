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
    public partial class EditProfile : Form
    {

        int account_id;
        public EditProfile(int account_id)
        {
            InitializeComponent();
            this.account_id = account_id;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                MessageBox.Show("Please enter a name!");
                return;
            }
            SqlCommand command = conn.CreateCommand();
            command.CommandText = "UPDATE account SET account_name=@name, account_dob=@dob,account_notes=@notes,account_phone=@phone WHERE account_id=@account_id";
            command.Parameters.AddWithValue("@name", textBox6.Text);
            command.Parameters.AddWithValue("@dob", dateTimePicker1.Value.ToString()); // command.Parameters.AddWithValue("@dob", dateTimePicker1.Value);
            command.Parameters.AddWithValue("@phone", textBox8.Text);
            command.Parameters.AddWithValue("@notes", richTextBox2.Text);
            command.Parameters.AddWithValue("@account_id", account_id);

            conn.Open();
            if (command.ExecuteNonQuery() > 0)
                MessageBox.Show("Account was updated!");
            else
                MessageBox.Show("Account was not updated!");
            conn.Close();
        }

        SqlConnection conn = new SqlConnection(Properties.Resources.connectionString);
        

        private void EditProfile_Load(object sender, EventArgs e)
        {
            SqlCommand command = conn.CreateCommand();
            command.CommandText = "SELECT user_username,account_name , account_dob , account_phone , account_type , account_notes , account_creation_date FROM [user], account WHERE account_user_id = user_id AND account_id=@account_id";
            command.Parameters.AddWithValue("@account_id", account_id);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                textBox4.Text = account_id.ToString();
                textBox3.Text = reader.GetValue(0).ToString();
                textBox6.Text = reader.GetValue(1).ToString();
                try
                {
                    dateTimePicker1.Value = DateTime.Parse(reader.GetValue(2).ToString());
                }
                catch (Exception)
                {

                }
                textBox8.Text = reader.GetValue(3).ToString();
                if (reader.GetInt32(4) == 0)
                    textBox7.Text = "Secretary";
                else if (reader.GetInt32(4) == 1)
                    textBox7.Text = "Doctor";
                else if (reader.GetInt32(4) == 2)
                    textBox7.Text = "Patient";
                richTextBox2.Text = reader.GetValue(5).ToString();
                textBox9.Text = reader.GetValue(6).ToString();

            }

            conn.Close();
        }
    }
}
