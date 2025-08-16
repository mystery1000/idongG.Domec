using System;

namespace idongG.Domec.PlcDA.EquipmentManage;

/// <summary>
/// PLC地址信息类
/// </summary>
/// <remarks>
/// 构造函数
/// </remarks>
/// <param name="name">地址名称</param>
/// <param name="address">地址值</param>
/// <param name="dataLength">数据长度</param>
/// <param name="dataType">数据类型</param>
public class PlcAddress(string name, string address, PlcAddress.InOutType inOut, int dataLength, InovancePlcDataType dataType)
{
    public enum InOutType
    {
        Input,
        Output
    }

    /// <summary>
    /// 输入输出类型
    /// </summary>
    public InOutType InOrOut { get; set; } = inOut;

    /// <summary>
    /// 地址名称
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// 地址值
    /// </summary>
    public string Address { get; set; } = address;

    /// <summary>
    /// 数据长度
    /// </summary>
    public int DataLength { get; set; } = dataLength;

    /// <summary>
    /// 数据类型
    /// </summary>
    public InovancePlcDataType DataType { get; set; } = dataType;

    /// <summary>
    /// 当前值
    /// </summary>
    public dynamic? CurrentValue => currentValue;

    internal dynamic? currentValue;

    /// <summary>
    /// 根据数据类型返回当前值
    /// </summary>
    /// <returns>与DataType匹配的值</returns>
    public T GetValue<T>()
    {
        if (CurrentValue == null)
            return default(T);

        // 根据DataType枚举进行类型转换
        switch (DataType)
        {
            case InovancePlcDataType.Bit:
                return Convert.ChangeType(CurrentValue, typeof(bool));

            case InovancePlcDataType.Byte:
                return Convert.ChangeType(CurrentValue, typeof(byte));

            case InovancePlcDataType.Word:
                return Convert.ChangeType(CurrentValue, typeof(ushort));

            case InovancePlcDataType.DWord:
                return Convert.ChangeType(CurrentValue, typeof(uint));

            case InovancePlcDataType.Short:
                return Convert.ChangeType(CurrentValue, typeof(short));

            case InovancePlcDataType.UShort:
                return Convert.ChangeType(CurrentValue, typeof(ushort));

            case InovancePlcDataType.Int:
                return Convert.ChangeType(CurrentValue, typeof(int));

            case InovancePlcDataType.UInt:
                return Convert.ChangeType(CurrentValue, typeof(uint));

            case InovancePlcDataType.Long:
                return Convert.ChangeType(CurrentValue, typeof(long));

            case InovancePlcDataType.ULong:
                return Convert.ChangeType(CurrentValue, typeof(ulong));

            case InovancePlcDataType.Float:
                return Convert.ChangeType(CurrentValue, typeof(float));

            case InovancePlcDataType.Double:
                return Convert.ChangeType(CurrentValue, typeof(double));

            case InovancePlcDataType.String:
                return Convert.ChangeType(CurrentValue, typeof(string));

            default:
                return CurrentValue;
        }
    }
}