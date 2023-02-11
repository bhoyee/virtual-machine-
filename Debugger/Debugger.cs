namespace Debug;

/// <summary>
/// Debugger instance, uses WinForms as graphical representation.
/// </summary>
public sealed class Debugger : IDebugger
{
    #region TASK 5 - TO BE IMPLEMETED BY THE STUDENT
    /// <summary>
    /// This Reference to a WinForms window, on which 
    /// Stack and Frame dumps are shown
    /// </summary>
    private DebugWindow _window = null;

    /// <summary>
    /// Reference to IVirtualMachine, from which it was invoked.
    /// </summary>
    private IVirtualMachine _virtualMachine = null;

    /// <summary>
    /// Determines if user clicked on 'Continue' button,
    /// or closed the Debug View window itself.
    /// </summary>
    private bool _isContinued = false;

    /// <summary>
    /// Determines if the Debug View window is initialized or not.
    /// Used to set timeouts, for the code which is following to instanciation of it.
    /// </summary>
    private bool _isWindowInitialized = false;

    /// <summary>
    /// Assigns a reference to the virtual machine
    /// which was invoked this debugger.
    /// </summary>
    public IVirtualMachine VirtualMachine
    {
        set => _virtualMachine = value;
    }

    /// <summary>
    /// Creates the new instance of DebugWindow.
    /// </summary>
    /// <param name="parameter">Not used.</param>
    private void CreateDebugViewWindow(object parameter)
    {
        Application.EnableVisualStyles();
        Application.Run(_window = new DebugWindow(this));
    }

    /// <summary>
    /// Updates the Debug View Window with new frame.
    /// If Debug View Window is closed, creates new instance of it.
    /// </summary>
    /// <param name="debugFrame">New frame which must be shown.</param>
    private void UpdateDebugWindow(IDebugFrame debugFrame)
    {
        if (_window == null)
        {
            ThreadPool.QueueUserWorkItem(CreateDebugViewWindow);
            // making some timeouts, before the Update method of the this._window is called.
            for (int i = 1; !_isWindowInitialized && i <= 5; i++)
                Thread.Sleep(500);
        }
        if (_isWindowInitialized)
        {
            // making the copy of the stack, to be able to retrieve the objects
            // from it for showing them on screen.
            Stack stackCopy = _virtualMachine.Stack.Clone() as Stack;
            _window?.Update(debugFrame, stackCopy);
        }
    }

    /// <summary>
    /// Used by DebugFrame as subscriber method.
    /// </summary>
    public void Continue() =>
        _isContinued = true;

    /// <summary>
    /// Used by DebugFrame as subscriber method.
    /// </summary>
    public void SetWindowIsInitialized() =>
        _isWindowInitialized = true;

    /// <summary>
    /// Used by DebugFrame as subscriber method.
    /// </summary>
    public void SetWindowIsTerminated()
    {
        _window = null;
        Continue();
    }

    /// <summary>
    /// Breaks an executing of the instruction, and dumps the debugging
    /// info on the screen.
    /// </summary>
    public void Break(IDebugFrame debugFrame)
    {
        _isContinued = false;
        _isWindowInitialized = _window != null;
        UpdateDebugWindow(debugFrame);
        // basically yielding the thread, until the window is closed
        // or the 'Continue' button is pressed.
        while (!_isContinued && _window != null)
            Thread.Yield();
    }
    #endregion
}