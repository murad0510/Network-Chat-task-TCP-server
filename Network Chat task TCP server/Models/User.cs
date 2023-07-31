using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Network_Chat_task_TCP_server.Models
{
    public class User
    {
        public string Name { get; set; }
        public IPEndPoint EndPoint { get; set; }
    }
}
