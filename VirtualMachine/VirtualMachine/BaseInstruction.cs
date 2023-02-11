namespace SVM.VirtualMachine;

/// <summary>
/// TODO: Describe the purpose of this class here
/// </summary>
public abstract class BaseInstruction: IInstruction
{
    #region Constants
    protected const string StackIsEmptyMessage = "Stack is empty.";
    protected const string StackUnderflowMessage = "A stack underflow error has occurred. ( at [line {0}] {1})";
    protected const string OperandOfWrongTypeMessage = "The operand on the stack is of the wrong type. (at [line {0}] {1} )";
    protected const string VirtualMachineErrorMessage = "A virtual machine error has occurred.";
    protected const string LabelIsNotDeclaredMessage = "Cannot find label with name {0} (at [line {1}]).";
    #endregion

    #region Fields
    private IVirtualMachine virtualMachine = null;
    #endregion

    #region Constructors
    #endregion

    #region Properties
    /// <summary>
    /// Contains the information about the breakpoint 
    /// at this particular instruction.
    /// </summary>
    public bool HasBreakPoint { get; set; }
    #endregion

    #region Public methods
    #endregion

    #region Non-public methods
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
        return this.GetType().Name;
    }
    #endregion

    #region IInstruction Members
    public abstract void Run();

    public IVirtualMachine VirtualMachine
    {
        get
        {
            return virtualMachine;
        }
        set
        {
            if (null == value)
            {
                throw new SvmRuntimeException(VirtualMachineErrorMessage);
            }
            virtualMachine = value;
        }
    }
    #endregion
}