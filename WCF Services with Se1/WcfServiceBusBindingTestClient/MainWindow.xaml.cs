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

using Microsoft.ServiceBus;
using System.ServiceModel;
using System.Runtime.Serialization;
using WcfServiceBusBindingContract;

namespace WcfServiceBusBindingTestClient
{   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public interface IServiceRelayChannel : IServiceRelay, IClientChannel { }
        public interface IServiceQueueChannel : IServiceQueue, IClientChannel { }

        private void ButtonServiceRelay_Click_1(object sender, RoutedEventArgs e)
        {

            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;

            ChannelFactory<IServiceRelayChannel> channelFactory = new ChannelFactory<IServiceRelayChannel>("RelayEndpoint");

            IServiceRelayChannel channel = channelFactory.CreateChannel();
            channel.Open();

           string result = channel.GetData(1);

           MessageBox.Show(result);

           CompositeType dcType = channel.GetDataUsingDataContract(new CompositeType() { BoolValue = true, StringValue = "Hey Code Camp" });

           MessageBox.Show(dcType.StringValue);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;

            ChannelFactory<IServiceQueueChannel> channelFactory = new ChannelFactory<IServiceQueueChannel>("MessagingEndpoint");

            IServiceQueueChannel channel = channelFactory.CreateChannel();
            channel.Open();

            channel.SendData(1);
        }
    }
}
