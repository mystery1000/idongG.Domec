using idongG.Domec.PlcDA.ToolManage;

namespace idongG.Domec.PlcDA.EquipmentManage;

/// <summary>
/// 设备接口，定义设备必须具有的属性
/// </summary>
public interface IEquipment : IEntity
{
    /// <summary>
    /// 设备名称
    /// </summary>
    string DeviceName { get; }

    /// <summary>
    /// 通信工具名称
    /// </summary>
    string CommunicationToolName { get; set; }

    /// <summary>
    /// PLC地址队列
    /// </summary>
    IReadOnlyList<PlcAddress> PlcAddresses { get; set; }

    /// <summary>
    /// 初始化设备
    /// </summary>
    bool Initialize();

    /// <summary>
    /// 启动设备监听
    /// </summary>
    bool StartListen();

    /// <summary>
    /// 停止设备监听
    /// </summary>
    void StopListen();

    /// <summary>
    /// 重命名设备
    /// </summary>
    /// <param name="newName">新名称</param>
    /// <returns>重命名成功返回true，名称重复或无效返回false</returns>
    bool Rename(string newName);
}