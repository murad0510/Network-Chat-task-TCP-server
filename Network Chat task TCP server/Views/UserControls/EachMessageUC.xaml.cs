using Network_Chat_task_TCP_server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Network_Chat_task_TCP_server.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EachMessageUC.xaml
    /// </summary>
    public partial class EachMessageUC : UserControl
    {
        public EachMessageUC()
        {
            InitializeComponent();
            EachMessageUcViewModel eachMessageUcView = new EachMessageUcViewModel();
            this.DataContext = eachMessageUcView;
        }
    }
}
