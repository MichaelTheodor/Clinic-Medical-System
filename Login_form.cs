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
    public partial class Login_form : Form
    {
        public Login_form()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)  // Login
        {
            string connectionString = CLINIC.Properties.Resources.connectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand command = conn.CreateCommand();
            command.CommandText = "SELECT user_id FROM [user] WHERE user_username = @username AND user_password=@password";
            command.Parameters.AddWithValue("@username", textBox1.Text);
            //  command.Parameters.AddWithValue("@password", Utils.hashPassword(textBox2.Text));
            command.Parameters.AddWithValue("@password", textBox2.Text);
            conn.Open();

            var result = command.ExecuteScalar();    //only one attribute in the query

            // Close the connection.
            conn.Close();

            // Check if the user is authenticated.
            if (result != null)
            {
                //Auth
             
                
                    command.Parameters.Clear();
                    conn.Open();
                    command.CommandText = "SELECT account_id, account_type FROM account WHERE account_user_id=@user_id";
                    command.Parameters.AddWithValue("@user_id", result.ToString());
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int account_id = reader.GetInt32(0);
                        int account_type = reader.GetInt32(1);

                        conn.Close();

                        if (account_type == 0)
                        {
                            //Secretary Panel
                            Hide();
                            SecretaryPanel secretaryPanel = new SecretaryPanel(account_id);
                            secretaryPanel.ShowDialog();
                            
                        }
                        else if (account_type == 1)
                        {
                            //Doctor Panel
                            Hide();
                            DoctorPanel doctorPanel = new DoctorPanel(account_id);
                            doctorPanel.ShowDialog();
                            
                        }
                        else if (account_type == 5)
                        {
                            //Admin Panel
                            Hide();
                            AdminPanel adminPanel = new AdminPanel();
                            adminPanel.ShowDialog();
                            
                        } 
                }
            }
            else
            {
                MessageBox.Show("Authentication Failed");
            }
        }

        private void button1_Click(object sender, EventArgs e) // Clear fields
        {
            Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) // Show password
        {
            if (textBox2.Text != "")
            {
                if (checkBox1.Checked)
                {
                    textBox2.UseSystemPasswordChar = true;
                }
                else
                {
                    textBox2.UseSystemPasswordChar = false;
                }
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void Clear() // method Clear
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox1.Focus();
        }
        private void button2_Click(object sender, EventArgs e) // Exit   // Close(); Login_form login_Form = new Login_form(); login_Form.ShowDialog();
        {
        Application.Exit();
           
        }

        private void Login_form_Load(object sender, EventArgs e)
        {
            Utils.createAdmin("admin123");
        }
    }
}

