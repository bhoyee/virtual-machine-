namespace SVM.VirtualMachine;

/// <summary>
/// Defines the interface contract for all Simple 
/// Virtual Machine instructions
/// </summary>
public interface IInstruction
{
    /// <summary>
    /// Executes the instruction
    /// </summary>
    void Run();

    /// <summary>
    /// Contains the information about the breakpoint 
    /// at this particular instruction.
    /// </summary>
    bool HasBreakPoint { get; set; }

    /// <summary>
    /// Assigns a reference to the virtual machine 
    /// that is executing this instruction
    /// </summary>
    IVirtualMachine VirtualMachine { set; }
}
