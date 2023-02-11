namespace SML.Extensions;

/// <summary>
/// Implements the SML Extension DisplayImage instruction.
/// Retrieves Image object from the top of the stack
/// and shows it on newly created form.
/// </summary>
public class DisplayImage : BaseInstruction
{
    public override void Run()
    {
        try
        {
            if (this.VirtualMachine.Stack.Count < 1)
                throw new SvmRuntimeException(StackIsEmptyMessage);
            var image = (Image)this.VirtualMachine.Stack.Pop();
            Application.Run(new ImageViewer(image));
        }
        catch (InvalidCastException)
        {
            throw new SvmRuntimeException(String.Format(OperandOfWrongTypeMessage,
                this.ToString(), this.VirtualMachine.ProgramCounter));
        }
        catch (Exception e)
        {
            throw new SvmRuntimeException($"{VirtualMachineErrorMessage}: {e.Message}");
        }
    }
}