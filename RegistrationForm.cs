using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace DemoExam
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void buttonReg_Click(object sender, EventArgs e)
        {
            try
            {
                if(String.IsNullOrWhiteSpace(textBoxLogin.Text) || String.IsNullOrWhiteSpace(textBoxPassword1.Text) || String.IsNullOrWhiteSpace(textBoxPassword2.Text))
                {
                    MessageBox.Show("Все поля должны быть заполнены и состоять не только из символов-разделителей!", "Retry", MessageBoxButtons.RetryCancel);
                    return;
                }
                if (textBoxPassword1.Text != textBoxPassword2.Text)
                {
                    MessageBox.Show("Пароли не совпадают!");
                    return;
                }
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.CommandText = "INSERT INTO register(login_user, password_user) VALUES(@login, @password)";
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@login",textBoxLogin.Text);
                    command.Parameters.AddWithValue("@password", textBoxPassword2.Text);
                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show($"Пользовать {textBoxLogin.Text} создан успешно!", "Success");
                        CurrentUser.Name = textBoxLogin.Text;
                        this.Close();
                    }
                    else MessageBox.Show("Пользователь не добавлен в базу данных. Повторите попытку!","Retry",MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                }
            }
            catch(Exception ex)
            {
                Log.Logging(ex);
            }
        }
    }
}
