using System;
using System.ComponentModel;

namespace idongG.Domec.PlcDA;

/// <summary>
/// 实体接口，定义所有实体都应具有的基本属性
/// </summary>
public interface IEntity : IDisposable, INotifyPropertyChanged
{
    /// <summary>
    /// 实体唯一标识符
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    /// 实体名称
    /// </summary>
    string NickName { get; set; }

    /// <summary>
    /// 实体启用状态
    /// </summary>
    bool IsEnabled { get; set; }

    /// <summary>
    /// 设备IP地址
    /// </summary>
    string IP { get; set; }

    /// <summary>
    /// 设备端口
    /// </summary>
    int Port { get; set; }
}