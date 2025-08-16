using System.Diagnostics;

namespace idongG.Domec.PlcDA.Extend;

/// <summary>
/// 扩展 函数 类
/// </summary>
public static class ExtendNumberic
{
    /// <summary>
    ///  几乎相等  比较 a ± offset < b 两个数在offset范围内相等
    /// </summary>
    /// <param name="currentVlaue">当前值</param>
    /// <param name="standard">标准</param>
    /// <param name="offset">波动范围</param>
    /// <returns></returns>
    public static bool EqualNear(double currentVlaue, double standard, double offset = 0.1)
    {//99,100,1
        if (currentVlaue == standard)
        {
            return true;
        }

        if (standard - offset <= currentVlaue && currentVlaue <= standard + offset)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Delaywait 等待.同步 与sleep一样
    /// </summary>
    /// <param name="msTime"></param>
    public static void Delay(this int msTime)
    {
        if (msTime < 0)
        {
            return;
        }
        Task.Delay(msTime, default).Wait();
    }

    /// <summary>
    ///同步, 与sleep一样
    /// </summary>
    /// <param name="mstime"></param>
    /// <param name="ct"></param>
    public static void Delay(this int mstime, CancellationToken ct)
    {
        if (mstime < 0)
        {
            return;
        }
        Task.Delay(mstime, ct).Wait();
    }

    /// <summary>
    ///      异步await实现delay
    /// </summary>
    /// <param name="msTime"></param>
    public static async Task DelayAsync(this int msTime)
    {
        await Task.Delay(msTime);
    }

    /// <summary>
    /// 延时 ms
    /// </summary>
    /// <param name="mstime"></param>
    public static void Sleep(this int mstime)
    {
        if (mstime < 0)
        {
            return;
        }
        Thread.Sleep(mstime);
    }

    /// <summary>
    /// 自旋 延时 ms ,精准,费CPU
    /// </summary>
    /// <param name="msTime">ms</param>
    public static void SleepSpin(this int msTime)
    {
        if (msTime < 0) return;
        Stopwatch st = Stopwatch.StartNew();
        while (st.ElapsedMilliseconds < msTime) ;
        st.Stop();
        return;
    }

    /// <summary>
    ///可取消的异步等待,调用时请用await接住该方法.异步await实现delay
    /// </summary>
    /// <param name="msTime"></param>
    /// <param name="ct">取消的cts</param>
    /// <returns></returns>
    public static async Task<int> DelayAsync(this int msTime, CancellationTokenSource cts = null)
    {
        if (msTime < 0) return msTime;
        if (cts == null)
        {
            await Task.Delay(msTime);
        }
        else
        {
            await Task.Delay(msTime, cts.Token);
        }

        return msTime;
    }
}