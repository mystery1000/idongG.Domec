namespace idongG.Domec.PlcDA.ToolManage.Modbus.EasyModbus;

public interface IEasyModbus
{
    public enum Enum连接方式
    {
        串口 = 0,
        网口 = 1,
    }

    public Enum连接方式 连接方式 { get; set; }
    public string IP { get; set; }
    public int Port { get; set; }
    public int Timeout { get; set; }
    public int StationNo { get; set; }
    /// <summary>
    /// 端口号
    /// </summary>

    public string PortName { get; set; }

    /// <summary>
    /// 波特率
    /// </summary>
    public int BautRate { get; set; }

    /// <summary>
    /// 数据位
    /// </summary>
    public int DataBits { get; set; }

    /// <summary>
    /// 停止位
    /// </summary>
    public System.IO.Ports.StopBits StopBits { get; set; }

    /// <summary>
    /// 校验位
    /// </summary>
    public System.IO.Ports.Parity Parity { get; set; }
}