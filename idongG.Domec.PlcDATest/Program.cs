// See https://aka.ms/new-console-template for more information
using idongG.Domec.PlcDA.EquipmentManage;
using idongG.Domec.PlcDA.EquipmentManage.DomecEquipment;
using idongG.Domec.PlcDA.Extend;
using idongG.Domec.PlcDA.ToolManage;

Console.WriteLine($"当前版本: {typeof(Program).Assembly.GetName().Version}");

FilePath.CheckFileExit();

#region NameCenter

string 装框机1 = nameof(装框机1);
string 装框机2 = nameof(装框机2);
string 装框机3 = nameof(装框机3);

string 小胖胖PLCClient1 = nameof(小胖胖PLCClient1);
string 小胖胖PLCClient2 = nameof(小胖胖PLCClient2);
string 小胖胖PLCClient3 = nameof(小胖胖PLCClient3);

string 小胖胖PLCServer1 = nameof(小胖胖PLCServer1);
string 小胖胖PLCServer2 = nameof(小胖胖PLCServer2);
string 小胖胖PLCServer3 = nameof(小胖胖PLCServer3);

#endregion

//创 建 三 台设备 /
EquipmentManager.Instance.Add<UploadMeterialEquipment>(装框机1);
EquipmentManager.Instance.Add<UploadMeterialEquipment>(装框机2);
EquipmentManager.Instance.Add<UploadMeterialEquipment>(装框机3);

//创建三个连接工具
ToolManager.Instance.Add<ModbusClientTool>(小胖胖PLCClient1);
ToolManager.Instance.Add<ModbusClientTool>(小胖胖PLCClient2);
ToolManager.Instance.Add<ModbusClientTool>(小胖胖PLCClient3);

//模拟添加三个PLC工具
ToolManager.Instance.Add<ModbusServerTool>(小胖胖PLCServer1);
ToolManager.Instance.Add<ModbusServerTool>(小胖胖PLCServer2);
ToolManager.Instance.Add<ModbusServerTool>(小胖胖PLCServer3);

//得到三台设备的实例
var equipment1 = EquipmentManager.Instance.GetEntityByNameOrId(装框机1) as UploadMeterialEquipment;
var equipment2 = EquipmentManager.Instance.GetEntityByNameOrId(装框机2) as UploadMeterialEquipment;
var equipment3 = EquipmentManager.Instance.GetEntityByNameOrId(装框机3) as UploadMeterialEquipment;
//得到三个连接工具的实例
var tool1 = ToolManager.Instance.GetEntityByNameOrId(小胖胖PLCClient1) as ModbusClientTool;
var tool2 = ToolManager.Instance.GetEntityByNameOrId(小胖胖PLCClient2) as ModbusClientTool;
var tool3 = ToolManager.Instance.GetEntityByNameOrId(小胖胖PLCClient3) as ModbusClientTool;
//设置三个连接工具的IP和端口
tool1.IP = "127.0.0.1";
tool1.Port = 502;
tool2.IP = "127.0.0.1";
tool2.Port = 503;
tool3.IP = "127.0.0.1";
tool3.Port = 504;
//得到三个Server的实例
var server1 = ToolManager.Instance.GetEntityByNameOrId(小胖胖PLCServer1) as ModbusServerTool;
var server2 = ToolManager.Instance.GetEntityByNameOrId(小胖胖PLCServer2) as ModbusServerTool;
var server3 = ToolManager.Instance.GetEntityByNameOrId(小胖胖PLCServer3) as ModbusServerTool;

//启动三个服务
server1.IP = "127.0.0.1";
server1.Port = 502;
var b1 = server1.StartServer();
if (b1)
{
    Console.WriteLine($"{server1.NickName}启动成功");
}
else
{
    Console.WriteLine($"{server1.NickName}启动失败!!!");
}

server2.IP = "127.0.0.1";
server2.Port = 503;
var b2 = server2.StartServer();
if (b2)
{
    Console.WriteLine($"{server2.NickName}启动成功");
}
else
{
    Console.WriteLine($"{server2.NickName}启动失败!!!");
}

server3.IP = "127.0.0.1";
server3.Port = 504;
var b3 = server3.StartServer();
if (b3)
{
    Console.WriteLine($"{server3.NickName}启动成功");
}
else
{
    Console.WriteLine($"{server3.NickName}启动失败!!!");
}

//绑定设备和工具
equipment1.CommunicationToolName = tool1.NickName;
equipment2.CommunicationToolName = tool2.NickName;
equipment3.CommunicationToolName = tool3.NickName;

// 设置心跳地址
equipment1.SetInHeartAddress(new PlcAddress("心跳", "M1500", PlcAddress.InOutType.Input, 1, InovancePlcDataType.Bit));
equipment2.SetInHeartAddress(new PlcAddress("心跳", "M1500", PlcAddress.InOutType.Input, 1, InovancePlcDataType.Bit));
equipment3.SetInHeartAddress(new PlcAddress("心跳", "M1500", PlcAddress.InOutType.Input, 1, InovancePlcDataType.Bit));
//保存配置
EquipmentManager.Instance.Save();
ToolManager.Instance.Save();

//设备启动监听
equipment1.StartListen();
equipment2.StartListen();
equipment3.StartListen();

for (int i = 0; i < 1000; i++)
{
    //手动修改服务器的线圈值，模拟心跳
    server1.SetCoil(1501, value: true);
    100.Sleep();
    //读取心跳地址的值
    Console.WriteLine($"{equipment1.NickName}:{equipment1.InHeartAddress.GetValue<bool>()}");
    100.Sleep();
    //手动修改服务器的线圈值，模拟心跳
    server1.SetCoil(1501, false);
    100.Sleep();
    //读取心跳地址的值
    Console.WriteLine($"{equipment1.NickName}:{equipment1.InHeartAddress.GetValue<bool>()}");

    //手动修改服务器的线圈值，模拟心跳
    server2.SetCoil(1501, value: true);
    100.Sleep();
    //读取心跳地址的值
    Console.WriteLine($"{equipment2.NickName}:{equipment2.InHeartAddress.GetValue<bool>()}");
    100.Sleep();
    //手动修改服务器的线圈值，模拟心跳
    server2.SetCoil(1501, false);
    100.Sleep();
    //读取心跳地址的值
    Console.WriteLine($"{equipment2.NickName}:{equipment2.InHeartAddress.GetValue<bool>()}");

    //手动修改服务器的线圈值，模拟心跳
    server3.SetCoil(1501, value: true);
    100.Sleep();
    //读取心跳地址的值
    Console.WriteLine($"{equipment3.NickName}:{equipment3.InHeartAddress.GetValue<bool>()}");
    100.Sleep();
    //手动修改服务器的线圈值，模拟心跳
    server3.SetCoil(1501, false);
    100.Sleep();
    //读取心跳地址的值
    Console.WriteLine($"{equipment3.NickName}:{equipment3.InHeartAddress.GetValue<bool>()}");
}

Console.ReadLine();