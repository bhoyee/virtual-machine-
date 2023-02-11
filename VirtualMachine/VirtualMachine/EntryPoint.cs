#region Using directives
using System.Runtime.InteropServices;
#endregion

[DllImport("kernel32.dll", SetLastError = true)]
[return: MarshalAs(UnmanagedType.Bool)]
static extern bool AllocConsole();

AllocConsole();

if (CommandLineIsValid(args))
{
    IVirtualMachine vm = new SvmVirtualMachine(args[0]);
    vm.Run();
    Console.ReadLine();
}

#region Validate command line
/// <summary>
/// Verifies that a valid command line has been supplied
/// by the user
/// </summary>
static bool CommandLineIsValid(string[] args)
{
    bool valid = true;

    if (args.Length != 1)
    {
        DisplayUsageMessage("Wrong number of command line arguments");
        valid = false;
    }

    if (valid && !args[0].EndsWith(".sml", StringComparison.CurrentCultureIgnoreCase))
    {
        DisplayUsageMessage("SML programs must be in a file named with a .sml extension");
        valid = false;
    }

    return valid;
}

/// <summary>
/// Displays comamnd line usage information for the
/// SVM virtual machine 
/// </summary>
/// <param name="message">A custom message to display
/// to the user</param>
static void DisplayUsageMessage(string message)
{
    Console.WriteLine("The command line arguments are not valid. {0} \r\n", message);
    Console.WriteLine("USAGE:");
    Console.WriteLine("svm program_name.sml");
}
#endregion

