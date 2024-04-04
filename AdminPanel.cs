using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Principal;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Remoting.Messaging;

namespace CLINIC
{
    public partial class AdminPanel : Form
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void updatelist(string query)
        {
            SqlConnection conn = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand command = conn.CreateCommand();
            conn.Open();
            command.CommandText = " SELECT account_id,account_name,account_type FROM account WHERE account_type in (0,1) AND (account_name LIKE @query OR account_phone LIKE @query OR account_phone LIKE @query) ORDER BY account_type";
            command.Parameters.AddWithValue("@query", query + "%");
            SqlDataReader reader = command.ExecuteReader();

            Accounts_List.Items.Clear();
            while (reader.Read())
                Accounts_List.Items.Add(new account(reader.GetInt32(0),reader.GetString(1), reader.GetInt32(2)));
            conn.Close();
        }

        private void AdminPanel_Load_1(object sender, EventArgs e)
        {
            updatelist("");
        }
   

    private void textBox12_TextChanged(object sender, EventArgs e)
        {
            updatelist(textBox12.Text);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int account_id;
            try
            {
                account_id = ((account)Accounts_List.SelectedItem).getID();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting the account ID: " + ex.Message);
                return;
            }

            SqlConnection conn = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand command = conn.CreateCommand();
            command.CommandText = "SELECT user_username, account_name, account_dob,account_phone, account_type, account_notes ,account_creation_date FROM[user], account WHERE user_id = account_user_id AND account_id = @id";
            command.Parameters.AddWithValue("@id", account_id);
            conn.Open();

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                textBox4.Text = account_id.ToString();
                textBox3.Text = reader.GetValue(0).ToString();
                textBox6.Text = reader.GetValue(1).ToString();
                textBox5.Text = reader.GetValue(2).ToString();
                textBox8.Text = reader.GetValue(3).ToString();
                if (reader.GetInt32(4) == 0)
                {
                    textBox7.Text = "Secretary";
                }
                else
                    textBox7.Text = "Doctor";
                }
                richTextBox2.Text = reader.GetValue(5).ToString();
                textBox9.Text = reader.GetValue(6).ToString();


            


        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!validateInputs())
            {
                MessageBox.Show("Please check the input fields again! ");
                return;
            }


            SqlConnection conn = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand command = conn.CreateCommand();

            command.CommandText = "INSERT INTO [user](user_username,user_password) VALUES (@username ,@password)";
            command.Parameters.AddWithValue("@username", textBox1.Text);
            //  command.Parameters.AddWithValue("@password", Utils.hashPassword(textBox2.Text));
            command.Parameters.AddWithValue("@password", textBox2.Text);
            conn.Open();


            if (command.ExecuteNonQuery() > 0)
            {
                //We created the record in user table
                command.CommandText = "SELECT user_id FROM [user] WHERE user_username = @username";
                int user_id = (int)command.ExecuteScalar();

                command.CommandText = "INSERT INTO account (account_user_id, account_name,account_dob, account_type, account_notes, account_creation_date,account_phone) VALUES (@user_id ,@name ,@dob,@type ,@notes,@date,@phone)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@user_id", user_id);
                command.Parameters.AddWithValue("@name", textBox10.Text);
                command.Parameters.AddWithValue("@dob", dateTimePicker1.Value);
                command.Parameters.AddWithValue("@type", comboBox1.SelectedIndex);
                command.Parameters.AddWithValue("@notes", richTextBox1.Text);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@phone", textBox11.Text);

                if (command.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Account was successfully created!");
                }
                else
                    MessageBox.Show("Error while creating the account!");

            }
            conn.Close();
            updatelist("");

        }

        private bool validateInputs()
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox10.Text == "")
                return false;

            if (comboBox1.SelectedIndex < 0)
                return false;

            return true;
        }




        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
                return;
            SqlConnection con = new SqlConnection(Properties.Resources.connectionString); 
            SqlCommand command = con.CreateCommand();
            command.CommandText = "DELETE FROM [user] WHERE user_username=@username"; 
            command.Parameters.AddWithValue("@username", textBox3.Text);
            
            con.Open();
            
            if (command.ExecuteNonQuery() > 0)
                MessageBox.Show("Account was deleted!");
            else
                MessageBox.Show("Account was not deleted!");
            con.Close();

            updatelist("");


            textBox4.Clear();
            textBox3.Clear();
            textBox6.Clear();
            textBox5.Clear();
            textBox8.Clear();
            textBox7.Clear();   
            richTextBox2.Clear();
            textBox9.Clear();
        }

        private void AdminPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            Login_form login_Form = new Login_form();
            login_Form.Show();
        }
    }
}