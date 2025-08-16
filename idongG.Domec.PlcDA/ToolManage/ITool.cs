using System;

namespace idongG.Domec.PlcDA.ToolManage;

/// <summary>
/// 工具接口，定义工具必须具有的属性
/// </summary>
public interface ITool : IEntity
{
    /// <summary>
    /// 工具类型
    /// </summary>
    string Type { get; set; }

    /// <summary>
    /// 初始化工具
    /// </summary>
    void Initialize();

    /// <summary>
    /// 启动工具
    /// </summary>
    void Start();

    /// <summary>
    /// 停止工具
    /// </summary>
    void Stop();

    /// <summary>
    /// 工具运行状态
    /// </summary>
    bool IsRunning { get;  }

    /// <summary>
    /// 重命名工具
    /// </summary>
    /// <param name="newName">新名称</param>
    /// <returns>重命名成功返回true，名称重复或无效返回false</returns>
    bool Rename(string newName);
}