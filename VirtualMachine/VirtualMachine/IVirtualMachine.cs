namespace SVM.VirtualMachine;

public interface IVirtualMachine
{
    /// <summary>
    ///  Gets a reference to the virtual machine stack.
    ///  This is used by executing instructions to retrieve
    ///  operands and store results
    /// </summary>
    public Stack Stack { get; }

    /// <summary>
    /// Accesses the virtual machine program counter.
    /// This can be used by executing instructions to 
    /// determine their order (ie. line number) in the 
    /// sequence of executing SML instructions
    /// </summary>
    public int ProgramCounter { get; set; }

    /// <summary>
    /// Gets a reference to Label Entries, which
    /// is basically the string->int key-value pair, 
    /// where the string value is the label's identifier,
    /// and int value is the line on which this label was declared.
    /// </summary>
    public Dictionary<string, int> LabelEntries { get; }

    /// <summary>
    /// Executes every SML instruction step-by-step.
    /// </summary>
    public void Run();
}