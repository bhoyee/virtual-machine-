namespace SVM.SimpleMachineLanguage;

/// <summary>
/// Implements the SML BgrInt instruction.
/// Peeks integer value from the top of the
/// stack and checks if the integer specified as operand is greater than the one from stack.
/// If it is greater, then the jump on label specified in second operand is performed.
/// </summary>
public class BgrInt : BaseInstructionWithOperand
{
    public override void Run()
    {
        try
        {
            if (this.VirtualMachine.Stack.Count < 1)
                throw new SvmRuntimeException(StackIsEmptyMessage);
            int lValue = (int)this.VirtualMachine.Stack.Peek();
            int rValue = Convert.ToInt32(this.Operands[0]);
            if (!this.VirtualMachine.LabelEntries.ContainsKey(this.Operands[1]))
                throw new SvmRuntimeException(string.Format(LabelIsNotDeclaredMessage,
                    this.Operands[1], this.VirtualMachine.ProgramCounter));
            if (rValue > lValue)
                this.VirtualMachine.ProgramCounter = this.VirtualMachine.LabelEntries[this.Operands[1]];
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

