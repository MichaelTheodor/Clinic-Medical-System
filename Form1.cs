﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Input;

namespace CLINIC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
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
                if (textBox1.Text == "admin")
                {
                    //Admin Panel
                    Hide();
                    AdminPanel adminPanel = new AdminPanel();
                    adminPanel.ShowDialog();
                    Show();
                }
                else
                {
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
                            Show();
                        }
                        else if (account_type == 1)
                        {
                            //Doctor Panel
                            Hide();
                            DoctorPanel doctorPanel = new DoctorPanel(account_id);
                            doctorPanel.ShowDialog();
                            Show();
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("Authentication Failed");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Utils.createAdmin("admin123");
        }
    }
    
}
