namespace idongG.Domec.PlcDA.Logs;

public enum EnumMsgType
{
    Main = 0,
    Important,
    Alarm,
    Error,
    Nomal,
    Good,
    None
}

public interface INewLog : IDisposable
{
    public string NickName { get; set; }
    public string SavePath { get; set; }

    public string Description { get; set; }

    public int SaveDays { get; set; }

    public int ReflashTime { get; set; }

    public int LineCounts { get; set; }

    public int FastWriteModeCount { get; set; }

    /// <summary>
    /// 添加记录
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="enumMsgType"></param>
    /// <param name="isWrited"></param>
    /// <param name="isShowInUI"></param>
    /// <returns></returns>
    bool Add(string msg, EnumMsgType enumMsgType = EnumMsgType.None, bool isWrited = true, bool isShowInUI = true);

    /// <summary>
    /// 删除旧文件
    /// </summary>
    void ClearOldFile();

    void NewView();
}