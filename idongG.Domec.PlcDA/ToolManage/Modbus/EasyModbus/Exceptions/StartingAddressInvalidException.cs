using System.Runtime.Serialization;

namespace idongG.Domec.PlcDA.ToolManage.Modbus.EasyModbus.Exceptions;

/// <summary>
/// Exception to be thrown if Modbus Server returns error code "starting adddress and quantity invalid"
/// </summary>
public class StartingAddressInvalidException : ModbusException
{
    protected StartingAddressInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public StartingAddressInvalidException() : base()
    {
    }

    public StartingAddressInvalidException(string message) : base(message)
    {
    }

    public StartingAddressInvalidException(string message, Exception innerException) : base(message, innerException)
    {
    }
}