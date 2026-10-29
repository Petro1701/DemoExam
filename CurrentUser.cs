using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoExam
{
    public static class CurrentUser
    {
        private static string nameUser = "Danil Petrov";
        private static int idUser;
        public static string Name
        {
            get
            {
                return nameUser;
            }
            set
            {
                nameUser = value;
            }
        }
        public static int Id
        {
            get
            {
                return idUser;
            }
            set
            {
                idUser = value;
            }
        }
    }
}
