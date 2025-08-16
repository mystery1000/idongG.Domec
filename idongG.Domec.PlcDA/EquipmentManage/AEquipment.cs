using idongG.Domec.PlcDA.Extend;
using idongG.Domec.PlcDA.ToolManage;
using System.ComponentModel;

namespace idongG.Domec.PlcDA.EquipmentManage;

/// <summary>
/// 设备基类，代表一个设备
/// </summary>
/// <remarks>
/// 构造函数
/// </remarks>
/// <param name="name">设备名称</param>
/// <param name="deviceName">设备名称</param>
/// <param name="communicationToolName">通信工具名称</param>
public abstract class AEquipment(string nickName) : IEquipment
{
    protected internal string _deviceName = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    public string DeviceName => _deviceName;

    /// <summary>
    /// 通信工具名称
    /// </summary>
    public string CommunicationToolName { get; set; } = string.Empty;

    /// <summary>
    /// 设备启用状态
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    public string IP { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 80;
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NickName { get; set; } = nickName;

    /// <summary>
    /// 心跳输入
    /// </summary>
    public PlcAddress InHeartAddress { get; set; }

    /// <summary>
    /// 心跳输出
    /// </summary>
    public PlcAddress OutHeartAddress { get; set; }

    public IReadOnlyList<PlcAddress> PlcAddresses { get; set; } = new List<PlcAddress>();
    private ModbusClientTool? tool = null;

    /// <summary>
    /// 初始化设备
    /// </summary>
    public virtual bool Initialize()
    {
        // 默认实现不执行任何操作
        tool = (ModbusClientTool)ToolManager.Instance.GetEntityByNameOrId(CommunicationToolName);
        if (tool == null) return false;
        if (!tool.IsConnect) return tool.Connect();
        return true;

        // 子类可以根据需要重写此方法以执行特定的初始化逻辑
    }

    /// <summary>
    /// 监听取消令牌源
    /// </summary>
    private CancellationTokenSource? _listenCancellationTokenSource = null;

    /// <summary>
    /// 监听任务
    /// </summary>
    private Task? _listenTask = null;

    /// <summary>
    /// 启动设备监听
    /// </summary>
    public bool StartListen()
    {
        // 创建取消令牌源
        _listenCancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _listenCancellationTokenSource.Token;

        // 启动监听任务
        _listenTask = Task.Run(() =>
        {
            // 根据工具名称从ToolManager中获取对应的工具
            tool = (ModbusClientTool)ToolManager.Instance.GetEntityByNameOrId(CommunicationToolName);

            while (!cancellationToken.IsCancellationRequested)
            {
                if (tool == null)
                {
                    // 如果工具不存在，等待一段时间后继续
                    100.Sleep();
                    continue;
                }

                // 连接工具
                if (!tool.IsConnect)
                {
                    tool.Connect();
                }
                if (InHeartAddress != null)
                {
                    var value = ReadPlcValue(tool, InHeartAddress.Address, InHeartAddress.DataType, InHeartAddress.DataLength);
                    InHeartAddress.currentValue = value ;
                }
                if (OutHeartAddress != null)
                {
                    var value = ReadPlcValue(tool, OutHeartAddress.Address, OutHeartAddress.DataType, OutHeartAddress.DataLength);
                    OutHeartAddress.currentValue = value;
                }

                // 遍历PlcAddresses表中的每个元素
                foreach (var plcAddress in PlcAddresses)
                {
                    // 根据地址、数据类型和数据长度读取PLC的值
                    // 这里需要根据具体的工具类型实现读取逻辑
                    // 由于不同工具的读取方式可能不同，这里只是一个示例
                    if (plcAddress != null)
                    {
                        var value = ReadPlcValue(tool, plcAddress.Address, plcAddress.DataType, plcAddress.DataLength);
                        plcAddress.currentValue = value;
                    }
                }

                // 等待一段时间后继续监听
                100.Sleep();
            }
        }, cancellationToken);
        return true;
    }

    /// <summary>
    /// 停止设备监听
    /// </summary>
    public void StopListen()
    {
        // 取消监听任务
        _listenCancellationTokenSource?.Cancel();
        // 释放资源
        _listenCancellationTokenSource?.Dispose();
        _listenCancellationTokenSource = null;
        _listenTask = null;
    }

    /// <summary>
    /// 读取PLC值
    /// </summary>
    /// <param name="tool">工具</param>
    /// <param name="address">地址</param>
    /// <param name="dataType">数据类型</param>
    /// <param name="dataLength">数据长度</param>
    /// <returns>PLC值</returns>
    private dynamic? ReadPlcValue(ModbusClientTool tool, string address, InovancePlcDataType dataType, int dataLength)
    {
        // 这里需要根据具体的工具类型实现读取逻辑
        // 由于不同工具的读取方式可能不同，这里只是一个示例
        // 在实际实现中，需要根据tool的具体类型调用相应的读取方法

        // 例如，如果tool是ModbusClientTool类型，可以调用其读取方法
        // 以下代码仅为示例，实际实现需要根据具体工具类进行调整

        // 根据数据类型和数据长度调用相应的读取方法
        switch (dataType)
        {
            case InovancePlcDataType.Bit:
                return tool.ReadCoils(address.GetAddressIntPart(), 1)[0];

            case InovancePlcDataType.Byte:
            //  return tool.ReadInputRegisterAsync(address);
            case InovancePlcDataType.Word:
            //   return tool.ReadHoldingRegisterAsync(address);
            case InovancePlcDataType.DWord:
            //   return tool.ReadHoldingRegistersAsync(address, 2);
            case InovancePlcDataType.Short:
                return tool.ReadHoldRegists(address.GetAddressIntPart(), dataLength);

            case InovancePlcDataType.UShort:
            // return tool.ReadHoldingRegisterAsUShortAsync(address);
            case InovancePlcDataType.Int:
                return tool.ReadHoldRegistsInteger(address.GetAddressIntPart(), dataLength);

            case InovancePlcDataType.UInt:
            // return tool.ReadHoldingRegistersAsUIntAsync(address);

            case InovancePlcDataType.Long:
            // return tool.ReadHoldingRegistersAsLongAsync(address);

            case InovancePlcDataType.ULong:
            //return tool.ReadHoldingRegistersAsULongAsync(address);

            case InovancePlcDataType.Float:
            // return tool.ReadHoldingRegistersAsFloatAsync(address);

            case InovancePlcDataType.Double:
            // return tool.ReadHoldingRegistersAsDoubleAsync(address);

            case InovancePlcDataType.String:
                return tool.ReadString(address.GetAddressIntPart(), dataLength);

            default:
                throw new NotSupportedException($"不支持的数据类型: {dataType}");
        }
    }

    /// <summary>
    /// 重命名设备
    /// </summary>
    /// <param name="newName">新名称</param>
    /// <returns>重命名成功返回true，名称重复或无效返回false</returns>
    public bool Rename(string newName)
    {
        // 检查新名称是否有效
        if (string.IsNullOrWhiteSpace(newName) || newName == this.NickName)
        {
            return false;
        }

        // 检查新名称是否与系统中其他设备重复
        if (EquipmentManager.Instance.GetAllNames().Contains(newName))
        {
            return false;
        }

        this.NickName = newName;
        return true;
    }

    /// <summary>
    /// 释放设备资源
    /// </summary>
    public virtual void Dispose()
    {
        // 停止监听任务
        StopListen();
        // 释放工具资源
        if (tool != null)
        {
            tool.Dispose();
        }
    }

    public bool SetInHeartAddress(PlcAddress inHeartAddress)
    {
        // 检查参数是否有效
        if (inHeartAddress == null || inHeartAddress.InOrOut != PlcAddress.InOutType.Input)
            return false;
        // 设置输入心跳地址
        InHeartAddress = inHeartAddress;
        return true;
    }

    public bool SetOutHeartAddress(PlcAddress outHeartAddress)
    {
        // 检查参数是否有效
        if (outHeartAddress == null || outHeartAddress.InOrOut != PlcAddress.InOutType.Output)
            return false;
        // 设置输出心跳地址
        OutHeartAddress = outHeartAddress;
        return true;
    }

    /// <summary>
    /// 添加PLC地址
    /// </summary>
    /// <param name="plcAddress">PLC地址</param>
    /// <returns>添加成功返回true，否则返回false</returns>
    public bool AddPlcNormalAddress(PlcAddress plcAddress)
    {
        // 检查参数是否有效
        if (plcAddress == null)
            return false;

        // 检查是否已存在同名地址
        foreach (var address in PlcAddresses)
        {
            if (address.Name == plcAddress.Name)
                return false;
        }

        // 添加地址
        ((List<PlcAddress>)PlcAddresses).Add(plcAddress);
        return true;
    }

    /// <summary>
    /// 删除PLC地址
    /// </summary>
    /// <param name="addressName">地址名称</param>
    /// <returns>删除成功返回true，否则返回false</returns>
    public bool RemovePlcNormalAddress(string addressName)
    {
        // 查找要删除的地址
        for (int i = 0; i < PlcAddresses.Count; i++)
        {
            if (PlcAddresses[i].Name == addressName)
            {
                ((List<PlcAddress>)PlcAddresses).RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 重命名PLC地址
    /// </summary>
    /// <param name="oldName">原地址名称</param>
    /// <param name="newName">新地址名称</param>
    /// <returns>重命名成功返回true，否则返回false</returns>
    public bool RenamePlcNormalAddress(string oldName, string newName)
    {
        // 检查新名称是否有效
        if (newName.IsNullOrEmpty() || newName == oldName)
            return false;

        // 查找原地址并检查新名称是否已存在
        PlcAddress targetAddress = null;
        foreach (var address in PlcAddresses)
        {
            if (address.Name == oldName)
                targetAddress = address;

            // 检查新名称是否已存在
            if (address.Name == newName)
                return false;
        }

        // 如果找不到原地址，返回false
        if (targetAddress == null)
            return false;

        // 重命名地址
        targetAddress.Name = newName;
        return true;
    }

    /// <summary>
    /// 更新PLC地址
    /// </summary>
    /// <param name="addressName">地址名称</param>
    /// <param name="newAddress">新地址值</param>
    /// <param name="dataLength">数据长度</param>
    /// <param name="dataType">数据类型</param>
    /// <returns>更新成功返回true，否则返回false</returns>
    public bool UpdatePlcNormalAddress(string addressName, string newAddress, int dataLength, InovancePlcDataType dataType)
    {
        // 查找要更新的地址
        foreach (var plcAddress in PlcAddresses)
        {
            if (plcAddress.Name == addressName)
            {
                plcAddress.Address = newAddress;
                plcAddress.DataLength = dataLength;
                plcAddress.DataType = dataType;
                return true;
            }
        }

        return false;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)  //属性变更通知
    {
        try
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        catch (Exception)
        {
        }
    }
}