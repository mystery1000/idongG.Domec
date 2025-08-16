namespace idongG.Domec.PlcDA.Logs;

public class NewMessageClass
{
    public string MessageStr { get; set; } = "";

    public EnumMsgType Type { get; set; } = EnumMsgType.None;
    public DateTime DTime { get; set; }

    /// <summary>
    /// 是否写入文件
    /// </summary>
    public bool IsWrite2File { get; set; }

    /// <summary>
    /// 是否显示在界面上
    /// </summary>
    public bool IsShowInUI { get; set; }
}