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
    public partial class MainForm : Form
    {
        DataTable dataTable = new DataTable(); //Лучше создать внутри класса, чтобы обращаться ото всюду внутри класса, а не создавать заново
        public MainForm()
        {
            InitializeComponent();
            labelNameCurrentUser.Text = CurrentUser.Name;
            SetupDataGridView();
            LoadData();
        }

        private void SetupDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn()
            {
                Name = "ID",
                DataPropertyName = "id_student",
                Width = 30,
                Visible = false
            };
            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                DataPropertyName = "name_student",
                Width = 100,
                HeaderText = "Имя студента"
            };
            DataGridViewTextBoxColumn emailColumn = new DataGridViewTextBoxColumn()
            {
                Name = "Email",
                DataPropertyName = "email",
                Width = 150,
                HeaderText = "Почта студента",
                
            };
            DataGridViewImageColumn photoColumn = new DataGridViewImageColumn() //класс умеет работать с байтами изображения(BLOB) из БД
            {
                Name = "Photo",
                DataPropertyName = "photo_student",
                Width = 100,
                HeaderText = "Фото студента",
                ImageLayout = DataGridViewImageCellLayout.Zoom //отрисовка изображения в ячейке
            };
            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn()
            {
                Name = "Edit",
                Width = 40,
                HeaderText = String.Empty,
                Text = "✏", //нажмите Win+.(Или буква 'Ю' на клавиатуре)
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = new DataGridViewCellStyle() { Padding = new Padding(5,30,5, 30)} //отступы формы кнопок от границ ячейки
            };
            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn()
            {
                Name = "Delete",
                Width= 40,
                HeaderText = String.Empty,
                Text = "🗑",
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = new DataGridViewCellStyle() { Padding = new Padding(5, 30, 5, 30) }
            };
            dataGridView1.Columns.AddRange(idColumn, photoColumn, nameColumn,emailColumn,editButtonColumn,deleteButtonColumn);
            
        }

        private void LoadData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id_student,name_student,email,photo_student FROM students";
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
                dataGridView1.DataSource = dataTable; //обязательно после dataTable.Load(reader); так как этот метод генерирует автоматически колонки.
                                                      //Получается, что колонки SetupDataGridView() перекрываются автосгенерированными и вы увидите те колонки, которые хотели скрыть в SetupDataGridView()
            }
            catch (Exception ex)
            {
                Log.Logging(ex);
            }

        }


        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormAddStudent formAddStudent = new FormAddStudent();
            if (formAddStudent.ShowDialog() == DialogResult.OK) 
                LoadData();
        }
        private void DeleteStudent(int idStudent, string nameStudent) //удаление студента по id, переданного из dgv
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM students WHERE id_student = @idStudent";
                    command.Parameters.AddWithValue("@idStudent", idStudent);
                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                        MessageBox.Show($"Студент {nameStudent} удалён!");
                    else MessageBox.Show("Ошибка удаления. Повторите ещё раз!");
                }
            }
            catch(Exception ex)
            {
                Log.Logging(ex);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) //Реализовано удаление через кнопку в dgv
        {
            if (e.RowIndex < 0) return;
            try
            {
                if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index)
                {
                    int idStudent = (int)dataGridView1.Rows[e.RowIndex].Cells["ID"].Value;
                    string nameStudent = dataGridView1.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                    if (MessageBox.Show($"Удалить студента {nameStudent}?", "Are you sure?", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        DeleteStudent(idStudent, nameStudent);
                        dataTable.Rows.RemoveAt(e.RowIndex);
                    }
                }
                if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index)
                {
                    Student oldStudent = new Student()
                    {
                        Id = (int)dataGridView1.Rows[e.RowIndex].Cells["ID"].Value,
                        Name = dataGridView1.Rows[e.RowIndex].Cells["Name"].Value.ToString(),
                        Email = dataGridView1.Rows[e.RowIndex].Cells["Email"].Value.ToString(),
                        PhotoBytes = dataGridView1.Rows[e.RowIndex].Cells["Photo"].Value as byte[]
                    };
                    using (EditStudentForm editStudentForm = new EditStudentForm(oldStudent))
                    {
                        if (editStudentForm.ShowDialog() == DialogResult.OK) LoadData();

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
