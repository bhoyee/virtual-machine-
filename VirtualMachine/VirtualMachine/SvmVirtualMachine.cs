using SVM;
using SVM.VirtualMachine;
using System.Reflection;

/// <summary>
/// Implements the Simple Virtual Machine (SVM) virtual machine 
/// </summary>
public sealed class SvmVirtualMachine : IVirtualMachine
{
    #region Constants
    private const string CompilationErrorMessage = "An SVM compilation error has occurred at line {0}.\r\n\r\n{1}\r\n";
    private const string RuntimeErrorMessage = "An SVM runtime error has occurred.\r\n\r\n{0}\r\n";
    private const string InvalidOperandsMessage = "The instruction \r\n\r\n\t{0}\r\n\r\nis invalid because there are too many operands. An instruction may have no more than one operand.";
    private const string InvalidLabelMessage = "Invalid label: the label {0} at line {1} is not associated with an instruction.";
    private const string DuplicatedLabelMessage = "Invalid label: the label {0} at line {1} is already declared at line {2}.";
    private const string ProgramCounterMessage = "Program counter violation; the program counter value is out of range";
    #endregion

    #region Fields
    private IDebugger _debugger = null;
    private List<IInstruction> program = new List<IInstruction>();
    private Stack _stack = new Stack();
    private int programCounter = 0;


    private Dictionary<string, int> _labels;
    #endregion

    #region Constructors

    public SvmVirtualMachine(string filepath)
    {
        try
        {
            Compile(filepath);
            this._debugger = CreateDebugger();
        }
        catch
        {
            Console.WriteLine("Compilation was not successful. This may be due to errors in JIT compilation or in the sml file loaded. SVM is exiting.");
            return;
        }
    }
    #endregion

    #region Properties
    /// <summary>
    ///  Gets a reference to the virtual machine stack.
    ///  This is used by executing instructions to retrieve
    ///  operands and store results
    /// </summary>
    public Stack Stack
    {
        get => this._stack;
    }

    /// <summary>
    /// Accesses the virtual machine 
    /// program counter (see programCounter in the Fields region).
    /// This can be used by executing instructions to 
    /// determine their order (ie. line number) in the 
    /// sequence of executing SML instructions
    /// </summary>
    public int ProgramCounter
    {
        #region TASK 1 - TO BE IMPLEMENTED BY THE STUDENT
        get => this.programCounter;
        set => this.programCounter = value;
        #endregion
    }

    /// <summary>
    /// Gets a reference to Label Entries, which
    /// is basically the string->int key-value pair, 
    /// where the string value is the label's identifier,
    /// and int value is the line on which this label was declared.
    /// </summary>
    public Dictionary<string, int> LabelEntries
    {
        get => this._labels;
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// Executes a compiled SML program 
    /// </summary>
    /// <exception cref="SvmRuntimeException">
    /// If an unexpected error occurs during
    /// program execution
    /// </exception>
    public void Run()
    {
        #region TASK 2 - TO BE IMPLEMENTED BY THE STUDENT
        while (this.programCounter < this.program.Count)
        {
            #region TASK 5 & 7 - MAY REQUIRE MODIFCATION BY THE STUDENT
            // if instruction has break point
            if (this.program[this.programCounter].HasBreakPoint)
            {
                // we need to create debug frame
                var frame = CreateDebugFrame();
                if (frame != null)
                    // and pass it to debugger, if we have one.
                    this._debugger?.Break(frame);
            }
            #endregion
            // run next instruction
            this.program[this.programCounter++].Run();
           
        }
        #endregion
    }
    #endregion

    #region Non-public Methods

    /// <summary>
    /// Reads the specified file and tries to 
    /// compile any SML instructions it contains
    /// into an executable SVM program
    /// </summary>
    /// <param name="filepath">The path to the 
    /// .sml file containing the SML program to
    /// be compiled</param>
    /// <exception cref="SvmCompilationException">
    /// If file is not a valid SML program file or 
    /// the SML instructions cannot be compiled to an
    /// executable program</exception>
    private void Compile(string filepath)
    {
        int lineNumber = 0;
        if (!File.Exists(filepath))
            throw new SvmCompilationException($"The file {filepath} does not exist");
        try
        {
            this._labels = new Dictionary<string, int>();
            this.program = new List<IInstruction>();
            using (StreamReader sourceFile = new StreamReader(filepath))
            {
                while (!sourceFile.EndOfStream)
                {
                    string instruction = sourceFile.ReadLine();
                    if (!string.IsNullOrEmpty(instruction) &&
                        !string.IsNullOrWhiteSpace(instruction))
                    {
                        ParseInstruction(instruction, lineNumber++);
                    }
                }
            }
        }
        catch (SvmCompilationException err)
        {
            Console.WriteLine(CompilationErrorMessage, lineNumber, err.Message);
            throw;
        }
    }

    /// <summary>
    /// Creates and initializes IDebugger instance.
    /// </summary>    
    /// <returns>Instance of first-found IDebugger type, otherwise null.</returns>
    private IDebugger CreateDebugger()
    {
        var loadedDebugger = AssemblyLoader.CreateDebugger();
        if (loadedDebugger != null)
            loadedDebugger.VirtualMachine = this;
        return loadedDebugger;
    }

    /// <summary>
    /// Creates and initializes IDebugFrame instance.
    /// </summary>
    /// <returns>Instance of first-found IDebugFrame type, otherwise null.</returns>
    private IDebugFrame CreateDebugFrame()
    {
        if (this._debugger == null)
            return null;
        var currentCodeFrame = new List<IInstruction>();
        var currentInstruction = this.program[this.programCounter];
        var startIndex = Math.Max(0, this.programCounter - 4);
        var stopIndex = Math.Min(this.program.Count, this.programCounter + 5);
        for (int i = startIndex; i < stopIndex; i++)
            currentCodeFrame.Add(this.program[i]);
        var loadedFrame = AssemblyLoader.CreateDebugFrame(new object[] {
            currentInstruction, currentCodeFrame
        });
        return loadedFrame;
    }

    /// <summary>
    /// Parses a string from a .sml file containing a single
    /// SML instruction
    /// </summary>
    /// <param name="instruction">The string representation
    /// of an instruction</param>
    private void ParseInstruction(string instruction, int lineNumber)
    {
        #region TASK 5 & 7 - MAY REQUIRE MODIFICATION BY THE STUDENT
        string[] tokens = null;

        bool containsBreakPoint = false;

        if (!instruction.Contains("\""))
        {
            // Tokenize the instruction string by separating on spaces
            tokens = instruction.Split(new char[] { ' ' }, 
                StringSplitOptions.RemoveEmptyEntries);
        }
        else
        {
            var tempo = new List<string>();
            tokens = instruction.Split(new char[] { '\"' }, 
                StringSplitOptions.RemoveEmptyEntries);
            // Remove any unnecessary whitespace
            for (int i = 0; i < tokens.Length-1; i++)
            {
                tokens[i] = tokens[i].Trim();
                tempo.AddRange(tokens[i].Split(' '));
            }
            tempo.Add(tokens[tokens.Length-1]);
            tokens = tempo.ToArray();
        }
        if (tokens.Length > 0 && tokens[0][0] == '*')
        {
            containsBreakPoint = true;
            tokens = tokens.Skip(1).ToArray();
        }
        if (tokens.Length > 0 && tokens[0][0] == '%')
        {
            var splitted = tokens[0].Split('%');
            if (splitted.Length == 3)
            {
                var labelName = splitted[1];
                tokens = tokens.Skip(1).ToArray();
                if (_labels.ContainsKey(labelName))
                    throw new SvmCompilationException(string.Format(DuplicatedLabelMessage,
                        labelName, lineNumber, _labels[labelName]));
                this._labels[labelName] = lineNumber;
            }
        }

        // Ensure the correct number of operands
        if (tokens.Length > 3)
            throw new SvmCompilationException(String.Format(
                InvalidOperandsMessage, instruction));

        IInstruction compiledInstruction = null;
        switch (tokens.Length)
        {
            case 1:
                compiledInstruction =JITCompiler.
                    CompileInstruction(tokens[0]);
                break;
            default:
                var operands = new List<string>();
                for (int i = 1; i < tokens.Length; i++)
                    operands.Add(tokens[i].Trim('\"'));
                compiledInstruction = JITCompiler.
                    CompileInstruction(tokens[0], operands.ToArray());
                break;
        }
        compiledInstruction.VirtualMachine = this;
        compiledInstruction.HasBreakPoint = containsBreakPoint;
        this.program.Add(compiledInstruction);
    }
    #endregion
    #endregion

    #region System.Object overrides
    /// <summary>
    /// Determines whether the specified <see cref="System.Object">Object</see> is equal to the current <see cref="System.Object">Object</see>.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object">Object</see> to compare with the current <see cref="System.Object">Object</see>.</param>
    /// <returns><b>true</b> if the specified <see cref="System.Object">Object</see> is equal to the current <see cref="System.Object">Object</see>; otherwise, <b>false</b>.</returns>
    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    /// <summary>
    /// Serves as a hash function for this type.
    /// </summary>
    /// <returns>A hash code for the current <see cref="System.Object">Object</see>.</returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>
    /// Returns a <see cref="System.String">String</see> that represents the current <see cref="System.Object">Object</see>.
    /// </summary>
    /// <returns>A <see cref="System.String">String</see> that represents the current <see cref="System.Object">Object</see>.</returns>
    public override string ToString()
    {
        return base.ToString();
    }
    #endregion
}