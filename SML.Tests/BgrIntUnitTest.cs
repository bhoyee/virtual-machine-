namespace SML.Tests;

[TestClass]
public class BltIntUnitTest
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
        this._vm.LabelEntries["trueLabel"] = 3;
        this._instructions.Add(new LoadInt() { Operands = new string[] { "10" } });
        this._instructions.Add(new BltInt() { Operands = new string[] { "9", "trueLabel" } });
        this._instructions.Add(new WriteString());
        this._instructions.Add(new LoadString() { Operands = new string[] { "true" } });
        this._instructions.Add(new WriteString());
        this._vm.Load(this._instructions).Run();
        Assert.AreEqual(1, this._vm.Stack.Count);
    }

    [TestMethod]
    public void TestMethod2()
    {
        this._vm.LabelEntries["trueLabel"] = 3;
        this._instructions.Add(new LoadInt() { Operands = new string[] { "10" } });
        this._instructions.Add(new BltInt() { Operands = new string[] { "10", "trueLabel" } });
        this._instructions.Add(new WriteString());
        this._instructions.Add(new LoadString() { Operands = new string[] { "true" } });
        this._instructions.Add(new WriteString());
        this._vm.Load(this._instructions).Run();
        Assert.AreEqual(0, this._vm.Stack.Count);
    }

    [TestMethod]
    public void TestMethod3()
    {
        this._vm.LabelEntries["trueLabel"] = 3;
        this._instructions.Add(new LoadInt() { Operands = new string[] { "10" } });
        this._instructions.Add(new BltInt() { Operands = new string[] { "11", "trueLabel" } });
        this._instructions.Add(new WriteString());
        this._instructions.Add(new LoadString() { Operands = new string[] { "true" } });
        this._instructions.Add(new WriteString());
        this._vm.Load(this._instructions).Run();
        Assert.AreEqual(0, this._vm.Stack.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(SvmRuntimeException))]
    public void TestMethod4()
    {
        this._instructions.Add(new BltInt() { Operands = new string[] { "5", "label" } });
        this._vm.Load(this._instructions).Run();
    }

    [TestMethod]
    [ExpectedException(typeof(SvmRuntimeException))]
    public void TestMethod5()
    {
        this._instructions.Add(new LoadInt() { Operands = new string[] { "4" } });
        this._instructions.Add(new BltInt() { Operands = new string[] { "5", "label" } });
        this._vm.Load(this._instructions).Run();
    }
}

[TestClass]
public class BgrIntUnitTest
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
        this._vm.LabelEntries["trueLabel"] = 3;
        this._instructions.Add(new LoadInt() { Operands = new string[] { "10" } });
        this._instructions.Add(new BgrInt() { Operands = new string[] { "11", "trueLabel" } });
        this._instructions.Add(new WriteString());
        this._instructions.Add(new LoadString() { Operands = new string[] { "true" } });
        this._instructions.Add(new WriteString());
        this._vm.Load(this._instructions).Run();
        Assert.AreEqual(1, this._vm.Stack.Count);
    }

    [TestMethod]
    public void TestMethod2()
    {
        this._vm.LabelEntries["trueLabel"] = 3;
        this._instructions.Add(new LoadInt() { Operands = new string[] { "10" } });
        this._instructions.Add(new BgrInt() { Operands = new string[] { "10", "trueLabel" } });
        this._instructions.Add(new WriteString());
        this._instructions.Add(new LoadString() { Operands = new string[] { "true" } });
        this._instructions.Add(new WriteString());
        this._vm.Load(this._instructions).Run();
        Assert.AreEqual(0, this._vm.Stack.Count);
    }

    [TestMethod]
    public void TestMethod3()
    {
        this._vm.LabelEntries["trueLabel"] = 3;
        this._instructions.Add(new LoadInt() { Operands = new string[] { "10" } });
        this._instructions.Add(new BgrInt() { Operands = new string[] { "9", "trueLabel" } });
        this._instructions.Add(new WriteString());
        this._instructions.Add(new LoadString() { Operands = new string[] { "true" } });
        this._instructions.Add(new WriteString());
        this._vm.Load(this._instructions).Run();
        Assert.AreEqual(0, this._vm.Stack.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(SvmRuntimeException))]
    public void TestMethod4()
    {
        this._instructions.Add(new BgrInt() { Operands = new string[] { "5", "label" } });
        this._vm.Load(this._instructions).Run();
    }

    [TestMethod]
    [ExpectedException(typeof(SvmRuntimeException))]
    public void TestMethod5()
    {
        this._instructions.Add(new LoadInt() { Operands = new string[] { "4" } });
        this._instructions.Add(new BgrInt() { Operands = new string[] { "5", "label" } });
        this._vm.Load(this._instructions).Run();
    }
}