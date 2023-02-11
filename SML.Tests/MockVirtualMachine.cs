namespace SML.Tests;

internal class MockVirtualMachine : IVirtualMachine
{
    private Stack _stack;
    private int _pc;
    private List<IInstruction> _program;

    private Dictionary<string, int> _labels;

    public Stack Stack 
    { 
        get => this._stack; 
    }

    public int ProgramCounter 
    { 
        get => this._pc; 
        set => this._pc = value; 
    }

    public Dictionary<string, int> LabelEntries
    {
        get => this._labels;
    }

    public MockVirtualMachine()
    {
        this._stack = new Stack();
        this._labels = new Dictionary<string, int>();
        this._program = new List<IInstruction>();
        this._pc = 0;
    }

    public IVirtualMachine Load(List<IInstruction> instructions)
    {
        this._stack = new Stack();
        this._pc = 0;
        this._program = instructions;
        foreach (var instruction in instructions)
            instruction.VirtualMachine = this;
        return this;
    }

    public void Run()
    {
        while (this._pc < this._program.Count)
            this._program[this._pc++].Run();
    }
}