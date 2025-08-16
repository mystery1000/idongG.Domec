using System.Net;
using System.Net.Sockets;

namespace idongG.Domec.PlcDA.ToolManage.Modbus.EasyModbus;

internal struct NetworkConnectionParameter
{
    public NetworkStream stream;        //For TCP-Connection only
    public byte[] bytes;
    public int portIn;                  //For UDP-Connection only
    public IPAddress ipAddressIn;       //For UDP-Connection only
}