using Network_Chat_task_TCP_server.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_Chat_task_TCP_server.ViewModels
{
    public class ChatUcViewModel : BaseViewModel
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }

        private string userMessage;

        public string UserMessage
        {
            get { return userMessage; }
            set { userMessage = value; OnPropertyChanged(); }
        }

        public RelayCommand SendCommand { get; set; }

    }
}
