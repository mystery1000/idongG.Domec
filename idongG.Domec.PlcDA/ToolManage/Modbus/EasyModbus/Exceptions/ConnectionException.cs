using System.Runtime.Serialization;

namespace idongG.Domec.PlcDA.ToolManage.Modbus.EasyModbus.Exceptions
{
    /// <summary>
    /// Exception to be thrown if Connection to Modbus device failed
    /// </summary>
    public class ConnectionException : ModbusException
    {
        protected ConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ConnectionException() : base()
        {
        }

        public ConnectionException(string message) : base(message)
        {
        }

        public ConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}