namespace SVM.VirtualMachine;

/// <summary>
/// Utility class which generates compiles a textual representation
/// of an SML instruction into an executable instruction instance
/// </summary>
internal static class JITCompiler
{
    #region Constants
    #endregion

    #region Fields
    #endregion

    #region Constructors
    #endregion

    #region Properties
    #endregion

    #region Public methods
    #endregion

    #region Non-public methods
    //Activator.CreateInstance() is implemented inside the AssemlyLoader class
    //Finds any IInstruction type implementation, and instanciates it are inside AssemblyLoader class.
    #region TASK 1 - TO BE IMPLEMENTED BY THE STUDENT
    internal static IInstruction CompileInstruction(string opcode) =>
        AssemblyLoader.CreateInstruction(opcode);
    #endregion

  
    #region TASK 1 - TO BE IMPLEMENTED BY THE STUDENT
    internal static IInstruction CompileInstruction(string opcode, params string[] operands) =>
        AssemblyLoader.CreateInstruction(opcode, operands) as IInstructionWithOperand;
    #endregion
    #endregion
}
