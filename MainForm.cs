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
        DataTable dataTable = new DataTable();
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
                Name = "NameStudent",
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
            DataGridViewImageColumn photoColumn = new DataGridViewImageColumn()
            {
                Name = "Photo",
                DataPropertyName = "photo_student",
                Width = 100,
                HeaderText = "Фото студента",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn()
            {
                Name = "Edit",
                Width = 40,
                HeaderText = String.Empty,
                Text = "✏",
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = new DataGridViewCellStyle() { Padding = new Padding(5,30,5, 30)}
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
                dataGridView1.DataSource = dataTable;
            }
            catch(Exception ex)
            {
                Log.Logging(ex);
            }

        }


        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
