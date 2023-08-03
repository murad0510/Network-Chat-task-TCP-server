using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_Chat_task_TCP_server.ViewModels
{
    public class EachMessageUcViewModel : BaseViewModel
    {
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }
    }
}
