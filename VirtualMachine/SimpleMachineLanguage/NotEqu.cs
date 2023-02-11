namespace SVM.SimpleMachineLanguage;

/// <summary>
/// Implements the SML NotEqu instruction.
/// Peeks two values of any type from the top of the
/// stack and checks if they are equal or not (using object.Equals).
/// If they both are not equal, then the jump on label specified in second operand is performed.
/// </summary>
public class NotEqu : BaseInstructionWithOperand
{
    public override void Run()
    {
        try
        {
            if (VirtualMachine.Stack.Count < 1)
                throw new SvmRuntimeException(StackIsEmptyMessage);
            object lValue = this.VirtualMachine.Stack.Pop();
            if (VirtualMachine.Stack.Count < 1)
                throw new SvmRuntimeException(StackIsEmptyMessage);
            object rValue = this.VirtualMachine.Stack.Peek();
            if (!this.VirtualMachine.LabelEntries.ContainsKey(this.Operands[0]))
                throw new SvmRuntimeException(string.Format(LabelIsNotDeclaredMessage,
                    this.Operands[0], this.VirtualMachine.ProgramCounter));
            if (!lValue.Equals(rValue))
                this.VirtualMachine.ProgramCounter = this.VirtualMachine.LabelEntries[this.Operands[0]];
            // basically, to peek two values from stack, we need pop once, anc accumulate it's value
            // then after we made all our processing stuff, push this value back (restore the stack).
            this.VirtualMachine.Stack.Push(lValue);
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

