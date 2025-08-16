using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Channels;

namespace idongG.Domec.PlcDA.Logs;

/// <summary>
/// 日志
/// </summary>

[DisplayName("日志")]
public class BestLog : LogBase
{
    public BestLog(DirectoryInfo path)
    {
        SavePath = path.FullName;
        Description = "";
        LineCounts = 100;
        SaveDays = 900;
        FastWriteModeCount = 100;
        ClearOldFile();
        CreateChannel();
        subject.Subscribe(
                 value =>
                 {
                     SaveFile(value);
                 });
    }

    /// <summary>
    /// 创建新对象.清空
    /// </summary>
    public void CreateChannel()
    {
        oldMsg = "";
        ChannelsUI = null;
        ChannelsUI = Channel.CreateBounded<NewMessageClass>(
           new BoundedChannelOptions(1000)
           {
               FullMode = BoundedChannelFullMode.DropOldest//自动删除最早的数据
           });
    }

    internal Channel<NewMessageClass> ChannelsUI;
    private Subject<NewMessageClass> subject = new();
    private string oldMsg = "";

    /// <summary>
    ///
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="enumMsgType"></param>
    /// <param name="isWrited"></param>
    /// <param name="isShowInUI"></param>
    /// <returns></returns>
    public override bool Add(string msg,
                            EnumMsgType enumMsgType = EnumMsgType.None,
                            bool isWrited = true,
                            bool isShowInUI = true)
    {
        if (oldMsg == msg) return true;
        oldMsg = msg;

        var m = new NewMessageClass()
        {
            MessageStr = msg,
            Type = enumMsgType,
            IsShowInUI = isShowInUI,
            IsWrite2File = isWrited,
            DTime = DateTime.Now
        };
        if (isWrited)
        {
            subject.OnNext(m);
        }
        if (isShowInUI)
        {
            if (ChannelsUI.Reader.CanCount && ChannelsUI.Reader.Count > base.FastWriteModeCount)
            {
                ChannelsUI.Reader.ReadAsync();
            }

            ChannelsUI.Writer.WriteAsync(m);
        }

        return true;
    }
}