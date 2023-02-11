namespace SML.Tests;

[TestClass]
public class IncrUnitTest
{
    private MockVirtualMachine _vm = new MockVirtualMachine();
    private List<IInstruction> _instructions = new List<IInstruction>();

    [TestCleanup]
    public void TestCleanup()
    {
        this._instructions.Clear();
    }

    [TestMethod]
    public void TestMethod1()
    {
        this._instructions.Add(new LoadInt() { Operands = new string[] { "10" } });
        this._instructions.Add(new Incr());
        this._instructions.Add(new Incr());
        this._vm.Load(this._instructions).Run();
        Assert.AreEqual(1, this._vm.Stack.Count);
        var value = this._vm.Stack.Pop();
        if (value == null)
            Assert.Fail();
        Assert.AreEqual(10 + 2, (int)value);
    }

    [TestMethod]
    [ExpectedException(typeof(SvmRuntimeException))]
    public void TestMethod2()
    {
        this._instructions.Add(new Incr());
        this._vm.Load(this._instructions).Run();
    }

    [TestMethod]
    [ExpectedException(typeof(SvmRuntimeException))]
    public void TestMethod3()
    {
        this._instructions.Add(new LoadInt() { Operands = new string[] { "non-int value" } });
        this._instructions.Add(new Incr());
        this._vm.Load(this._instructions).Run();
    }
}
