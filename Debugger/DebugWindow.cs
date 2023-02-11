namespace Debug.Window;

/// <summary>
/// Represents the visual part of Debugger class.
/// </summary>
public partial class DebugWindow : Form
{
    /// <summary>
    /// Reference to debugger object.
    /// </summary>
    private Debugger _backEnd;

    public DebugWindow(Debugger backEnd)
    {
        this._backEnd = backEnd;
        this.InitializeComponent();
        // subscribes Debugger's methods to some useful events
        // to control the backend (Debugger) with fronend (this form).
        this.ContinueBtn.Click += this.UpdateBackEnd;
        this.Shown += (o, s) => backEnd.SetWindowIsInitialized();
        this.FormClosed += (o, s) => backEnd.SetWindowIsTerminated();
    }

    /// <summary>
    /// Updates the screen with new information about the frame and stack.
    /// </summary>
    /// <param name="frame">Frame which will be shown on screen.</param>
    /// <param name="frameStack">Stack which will be shown on screen</param>
    public void Update(IDebugFrame frame, Stack frameStack)
    {
        this.Invoke(() => { 
            this.ContinueBtn.Enabled = true;
            this.UpdateStackView(frameStack);
            this.UpdateCodeFrameView(frame);
        });
    }

    /// <summary>
    /// Updates the Stack ListBox on screen.
    /// </summary>
    /// <param name="frameStack"></param>
    private void UpdateStackView(Stack frameStack)
    {
        this.StackLb.Items.Clear();
        while (frameStack.Count > 0)
            // that's why the copy of stack was needed.
            this.StackLb.Items.Add(frameStack.Pop());
    }

    /// <summary>
    /// Updates the Code Frame ListBox on screen.
    /// </summary>
    /// <param name="frameStack"></param>
    private void UpdateCodeFrameView(IDebugFrame frame)
    {
        this.FrameLb.Items.Clear();
        int index = frame.CodeFrame.IndexOf(frame.CurrentInstruction);
        foreach (var instruction in frame.CodeFrame)
            this.FrameLb.Items.Add(instruction.ToString());
        if (index >= 0)
            this.FrameLb.SelectedIndex = index;
    }
    
    /// <summary>
    /// Sets the 'Continue' to Disabled, and sets the Debugger in continued state.
    /// </summary>
    private void UpdateBackEnd(object sender, EventArgs e)
    {
        this.ContinueBtn.Enabled = false;
        this._backEnd.Continue();
    }
}