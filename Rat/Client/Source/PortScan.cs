using System.Net.Sockets;
using Protocol;

namespace Client3
{
    class PortScan
    {
        public static void ScanPort(string ip, string port, Stream stream)
        {
            var client = new TcpClient();
            try
            {
                client.Connect(ip, int.Parse(port));
                stream.WriteString($"[~] status on port {port} --> open");
            }
            catch
            {
                stream.WriteString($"[~] status on port {port} --> closed");
            }
        }
    }
}
