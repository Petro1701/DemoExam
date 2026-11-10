using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoExam
{
    public partial class EditStudentForm : Form
    {
        private Student EdittedStudent; //Принимаем всю информацию об изменяемом студенте. Создаем студента в классе, чтобы могли обращаться к его свойствам из всех методов класса.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="student">Из MainForm передаём всю информацию об изменяемом студенте</param>
        public EditStudentForm(Student student)
        {
            InitializeComponent();
            EdittedStudent = new Student()
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                PhotoBytes = student.PhotoBytes
            };
            textBoxEmail.Text = student.Email;
            textBoxName.Text = student.Name;
            using(MemoryStream ms = new MemoryStream(student.PhotoBytes))
            {
                pictureBoxPhotoStudent.Image = Image.FromStream(ms);
            }

        }
        
        private void buttonEditPhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Выберите фото";
                ofd.InitialDirectory = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    EdittedStudent.PhotoBytes= File.ReadAllBytes(ofd.FileName);
                    using (MemoryStream ms = new MemoryStream(EdittedStudent.PhotoBytes))
                    {
                        pictureBoxPhotoStudent.Image = Image.FromStream(ms);
                    }
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                EdittedStudent.Name = textBoxName.Text;
                EdittedStudent.Email = textBoxEmail.Text;
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                using(MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE students SET name_student = @name, email=@email, photo_student=@photoBytes WHERE id_student = @id";
                    command.Parameters.AddWithValue("@name",EdittedStudent.Name);
                    command.Parameters.AddWithValue("@email", EdittedStudent.Email);
                    command.Parameters.AddWithValue("@photoBytes", EdittedStudent.PhotoBytes);
                    command.Parameters.AddWithValue("@id", EdittedStudent.Id);
                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Изменения приняты!");
                        DialogResult = DialogResult.OK;
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
