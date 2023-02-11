namespace SML.Tests;

[TestClass]
public class DecrUnitTest
{
    private MockVirtualMachine _virtualmachine = new MockVirtualMachine();
    private List<IInstruction> _instructions = new List<IInstruction>();

    [TestCleanup]
    public void TestCleanup()
    {
        this._instructions.Clear();
    }

    [TestMethod]
    public void TestMethod1()
    {
        this._instructions.Add(new LoadInt() { Operands = new string[] { "1" } });
        this._instructions.Add(new Decr());
        this._instructions.Add(new Decr());
        this._instructions.Add(new Decr());
        this._instructions.Add(new Decr());
        this._virtualmachine.Load(this._instructions).Run();
        Assert.AreEqual(1, this._virtualmachine.Stack.Count);
        var value = this._virtualmachine.Stack.Pop();
        if (value == null)
            Assert.Fail();
        Assert.AreEqual(1-4, (int)value);
    }

    [TestMethod]
    [ExpectedException(typeof(SvmRuntimeException))]
    public void TestMethod2()
    {
        this._instructions.Add(new Decr());
        this._virtualmachine.Load(this._instructions).Run();
    }

    [TestMethod]
    [ExpectedException(typeof(SvmRuntimeException))]
    public void TestMethod3()
    {
        this._instructions.Add(new LoadInt() { Operands = new string[] { "non-int value" } });
        this._instructions.Add(new Decr());
        this._virtualmachine.Load(this._instructions).Run();
    }
}