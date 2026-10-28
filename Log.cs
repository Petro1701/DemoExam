using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoExam
{
    public static class Log
    {
        public static void Logging(Exception ex)
        {
            try
            {
                string logText = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex.Message} \n {ex.StackTrace} \n";
                string pathLog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),"ErrorsLog.txt");
                File.AppendAllText(pathLog, logText);
                MessageBox.Show($"{ex.Message}. Подробности в файле {pathLog}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exLogging)
            {
                MessageBox.Show($"Не удалось записать лог. {exLogging.Message}","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
