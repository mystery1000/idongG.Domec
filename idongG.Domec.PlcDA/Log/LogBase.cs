using idongG.Domec.PlcDA.Extend;
using System.ComponentModel;
using System.Text;
using System.Threading.Channels;

namespace idongG.Domec.PlcDA.Logs;

public abstract class LogBase : INewLog
{
    public string NickName { get; set; }

    [DisplayName("路径")]
    public string SavePath { get; set; }

    [DisplayName("描述")]
    public string Description { get; set; }

    public int SaveDays { get; set; }

    [DisplayName("显示行数")]
    public int LineCounts { get; set; }

    /// <summary>
    /// 触发快速保存条数
    /// </summary>
    [DisplayName("触发快速保存条数")]
    public int FastWriteModeCount { get; set; }

    [DisplayName("刷新频率ms")]
    public int ReflashTime { get; set; }

    //internal UserControl view;

    //public UserControl View
    //{
    //    get
    //    {
    //        if (view == null)
    //        {
    //            NewView();
    //        }
    //        return view;
    //    }
    //}

    public virtual bool Add(string msg, EnumMsgType enumMsgType = EnumMsgType.None, bool isWrited = true, bool isShowInUI = true)
    {
        throw new Exception("没有实现");
    }

    /// <summary>
    /// 清除旧文件
    /// </summary>
    public void ClearOldFile()
    {
        if (SavePath.IsNullOrEmpty()) return;

        var d = new DirectoryInfo(SavePath);
        if (!d.Exists) { d.Create(); }
        var lst = d.GetFiles().Where(f => (DateTime.Now - f.LastWriteTime).Days > SaveDays);
        foreach (var item in lst) item.Delete();
    }

    private readonly object objLock = new object();

    /// <summary>
    /// 一次一条
    /// </summary>
    /// <param name="message"></param>
    protected internal void SaveFile(NewMessageClass message)
    {
        StreamWriter streamWriter = null;
        if (message == null) return;

        if (SavePath.IsNullOrEmpty()) return;
        if (!message.IsWrite2File) return;

        var d = new DirectoryInfo(SavePath);
        if (!d.Exists) d.Create();

        var fileName = message.DTime.ToString("yyyyMMdd");
        lock (objLock)
        {
            using (streamWriter = new StreamWriter(Path.Combine(d.FullName, $"{fileName}.log"), true, Encoding.UTF8))
            {
                streamWriter.WriteLine($"{message.DTime:yyyyMMdd_HHmmss_fff};{message.MessageStr};{message.Type}");
            }
        }
        // streamWriter.Close();
    }

    /// <summary>
    /// 快速保存
    /// </summary>
    /// <param name="channels"></param>
    protected internal void SaveFileFast(Channel<NewMessageClass> channels)
    {
        StreamWriter streamWriter = null;
        if (channels == null) return;
        if (SavePath.IsNullOrEmpty()) return;
        if (channels.Reader.Count == 0) return;
        NewMessageClass first = null;
        var b = channels.Reader.TryRead(out first);
        if (!b)
        {
            return;
        }
        var d = new DirectoryInfo(SavePath);
        var fileName = first.DTime.ToString("yyyyMMdd");
        if (!d.Exists) d.Create();
        using (streamWriter = new StreamWriter(Path.Combine(d.FullName, $"{fileName}.log"), true, Encoding.UTF8))
        {
            streamWriter.WriteLine($" {first.DTime:yyyyMMdd_HHmmss_fff};被动触千条写入模式-------->开始");
            streamWriter.WriteLine($" {first.DTime:yyyyMMdd_HHmmss_fff};{first.MessageStr};{first.Type} ");
            while (true)
            {
                b = channels.Reader.TryRead(out first);
                if (!b) break;
                if (!first.IsWrite2File) continue;
                streamWriter.WriteLine($" {first.DTime:yyyyMMdd_HHmmss_fff};{first.MessageStr};{first.Type}  ");
            }
            streamWriter.WriteLine($"{DateTime.Now:yyyyMMdd_HHmmss_fff};被动触千条写入模式-------->结束");
        }
        streamWriter.Close();
    }

    public virtual void Dispose()
    {
    }

    public void NewView()
    {
        throw new NotImplementedException();
    }
}