using idongG.Domec.PlcDA.EquipmentManage;
using idongG.Domec.PlcDA.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace idongG.Domec.PlcDA.ToolManage;

/// <summary>
/// 工具管理器类，负责工具的添加、删除、修改等操作
/// </summary>
public sealed class ToolManager : BaseManager
{
    // 使用Lazy<T>实现懒加载单例模式
    private static readonly Lazy<ToolManager> _instance = new Lazy<ToolManager>(() => new ToolManager(new BestLog(FilePath.LogPath)));

    /// <summary>
    /// 私有构造函数，防止外部实例化
    /// </summary>
    private ToolManager(INewLog logger) : base(logger)
    {
        // 设置相对保存路径
        var RelativeSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
        // 设置保存文件名
        SavePath = "ToolManager.json";
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
    /// 获取ToolManager的单例实例
    /// </summary>
    public static ToolManager Instance => _instance.Value;
}