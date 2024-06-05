using System.Net.Sockets;
using System.Text;

namespace Killer
{
    public partial class MainPage : ContentPage
    {
        private TcpClient _client;
        private NetworkStream _stream;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {
            try
            {
                await Connect();
                ConnectBtn.Text = "Connected!";
                SemanticScreenReader.Announce(ConnectBtn.Text);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Connection failed: {ex.Message}", "OK");
            }
        }


        private void OnTurnOffClicked(object sender, EventArgs e)
        {
            try
            {
                string token = "it can be litterally any token";
                byte[] bytesToSend = Encoding.UTF8.GetBytes(token);
                _stream.Write(bytesToSend, 0, bytesToSend.Length);
                CounterBtn.Text = "WELL DONE!";
                SemanticScreenReader.Announce(CounterBtn.Text);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to send message: {ex.Message}", "OK");
            }
        }


        public async Task Connect()
        {
            _client = new TcpClient();
            await _client.ConnectAsync("322.228.6.66", 8888);
            _stream = _client.GetStream();
        }
    }
}