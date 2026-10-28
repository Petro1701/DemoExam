using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DemoExam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.ShowDialog();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if(String.IsNullOrWhiteSpace(textBoxLogin.Text) || String.IsNullOrWhiteSpace(textBoxPassword.Text))
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "select id_user from register where login_user = @login and password_user = @password";
                    command.Parameters.AddWithValue("@login",textBoxLogin.Text);
                    command.Parameters.AddWithValue("@password",textBoxPassword.Text);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MessageBox.Show($"Добро пожаловать, {textBoxLogin.Text}!");
                            CurrentUser.Name = textBoxLogin.Text;
                            CurrentUser.Id = Convert.ToInt32(reader["id_user"]);
                            MainForm mainForm = new MainForm();
                            mainForm.Show();
                            this.Hide();
                        }
                        else MessageBox.Show("Такого пользователя не существует! Повторите ещё раз!", "Retry", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Logging(ex);
            }
        }
    }
}
