using idongG.Domec.PlcDA.Logs;

namespace idongG.Domec.PlcDA.EquipmentManage;

/// <summary>
/// 设备管理器类，负责设备的添加、删除、修改等操作
/// </summary>
public sealed class EquipmentManager : BaseManager
{
    // 使用Lazy<T>实现懒加载单例模式
    private static readonly Lazy<EquipmentManager> _instance = new Lazy<EquipmentManager>(() => new EquipmentManager(  new  BestLog(FilePath.LogPath)));

    /// <summary>
    /// 私有构造函数，防止外部实例化
    /// </summary>
    private EquipmentManager(INewLog logger) : base(logger)
    {
        // 设置相对保存路径
        var RelativeSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
        // 设置保存文件名
        SavePath = "EquipmentManager.json";
        // 确保目录存在
        if (!Directory.Exists(RelativeSavePath))
        {
            Directory.CreateDirectory(RelativeSavePath);
        }
        // 设置保存路径
        SavePath = Path.Combine(RelativeSavePath, SavePath);
        Load();
    }

    /// <summary>
    /// 获取EquipmentManager的单例实例
    /// </summary>
    public static EquipmentManager Instance => _instance.Value;
}