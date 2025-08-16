using System.Runtime.Serialization;

namespace idongG.Domec.PlcDA.ToolManage.Modbus.EasyModbus.Exceptions
{
    /// <summary>
	/// Exception to be thrown if CRC Check failed
	/// </summary>
	public class CRCCheckFailedException : ModbusException
    {
        protected CRCCheckFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CRCCheckFailedException() : base()
        {
        }

        public CRCCheckFailedException(string message) : base(message)
        {
        }

        public CRCCheckFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}