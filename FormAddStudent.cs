using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoExam
{
    public partial class FormAddStudent : Form
    {
        public FormAddStudent()
        {
            InitializeComponent();
            labelNameUser.Text = CurrentUser.Name;
        }
        private byte[] photoBytes = null;

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(textBoxName.Text) || String.IsNullOrWhiteSpace(textBoxEmail.Text))
                {
                    MessageBox.Show("Все поля должны быть заполнены");
                    return;
                }
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO students(name_student,email, photo_student) VALUES(@name,@email,@photo)";
                    command.Parameters.AddWithValue("@name", textBoxName.Text);
                    command.Parameters.AddWithValue("@email", textBoxEmail.Text);
                    if (photoBytes != null)
                        command.Parameters.AddWithValue("@photo", photoBytes);
                    else command.Parameters.AddWithValue("@photo", DBNull.Value);
                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Студент успешно добавлен!", "Succes", MessageBoxButtons.OK);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else MessageBox.Show("Студент не добавился на уровне отправки данных в БД");
                        
                }
            }
            catch(Exception ex)
            {
                Log.Logging(ex);
            }
        }

        private void buttonChoosePhoto_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Выберите фото студента";
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    photoBytes = File.ReadAllBytes(ofd.FileName);
                    pictureBoxPhotoStudent.Image = Image.FromFile(ofd.FileName);
                }
            }
        }
    }
}
