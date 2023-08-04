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
using Network_Chat_task_TCP_server.Views;
using Network_Chat_task_TCP_server.Views.UserControls;
using System.Windows.Interop;

namespace Network_Chat_task_TCP_server.ViewModels
{

    public class MainViewModel : BaseViewModel
    {
        public RelayCommand ConnectServerCommand { get; set; }
        public RelayCommand SelectedUserChangedCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        static TcpListener _listener = null;
        static BinaryWriter bw = null;
        static BinaryReader br = null;

        private ObservableCollection<User> allUsers = new ObservableCollection<User>();

        public ObservableCollection<User> AllUsers
        {
            get { return allUsers; }
            set { allUsers = value; OnPropertyChanged(); }
        }

        private string serverName;

        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; OnPropertyChanged(); }
        }

        private User selectedUser;

        public User SelectedUser
        {
            get { return selectedUser; }
            set { selectedUser = value; OnPropertyChanged(); }
        }

        private bool serverUpButtonIsEnabled = true;

        public bool ServerUpButtonIsEnabled
        {
            get { return serverUpButtonIsEnabled; }
            set { serverUpButtonIsEnabled = value; OnPropertyChanged(); }
        }


        static List<TcpClient> users = new List<TcpClient>();


        public MainViewModel()
        {
            var ipAdressRemote = IPAddress.Parse("10.2.27.3");
            var port = 27001;

            SelectedUserChangedCommand = new RelayCommand((_) =>
            {
                //var a = SelectedUser.EndPoint.Split(':');
                //var ipAdressRemote1 = IPAddress.Parse(a[0]);
                //var port1 = 27002;

                //var endPoint = new IPEndPoint(ipAdressRemote1, port1);

                //_listener = new TcpListener(endPoint);

                //_listener.Start();

                //var client1 = new TcpClient();

                App.MessageWrapPanel.Children.Clear();
                ChatUC chatUC = new ChatUC();
                ChatUcViewModel chatUcViewModel = new ChatUcViewModel();
                chatUcViewModel.UserName = SelectedUser.Name;
                chatUC.DataContext = chatUcViewModel;
                App.MessageWrapPanel.Children.Add(chatUC);

                chatUcViewModel.SendCommand = new RelayCommand((__) =>
                {
                    //MessageBox.Show(chatUcViewModel.UserMessage);

                    Task.Run(() =>
                    {
                        //while (true)
                        //{
                        for (int i = 0; i < users.Count; i++)
                        {
                            if (SelectedUser.EndPoint == users[i].Client.RemoteEndPoint.ToString())
                            {
                                var client = users[i];
                                //var writer = Task.Run(() =>
                                //{
                                var stream = client.GetStream();
                                bw = new BinaryWriter(stream);

                                //while (true)
                                //{
                                if (chatUcViewModel.UserMessage != String.Empty)
                                {
                                    bw.Write(chatUcViewModel.UserMessage);
                                    //MessageBox.Show($"Send message : {serverName}");
                                    App.Current.Dispatcher.Invoke((System.Action)delegate
                                    {
                                        EachMessageUcViewModel eachMessageUcViewModel = new EachMessageUcViewModel();
                                        EachMessageUC eachMessageUC = new EachMessageUC();
                                        eachMessageUcViewModel.Message = chatUcViewModel.UserMessage;
                                        eachMessageUC.DataContext = eachMessageUcViewModel;
                                        //MessageBox.Show($"{eachMessageUcViewModel.Msg}");
                                        App.UserMessageWrapPanel.Children.Add(eachMessageUC);
                                        //MessageBox.Show($"{App.UserMessageStackPanel.Children.Count}");
                                    });
                                    chatUcViewModel.UserMessage = String.Empty;
                                }
                                //}
                                //});
                            }
                            //}
                            ////var client=_listener.
                            //Task.Run(() =>
                            //{
                            //    //App.Current.Dispatcher.Invoke((System.Action)delegate
                            //    //{
                            //    //var reader = Task.Run(() =>
                            //    //{
                            //    //    var stream = client.GetStream();
                            //    //    br = new BinaryReader(stream);
                            //    //    while (true)
                            //    //    {

                            //    //        var msg = br.ReadString();

                            //    //        var user = JsonConvert.DeserializeObject<User>(msg);

                            //    //        //App.Current.Dispatcher.Invoke((System.Action)delegate
                            //    //        //{
                            //    //        //    AllUsers.Add(user);
                            //    //        //    //MessageBox.Show($"{user.Name} adli kisi sayfaya eklendi");
                            //    //        //    //MessageBox.Show($"{AllUsers.Count}");
                            //    //        //    //MessageBox.Show($"{client.Client.LocalEndPoint}");
                            //    //        //});
                            //    //        //try
                            //    //        //{
                            //    //        //    AllUsers.Add(user);
                            //    //        //}
                            //    //        //catch (Exception)
                            //    //        //{

                            //    //        //}
                            //    //    }
                            //    //});

                            //    //}
                            //    //});

                            //    //SendMessageCommand = new RelayCommand((__) =>
                            //    //{
                            //    //var writer = Task.Run(() =>
                            //    //{
                            //    //    var stream = client.GetStream();
                            //    //    bw = new BinaryWriter(stream);

                            //    //    while (true)
                            //    //    {
                            //    //        if (serverName != String.Empty)
                            //    //        {
                            //    //            bw.Write(chatUcViewModel.UserMessage);
                            //    //            //MessageBox.Show($"Send message : {serverName}");
                            //    //            serverName = String.Empty;
                            //    //        }
                            //    //    }
                            //    //});
                            //    //});
                            //});
                        }
                    });
                });
            });

            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        for (int i = 0; i < users.Count; i++)
            //        {
            //            if (!users[i].Connected)
            //            {
            //                users.Remove(users[i]);
            //            }
            //        }
            //    }
            //});

            ConnectServerCommand = new RelayCommand((_) =>
            {
                var endPoint = new IPEndPoint(ipAdressRemote, port);
                ServerUpButtonIsEnabled = false;

                _listener = new TcpListener(endPoint);

                _listener.Start();
                MessageBox.Show($"Server Start");
                User user = null;
                Task.Run(() =>
                {
                    while (true)
                    {
                        var client = _listener.AcceptTcpClient();
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
                                    try
                                    {
                                        var msg = br.ReadString();

                                        user = JsonConvert.DeserializeObject<User>(msg);

                                        App.Current.Dispatcher.Invoke((System.Action)delegate
                                        {
                                            AllUsers.Add(user);
                                            users.Add(client);
                                            //MessageBox.Show($"{user.Name} adli kisi sayfaya eklendi");
                                            //MessageBox.Show($"{AllUsers.Count}");
                                            //MessageBox.Show($"{client.Client.LocalEndPoint}");
                                        });
                                        //try
                                        //{
                                        //    AllUsers.Add(user);
                                        //}
                                        //catch (Exception)
                                        //{

                                        //}
                                    }
                                    catch (Exception)
                                    {
                                        App.Current.Dispatcher.Invoke((System.Action)delegate
                                        {
                                            AllUsers.Remove(user);
                                        });
                                    }
                                }
                            });

                            //}
                            //});

                            //SendMessageCommand = new RelayCommand((__) =>
                            //{

                            //var writer = Task.Run(() =>
                            //{
                            //    var stream = client.GetStream();
                            //    bw = new BinaryWriter(stream);

                            //    while (true)
                            //    {
                            //        if (serverName != String.Empty)
                            //        {
                            //            bw.Write(serverName);
                            //            //MessageBox.Show($"Send message : {serverName}");
                            //            serverName = String.Empty;
                            //        }
                            //    }
                            //});

                            //});
                        });
                    }
                });
            });
        }
    }
}
