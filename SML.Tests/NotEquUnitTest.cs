namespace SML.Tests;

[TestClass]
public class NotEquUnitTest
{
    private MockVirtualMachine _vm = new MockVirtualMachine();
    private List<IInstruction> _instructions = new List<IInstruction>();

    [TestCleanup]
    public void TestCleanup()
    {
        this._vm.Stack.Clear();
        this._vm.LabelEntries.Clear();
        this._instructions.Clear();
    }

    [TestMethod]
    public void TestMethod1()
    {
        this._vm.LabelEntries["trueLabel"] = 5;
        this._instructions.Add(new LoadInt()    { Operands = new string[] { "10" } });
        this._instructions.Add(new LoadInt()    { Operands = new string[] { "5" } });
        this._instructions.Add(new NotEqu()     { Operands = new string[] { "trueLabel" } });
        this._instructions.Add(new LoadString() { Operands = new string[] { "false" } });
        this._instructions.Add(new WriteString());
        this._instructions.Add(new LoadString() { Operands = new string[] { "true" } });
        this._instructions.Add(new WriteString());
        this._vm.Load(this._instructions).Run();
        Assert.AreEqual(2, this._vm.Stack.Count);
    }

    [TestMethod]
    public void TestMethod2()
    {
        this._vm.LabelEntries["trueLabel"] = 5;
        this._instructions.Add(new LoadString() { Operands = new string[] { "asd" } });
        this._instructions.Add(new LoadString() { Operands = new string[] { "asd" } });
        this._instructions.Add(new NotEqu() { Operands = new string[] { "trueLabel" } });
        this._instructions.Add(new LoadString() { Operands = new string[] { "false" } });
        this._instructions.Add(new LoadString() { Operands = new string[] { "true" } });
        this._instructions.Add(new WriteString());
        this._vm.Load(this._instructions).Run();
        Assert.AreEqual(3, this._vm.Stack.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(SvmRuntimeException))]
    public void TestMethod3()
    {
        this._instructions.Add(new NotEqu() { Operands = new string[] { "label" } });
        this._vm.Load(this._instructions).Run();
    }

    [TestMethod]
    [ExpectedException(typeof(SvmRuntimeException))]
    public void TestMethod4()
    {
        this._instructions.Add(new LoadString() { Operands = new string[] { "asd" } });
        this._instructions.Add(new NotEqu() { Operands = new string[] { "label" } });
        this._vm.Load(this._instructions).Run();
    }

    [TestMethod]
    [ExpectedException(typeof(SvmRuntimeException))]
    public void TestMethod5()
    {
        this._instructions.Add(new LoadString() { Operands = new string[] { "asd" } });
        this._instructions.Add(new LoadString() { Operands = new string[] { "asd" } });
        this._instructions.Add(new NotEqu() { Operands = new string[] { "label" } });
        this._vm.Load(this._instructions).Run();
    }
}