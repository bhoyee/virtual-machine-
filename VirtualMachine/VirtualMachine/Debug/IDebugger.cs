namespace SVM.VirtualMachine.Debug;

public interface IDebugger
{
    /// <summary>
    /// When program execution hits a breakpoint, this method is called
    /// and should display the debugger window, populating the Code section with
    /// the contents of the debugFrame.CodeFrame, and the Stack section with the
    /// contents of the virtual machine stack. The current instruction should be 
    /// visibly highlighted in the Code section of the Debugger window
    /// </summary>
    /// <param name="debugFrame">An IDebugFrame instance which is sent to the debugger</param>
    void Break(IDebugFrame debugFrame);

    /// <summary>
    /// Assigns a reference to the virtual machine 
    /// that is executing this instruction
    /// </summary>
    IVirtualMachine VirtualMachine { set; }
}
