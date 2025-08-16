using System.Runtime.Serialization;

namespace idongG.Domec.PlcDA.ToolManage.Modbus.EasyModbus.Exceptions;

/// <summary>
/// Exception to be thrown if Modbus Server returns error code "Function code not supported"
/// </summary>
public class FunctionCodeNotSupportedException : ModbusException
{
    protected FunctionCodeNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public FunctionCodeNotSupportedException() : base()
    {
    }

    public FunctionCodeNotSupportedException(string message) : base(message)
    {
    }

    public FunctionCodeNotSupportedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}