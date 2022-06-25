using System.Net;
using System.Net.Sockets;
using GameServer.Server.Packets;

namespace GameServer;


// Socket server class.
public static class GameServer
{
    private static readonly int Port = 11111;
    
    
    private static TcpListener _tcpListener;
    private static UdpClient _udpListener;
    
    public static void Main(string[] args)
    {
        _tcpListener = new TcpListener(IPAddress.Any, Port);
        _tcpListener.Start();
        _tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

        _udpListener = new UdpClient(Port);
        _udpListener.BeginReceive(UDPReceiveCallback, null);
    }
}