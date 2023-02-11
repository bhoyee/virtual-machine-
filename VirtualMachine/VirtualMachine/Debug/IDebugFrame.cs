namespace SVM.VirtualMachine.Debug;

public interface IDebugFrame
{
    /// <summary>
    /// Contains a reference to the CurrentInstruction which the Virtual Machine is 
    /// intending to execute. When the debugger breaks at the instruction, the instruction
    /// will not yet have been executed
    /// </summary>
    IInstruction CurrentInstruction { get; }

    /// <summary>
    /// Returns a list of a maximum of 9 instructions, containing the current instruction 
    /// and up to a maximum of 4 instructions preceding the current instruction, and up to
    /// a maximum of 4 instructions succeeding the current instruction
    /// </summary>
    List<IInstruction> CodeFrame { get; }
}
