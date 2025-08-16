using NModbus;
using NModbus.Serial;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;

namespace idongG.Domec.PlcDA.ToolManage.Modbus.NModbus;

public class ModbusMasterHelper : IDisposable
{
    private readonly TransportType _transportType;

    private IModbusMaster _master;

    private TcpClient _tcpClient;

    private SerialPort _serialPort;
    private ModbusFactory _modbusFactory;
    private int _timeout;

    /// <summary>
    /// 构造函数（TCP传输）
    /// </summary>
    public ModbusMasterHelper(string ipAddress, int port, int timeout = 2000)
    {
        _transportType = TransportType.Tcp;
        _tcpClient = new TcpClient();
        _tcpClient.Connect(ipAddress, port);
        _modbusFactory = new ModbusFactory();
        _timeout = timeout;
        _master = _modbusFactory.CreateMaster(_tcpClient);
        _master.Transport.ReadTimeout = 2000;
    }

    /// <summary>
    /// 构造函数（RTU传输）
    /// </summary>
    public ModbusMasterHelper(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, int timeout = 2000)
    {
        _transportType = TransportType.Rtu;
        if (_serialPort != null)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }
        _timeout = timeout;
        _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        _modbusFactory = new ModbusFactory();
        _master = _modbusFactory.CreateRtuMaster(new SerialPortAdapter(_serialPort));
        _master.Transport.ReadTimeout = _timeout;
        _serialPort.Open();
    }

    public enum TransportType { Tcp, Rtu }

    /// <summary>
    /// 将寄存器值转换为32位浮点数（IEEE 754格式）
    /// </summary>
    public static float ConvertRegistersToFloat(ushort highRegister, ushort lowRegister, bool isBigEndian = true)
    {
        byte[] bytes = isBigEndian ?
            new byte[] { (byte)(highRegister >> 8), (byte)highRegister, (byte)(lowRegister >> 8), (byte)lowRegister } :
            new byte[] { (byte)lowRegister, (byte)(lowRegister >> 8), (byte)highRegister, (byte)(highRegister >> 8) };

        return BitConverter.ToSingle(bytes, 0);
    }

    /// <summary>
    /// 将寄存器值转换为32位整数
    /// </summary>
    public static int ConvertRegistersToInt32(ushort highRegister, ushort lowRegister, bool isBigEndian = true)
    {
        byte[] bytes = isBigEndian ?
            new byte[] { (byte)(highRegister >> 8), (byte)highRegister, (byte)(lowRegister >> 8), (byte)lowRegister } :
            new byte[] { (byte)lowRegister, (byte)(lowRegister >> 8), (byte)highRegister, (byte)(highRegister >> 8) };

        return BitConverter.ToInt32(bytes, 0);
    }

    /// <summary>
    /// 将浮点数转换为两个寄存器值（IEEE 754格式）
    /// </summary>
    public static ushort[] ConvertFloatToRegisters(float value, bool isBigEndian = true)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        return isBigEndian ?
            new ushort[] { (ushort)(bytes[0] << 8 | bytes[1]), (ushort)(bytes[2] << 8 | bytes[3]) } :
            new ushort[] { (ushort)(bytes[2] << 8 | bytes[3]), (ushort)(bytes[0] << 8 | bytes[1]) };
    }

    /// <summary>
    /// 读取线圈状态（FC1）
    /// </summary>
    public bool[] ReadCoils(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        return _master.ReadCoils(slaveAddress, startAddress, numberOfPoints);
    }

    /// <summary>
    /// 读取从站输入数据
    /// </summary>
    /// <param name="slaveAddress">从站地址</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numberOfPoints">读取点数</param>
    /// <returns>返回布尔数组，表示读取到的输入数据</returns>
    public bool[] ReadInputs(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        return _master.ReadInputs(slaveAddress, startAddress, numberOfPoints);
    }

    /// <summary>
    /// 读取保持寄存器（FC3）
    /// </summary>
    public ushort[] ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        return _master.ReadHoldingRegisters(slaveAddress, startAddress, numberOfPoints);
    }

    /// <summary>
    /// 读取输入寄存器（FC4）
    /// </summary>
    public ushort[] ReadInputRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        return _master.ReadInputRegisters(slaveAddress, startAddress, numberOfPoints);
    }

    /// <summary>
    /// 写入单个线圈（FC5）
    /// </summary>
    public void WriteSingleCoil(byte slaveAddress, ushort coilAddress, bool value)
    {
        _master.WriteSingleCoil(slaveAddress, coilAddress, value);
    }

    /// <summary>
    /// 写入单个寄存器（FC6）
    /// </summary>
    public void WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort value)
    {
        _master.WriteSingleRegister(slaveAddress, registerAddress, value);
    }

    /// <summary>
    /// 写入多个线圈（FC15）
    /// </summary>
    public void WriteMultipleCoils(byte slaveAddress, ushort startAddress, bool[] data)
    {
        _master.WriteMultipleCoils(slaveAddress, startAddress, data);
    }

    /// <summary>
    /// 写入多个寄存器（FC16）
    /// </summary>
    public void WriteMultipleRegisters(byte slaveAddress, ushort startAddress, ushort[] data)
    {
        _master.WriteMultipleRegisters(slaveAddress, startAddress, data);
    }

    public void Dispose()
    {
        _master?.Dispose();

        if (_transportType == TransportType.Tcp)
        {
            _tcpClient?.Close();
            _tcpClient?.Dispose();
        }
        else
        {
            _serialPort?.Close();
            _serialPort?.Dispose();
        }
    }
}