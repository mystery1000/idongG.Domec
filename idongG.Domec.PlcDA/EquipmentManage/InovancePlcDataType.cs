namespace idongG.Domec.PlcDA.EquipmentManage;

/// <summary>
/// 汇川PLC数据类型枚举
/// </summary>
public enum InovancePlcDataType
{
    /// <summary>
    /// 位（布尔值）
    /// </summary>
    Bit,

    /// <summary>
    /// 字节
    /// </summary>
    Byte,

    /// <summary>
    /// 字
    /// </summary>
    Word,

    /// <summary>
    /// 双字
    /// </summary>
    DWord,

    /// <summary>
    /// 短整型
    /// </summary>
    Short,

    /// <summary>
    /// 无符号短整型
    /// </summary>
    UShort,

    /// <summary>
    /// 整型
    /// </summary>
    Int,

    /// <summary>
    /// 无符号整型
    /// </summary>
    UInt,

    /// <summary>
    /// 长整型
    /// </summary>
    Long,

    /// <summary>
    /// 无符号长整型
    /// </summary>
    ULong,

    /// <summary>
    /// 浮点数
    /// </summary>
    Float,

    /// <summary>
    /// 双精度浮点数
    /// </summary>
    Double,

    /// <summary>
    /// 字符串
    /// </summary>
    String
}