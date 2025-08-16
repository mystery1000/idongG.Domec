using idongG.Domec.PlcDA.Logs;
using idongG.Domec.PlcDA.ToolManage.Modbus.EasyModbus;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net;
using System.Text;

namespace idongG.Domec.PlcDA.ToolManage
{
    public class ModbusServerTool : ATool
    {
        public override void Start()
        {
            // 启动逻辑
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public static string Modbus服务端 = nameof(Modbus服务端);
        [JsonIgnore] private ModbusServer server;
        private bool isOpen;
        [Browsable(false)][JsonIgnore] public bool IsOpen => isOpen;

        public int Timeout { get; private set; }
        public int StationNo { get; private set; }

        public ModbusServerTool()
        {
            if (string.IsNullOrEmpty(NickName))
            {
                NickName = Modbus服务端;
            }

            server = new ModbusServer();
            IP = "127.0.0.1";
            Port = 502;
            Timeout = 300;
            StationNo = 1;
        }

        /// <summary>
        ///    int f = 10_0000_0000;
        ///  int s = (int)f / 32768;
        /// int si = f % 32768;
        /// ser.holdingRegisters[160] = (short) s;
        /// ser.holdingRegisters[161] = (short) si;
        /// </summary>
        /// <param name="adress"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetHoldRegist(int adress, short value)
        {
            server.holdingRegisters[adress] = value;
            return true;
        }

        public bool SetCoil(int adress, bool value)
        {
            server.coils[adress] = value;
            return true;
        }

        /// <summary>
        /// 写两位寄存器
        /// </summary>
        /// <param name="adress"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetHoldRegist2(int adress, int value)
        {
            int s = (int)value / 32768;
            int si = value % 32768;
            server.holdingRegisters[adress] = (short)s;
            server.holdingRegisters[adress + 1] = (short)si;
            return true;
        }

        public short GetHoldRegist(int adress)
        {
            return server.holdingRegisters[adress];
        }

        public bool StartServer()
        {
            try
            {
                server.UnitIdentifier = (byte)StationNo;

                server.LocalIPAddress = IPAddress.Parse(IP);
                server.Port = Port;

                server.Listen();
                server.CoilsChanged += Server_CoilsChanged;
                server.HoldingRegistersChanged += Server_HoldingRegistersChanged;
                server.NumberOfConnectedClientsChanged += Server_NumberOfConnectedClientsChanged;
                isOpen = true;
                return true;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        private void Server_NumberOfConnectedClientsChanged()
        {
        }

        private void Server_HoldingRegistersChanged(int register, int numberOfRegisters)
        {
            var sb = new StringBuilder();

            if (numberOfRegisters > 1)
            {
                for (int i = 0; i < numberOfRegisters; i++) sb.Append($",{server.holdingRegisters[register + i]}");
            }
            else
            {
                sb.Append(server.holdingRegisters[register]);
            }
            ToolManager.Instance.Log.Add($"{NickName}:被设置,寄存器号:{register - 1},寄存器数量:{numberOfRegisters},值:{sb}", EnumMsgType.Good);
        }

        private void Server_CoilsChanged(int coil, int numberOfCoils)
        {
            var sb = new StringBuilder();

            if (numberOfCoils > 1)
            {
                for (int i = 0; i < numberOfCoils; i++) sb.Append($",{server.coils[coil + i]}");
            }
            else
            {
                sb.Append(server.coils[coil]);
            }
            ToolManager.Instance.Log.Add($"{NickName}:被设置 线圈号:{coil - 1},线圈数量:{numberOfCoils},值:{sb}", EnumMsgType.Good);
        }

        public bool StopServer()
        {
            if (isOpen)
            {
                server?.StopListening();
            }

            isOpen = false;
            return true;
        }
    }
}