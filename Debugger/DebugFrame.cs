namespace Debug;

/// <summary>
/// IDebugFrame type implementation.
/// Just implements the IDebugFrame interface, no more.
/// </summary>
public sealed class DebugFrame : IDebugFrame
{
    private IInstruction _instruction;
    private List<IInstruction> _frame;

    public IInstruction CurrentInstruction { get => _instruction; }
    public List<IInstruction> CodeFrame { get => _frame; }

    public DebugFrame(IInstruction instruction, List<IInstruction> frame)
    {
        this._instruction = instruction;
        this._frame = frame;
    }
}