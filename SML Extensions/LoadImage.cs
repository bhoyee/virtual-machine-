namespace SML.Extensions;

/// <summary>
/// Implements the SML Extension LoadImage instruction.
/// Retrieves image path from the top of the stack,
/// loads image, and pushes it on to the stack.
/// </summary>
public class LoadImage : BaseInstructionWithOperand
{
    public override void Run()
    {
        try
        {
            var imagePath = this.Operands[0];
            this.VirtualMachine.Stack.Push(Image.FromFile(imagePath));
        }
        catch (InvalidCastException)
        {
            throw new SvmRuntimeException(string.Format(OperandOfWrongTypeMessage,
                this.ToString(), this.VirtualMachine.ProgramCounter));
        }
        catch (Exception e)
        {
            throw new SvmRuntimeException($"{VirtualMachineErrorMessage}: {e.Message}");
        }
    }
}