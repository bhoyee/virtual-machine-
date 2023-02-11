namespace SVM.SimpleMachineLanguage;

/// <summary>
/// Implements the SML Decr  instruction
/// Decrements the integer value stored on top of the stack, 
/// leaving the result on the stack
/// </summary>
public class Decr : BaseInstruction
{
    public override void Run()
    {
        try
        {
            if (this.VirtualMachine.Stack.Count < 1)
                throw new SvmRuntimeException(StackIsEmptyMessage);
            var stackValue = (int)this.VirtualMachine.Stack.Pop();
            this.VirtualMachine.Stack.Push(stackValue - 1);
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