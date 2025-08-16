using System;

namespace idongG.Domec.PlcDA.EquipmentManage.DomecEquipment;

public class UploadMeterialEquipment : AEquipment
{
    /// <summary>
    /// 上料机
    /// </summary>
    private const string 上料机 = nameof(上料机);

    /// <summary>
    /// 上传物料设备构造函数
    /// </summary>
    public UploadMeterialEquipment() : base("设备昵称")
    {
        Id = Guid.NewGuid();
        _deviceName = 上料机;
    }

    /// <summary>
    /// 初始化设备
    /// </summary>
    public override bool Initialize()
    {
        // 设备初始化逻辑
        return true;
    }
}