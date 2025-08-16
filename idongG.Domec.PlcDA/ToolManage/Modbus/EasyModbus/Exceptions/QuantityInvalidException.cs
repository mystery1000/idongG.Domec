using System.Runtime.Serialization;

namespace idongG.Domec.PlcDA.ToolManage.Modbus.EasyModbus.Exceptions;

/// <summary>
/// Exception to be thrown if Modbus Server returns error code "quantity invalid"
/// </summary>
public class QuantityInvalidException : ModbusException
{
    protected QuantityInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public QuantityInvalidException() : base()
    {
    }

    public QuantityInvalidException(string message) : base(message)
    {
    }

    public QuantityInvalidException(string message, Exception innerException) : base(message, innerException)
    {
    }
}