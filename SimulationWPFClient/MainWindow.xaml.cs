using Microsoft.AspNetCore.SignalR.Client;
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

namespace SimulationWPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            InitializeSignalR();
        }

        private async void InitializeSignalR()
        {

            try
            {
                connection = new HubConnectionBuilder()
                            .WithUrl("https://localhost:5000/chat")
                            .Build();

                #region snippet_ClosedRestart
                connection.Closed += async (error) =>
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await connection.StartAsync();
                };
                #endregion

                #region snippet_ConnectionOn
                connection.On<string>("Send", (message) =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        lstListBox.Items.Add(message);
                    });
                });

                connection.On<string>("Notify", (message) =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        lstListBox.Items.Add(message);
                    });
                });
                #endregion

                try
                {
                    await connection.StartAsync();
                    lstListBox.Items.Add("Connection started");
                    //connectButton.IsEnabled = false;
                    sendBtn.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    lstListBox.Items.Add(ex.Message);
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text = message.Text;
            if (text != "")
            {
                connection.InvokeAsync("Send", text);
            }
        }
    }
}
