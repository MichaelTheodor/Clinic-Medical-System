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

namespace MedicalCareSystem
{
    public partial class Login : Form
    {
        String connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MedicalClinicDb;Integrated Security=True";

        SqlConnection conn;
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Load(object sender, EventArgs e)
        {
            conn= new SqlConnection(connectionString);
        }
        private void button1_Click(object sender, EventArgs e)  // Log in
        {

        }

        

        private void label3_Click(object sender, EventArgs e) 
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) // Show Password
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

        private void label6_Click(object sender, EventArgs e) // Go to Register form
        {
            Register newRegister = new Register();
            newRegister.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)  // Exit
        {
            Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e) // Clear fields
        {
            Clear();
        }


        private void Clear() // method Clear
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e) // Sign In
        {
            if (textBox1.Text=="")
            {
                MessageBox.Show("Enter your username!");
                textBox1.Focus();
            }
            else if (textBox2.Text== "") 
            {
                MessageBox.Show("Enter your password!");
                textBox2.Focus();
            }
            else 
            {
                try 
                {
                    conn.Open();

                    String username = textBox1.Text;
                    String password = textBox2.Text;
                    //Parameterized query
                    String query = "SELECT * FROM [Users] WHERE Username=@username AND Password =@password"; //[ ] to User (as it is a reserved word)
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        MessageBox.Show("Signed in successfully!");  //Authenticated

                        cmd.CommandText = " SELECT account_id,account_type FROM Accounts WHERE user_id = @user_id";
                        cmd.Parameters.AddWithValue("@user_id", result.ToString());
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            int account_id = reader.GetInt32(0);
                            int account_type = reader.GetInt32(1);

                            if (account_type == 1)
                            {

                                AdminPanel f1 = new AdminPanel(account_id); //Admin
                                f1.Show();
                                this.Hide();
                            }
                            else if (account_type == 2)
                            {

                                Form_Secretary f2 = new Form_Secretary(account_id); //Secretary
                                f2.Show();
                                this.Hide();
                            }
                            else if (account_type == 3)
                            {
                                Form_Doctor f3 = new Form_Doctor(account_id); //Doctor
                                f3.Show();
                                this.Hide();
                            }
                            else if (account_type == 4)
                            {
                                Form_Patient f4 = new Form_Patient(account_id);  //Patient
                                f4.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Error");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or/and password...!"); //Authentication failed
                    }
                }
                catch (Exception ex) 
                { 
                    MessageBox.Show("" + ex.Message);
                }
                conn.Close();
            }
        }
    }
}
