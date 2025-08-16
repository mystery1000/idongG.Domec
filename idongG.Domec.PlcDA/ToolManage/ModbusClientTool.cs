using idongG.Domec.PlcDA.ToolManage.Modbus.EasyModbus;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text;

namespace idongG.Domec.PlcDA.ToolManage;

public class ModbusClientTool : ATool
{
    [JsonIgnore] private ModbusClient? client;
    private bool isOpen;

    [Browsable(false)][JsonIgnore] public bool IsConnect => isOpen;
    public static string Modbus客户端 = nameof(Modbus客户端);
    private readonly object objLock = new object();
    public int Timeout { get; set; }
    public int StationNo { get; set; }

    public ModbusClientTool()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        client = new ModbusClient();
        IP = "127.0.0.1";
        Port = 502;
        Timeout = 300;
        StationNo = 1;
    }

    public bool Connect()
    {
        client.IPAddress = IP;
        client.Port = Port;

        client.ConnectionTimeout = Timeout;
        client.UnitIdentifier = (byte)StationNo;
        try
        {
            client.Connect();
            isOpen = true;
            return true;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// 读整数
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public int ReadHoldRegistsInteger(int address, int len = 1)
    {
        if (address < 0)
        {
            throw new Exception("地址不对");
        }
        if (len != 1 && len != 2)
        {
            throw new Exception("长度不对");
        }
        int ret = -9999;
        lock (objLock)
        {
            int[] s = client.ReadHoldingRegisters(address, len);
            switch (len)
            {
                case 1:
                    ret = s[0];
                    return ret;

                case 2:

                    ret = s[0] * 32768 + s[1];
                    return ret;
            }
        }

        return ret;
    }

    public bool[] ReadCoils(int address, int quantity)
    {
        if (address < 0) return [];
        lock (objLock)
        {
            var b = client.ReadCoils(address, quantity);
            return b;
        }
    }

    /// <summary>
    /// 写整数
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public bool WriteSingleRegister(int address, int value)
    {
        if (address < 0)
        {
            throw new Exception("地址不对");
        }
        int[] d;
        lock (objLock)
        {
            client.WriteSingleRegister(address, value);
            d = client.ReadHoldingRegisters(address, 1);
        }

        return d[0] == value;
    }

    /// <summary>
    /// 读整数
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public int[] ReadHoldRegists(int address, int len)
    {
        if (address < 0) throw new Exception("地址不对");
        if (len < 0) throw new Exception("长度不对");
        lock (objLock)
        {
            var s = client.ReadHoldingRegisters(address - 1, len);

            return s;
        }
    }

    public enum EnumCoding
    {
        UTF8,
        GB2312,
        ASCII,
        GBK,
        BIG5,
        Unicode,
    }

    public bool WriteString(int address, string msg, EnumCoding encoding = EnumCoding.UTF8)
    {
        byte[] array = new byte[10];
        switch (encoding)
        {
            case EnumCoding.GB2312:
                {
                    var code = Encoding.GetEncoding("GB2312");
                    array = code.GetBytes(msg);
                }

                break;

            case EnumCoding.ASCII:
                {
                    array = Encoding.ASCII.GetBytes(msg);
                }
                break;

            case EnumCoding.GBK:
                {
                    var code = Encoding.GetEncoding("GBK");
                    array = code.GetBytes(msg);
                }
                break;

            case EnumCoding.BIG5:
                {
                    var code = Encoding.GetEncoding("BIG5");
                    array = code.GetBytes(msg);
                }
                break;

            case EnumCoding.Unicode:
                array = Encoding.Unicode.GetBytes(msg);
                break;

            case EnumCoding.UTF8:
            default:
                {
                    array = Encoding.UTF8.GetBytes(msg);
                }

                break;
        }

        int[] returnarray = new int[array.Length / 2 + array.Length % 2];
        for (int i = 0; i < returnarray.Length; i++)
        {
            returnarray[i] = array[i * 2];
            if (i * 2 + 1 < array.Length)
            {
                returnarray[i] = returnarray[i] | array[i * 2 + 1] << 8;
            }
        }

        client.WriteMultipleRegisters(address, returnarray);
        return true;
    }

    public string ReadString(int address, int len, EnumCoding encoding = EnumCoding.UTF8)
    {
        lock (objLock)
        {//4754  52 4C   4C 4C  4C 4C  2D31   3233   3433   38CE   D2B0  AEC4    E300  00 00
            var s = client.ReadHoldingRegisters2Byte(address, len);
            switch (encoding)
            {
                case EnumCoding.GB2312:
                    {
                        Encoding coding = Encoding.GetEncoding("GB2312");
                        return coding.GetString(s);
                    }
                case EnumCoding.ASCII:
                    {
                        return Encoding.ASCII.GetString(s);
                    }
                case EnumCoding.GBK:
                    {
                        Encoding coding = Encoding.GetEncoding("gbk");
                        return coding.GetString(s);
                    }
                case EnumCoding.BIG5:
                    {
                        Encoding coding = Encoding.GetEncoding("big5");
                        return coding.GetString(s);
                    }
                case EnumCoding.Unicode:
                    { return Encoding.Unicode.GetString(s); }
                case EnumCoding.UTF8:
                default:
                    {
                        return Encoding.UTF8.GetString(s);
                    }
            }

            // return ConvertRegistersToString(s, 0, strLen);
        }
    }

    //public string ConvertRegistersToString(int[] registers, int offset, int stringLength)
    //{
    //    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    //    byte[] result = new byte[stringLength];
    //    byte[] registerResult = new byte[2];

    //    for (int i = 0; i < stringLength / 2; i++)
    //    {
    //        registerResult = BitConverter.GetBytes(registers[offset + i]);
    //        result[i * 2] = registerResult[0];
    //        result[i * 2 + 1] = registerResult[1];
    //    }

    //}

    /// <summary>
    /// 读浮点数
    /// </summary>
    /// <param name="address1"></param>
    /// <param name="address2"></param>
    /// <returns></returns>
    public float ReadHoldRegistFloat(int address1, int len)
    {
        lock (objLock)
        {
            var s = client.ReadHoldingRegisters(address1, len);
            return ModbusClient.ConvertRegistersToFloat(s);
            //return GetFloat((ushort)s[0], 0);
        }
    }

    public bool WriteCoil(int address, bool value)
    {
        if (address < 0) return false;
        lock (objLock)
        {
            client.WriteSingleCoil(address, value);
            return true;
        }
    }

    public bool WriteMultCoils(int startAddress, bool[] values)
    {
        if (startAddress < 0) return false;
        if (values == null) return false;
        lock (objLock)
        {
            client.WriteMultipleCoils(startAddress, values);
            return true;
        }
    }

    public float GetFloat(ushort P1, ushort P2)
    {
        int intSign, intSignRest, intExponent, intExponentRest;
        float faResult, faDigit;
        intSign = P1 / 32768;
        intSignRest = P1 % 32768;
        intExponent = intSignRest / 128;
        intExponentRest = intSignRest % 128;
        faDigit = (float)(intExponentRest * 65536 + P2) / 8388608;
        faResult = (float)Math.Pow(-1, intSign) * (float)Math.Pow(2, intExponent - 127) * (faDigit + 1);
        return faResult;
    }

    public override void Start()
    {
    }

    public override void Dispose()
    {
        client?.Disconnect();

        client = null;
        isOpen = false;
    }
}