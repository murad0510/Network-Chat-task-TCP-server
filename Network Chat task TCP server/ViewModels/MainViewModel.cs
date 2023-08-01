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
using Newtonsoft.Json;

namespace Network_Chat_task_TCP_server.ViewModels
{

    public class MainViewModel : BaseViewModel
    {
        public RelayCommand ConnectServerCommand { get; set; }
        public RelayCommand SelectedUserChangedCommand { get; set; }

        static TcpListener _listener = null;
        static BinaryWriter bw = null;
        static BinaryReader br = null;

        private ObservableCollection<User> allUsers = new ObservableCollection<User>();

        public ObservableCollection<User> AllUsers
        {
            get { return allUsers; }
            set { allUsers = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            var ipAdressRemote = IPAddress.Parse("192.168.0.109");
            var port = 27001;

            SelectedUserChangedCommand = new RelayCommand((_) =>
            {
                MessageBox.Show("s");
            });

            ConnectServerCommand = new RelayCommand((_) =>
            {

                var endPoint = new IPEndPoint(ipAdressRemote, port);

                _listener = new TcpListener(endPoint);

                _listener.Start();
                MessageBox.Show($"Server Start");

                while (true)
                {
                    var client = _listener.AcceptTcpClient();
                    MessageBox.Show($"{client.Client.LocalEndPoint}");

                    Task.Run(() =>
                    {
                        //App.Current.Dispatcher.Invoke((System.Action)delegate
                        //{
                        var reader = Task.Run(() =>
                        {
                            var stream = client.GetStream();
                            br = new BinaryReader(stream);
                            while (true)
                            {
                                var msg = br.ReadString();

                                var user = JsonConvert.DeserializeObject<User>(msg);

                                MessageBox.Show($"{user.Name} adli kisi sayfaya eklendi");
                                //try
                                //{
                                //    AllUsers.Add(user);
                                //}
                                //catch (Exception)
                                //{
                                //    MessageBox.Show($"{AllUsers.Count}");

                                //}
                            }
                        });

                        //}
                        //});

                        var writer = Task.Run(() =>
                        {
                            var stream = client.GetStream();
                            bw = new BinaryWriter(stream);

                            while (true)
                            {
                                bw.Write("salam");
                            }
                        });
                    });
                }
            });
        }
    }
}
