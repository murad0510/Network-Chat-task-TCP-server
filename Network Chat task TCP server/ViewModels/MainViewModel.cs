using Network_Chat_task_TCP_server.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;
using Network_Chat_task_TCP_server.Models;

namespace Network_Chat_task_TCP_server.ViewModels
{

    public class MainViewModel : BaseViewModel
    {
        public RelayCommand ConnectServerCommand { get; set; }

        static TcpListener _listener = null;
        static BinaryWriter _writer = null;
        static BinaryReader _reader = null;

        private ObservableCollection<string> users = new ObservableCollection<string>();

        public ObservableCollection<string> Users
        {
            get { return users; }
            set { users = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            ConnectServerCommand = new RelayCommand((_) =>
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        var ipAdressRemote = IPAddress.Parse("10.2.11.3");
                        var port = 27001;

                        var endPoint = new IPEndPoint(ipAdressRemote, port);

                        _listener = new TcpListener(endPoint);

                        _listener.Start();
                        MessageBox.Show($"Server Start");

                        var client = _listener.AcceptTcpClient();
                        MessageBox.Show($"{client.Client.LocalEndPoint}");

                        var stream = client.GetStream();
                        _reader = new BinaryReader(stream);
                        var msg = _reader.ReadString();

                        App.Current.Dispatcher.Invoke((System.Action)delegate
                            {
                        Users.Add(msg);
                    });
                    }
                });

            });
        }
    }
}
