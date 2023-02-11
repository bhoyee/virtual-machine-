namespace SVM.SimpleMachineLanguage;

/// <summary>
/// Implements the SML Incr  instruction
/// Increments the integer value stored on top of the stack, 
/// leaving the result on the stack
/// </summary>
public class Incr : BaseInstruction
{
    public override void Run()
    {
        try
        {
            if (this.VirtualMachine.Stack.Count < 1)
                throw new SvmRuntimeException(StackIsEmptyMessage);
            var _topValue = (int)this.VirtualMachine.Stack.Pop();
            this.VirtualMachine.Stack.Push(_topValue + 1);
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