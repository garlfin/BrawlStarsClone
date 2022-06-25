using System.Runtime.InteropServices;

namespace GameServer.Server.Packets;

public struct PacketData
{
    public int PacketId { get; set; }
    public int PacketSize { get; set; }
    public byte[] Data;
}
