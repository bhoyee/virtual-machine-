#region Using directives
using System.Reflection;
using System.Configuration;
using System.Security.Cryptography;
using System.Security;
#endregion

namespace SVM.Loader;

/// <summary>
/// This Class responsible for loading assemblies and types needed in this project.
/// </summary>
public static class AssemblyLoader
{
    /// <summary>
    /// These both fields are needed for MetadataLoadContext creation.
    /// </summary>
    private static readonly string _mscorlibName = "System.Private.CoreLib";
    private static readonly string _mscorlibPath = typeof(object).Assembly.Location;

    /// <summary>
    /// Dictionary of types that are registered in AssemblyLoader
    /// by the moment of accessing this field.
    /// </summary>
    private static Dictionary<Type, bool> _typeRepository;

    /// <summary>
    /// Dictionary of assembly names, needed for determining if the assembly
    /// was loaded or not without actual loading the target assembly.
    /// </summary>
    private static Dictionary<string, bool> _assemblyNameRepository;

    /// <summary>
    /// Dictionary of assemblies that are registered in AssemblyLoader
    /// by the moment of accessing this field.
    /// </summary>
    private static Dictionary<DirectoryInfo, List<Assembly>> _assemblyRepository;

    /// <summary>
    /// Determines if the type specified is valid for appending to _typeRepository.
    /// </summary>
    /// <param name="type">Type that we want to check.</param>
    /// <returns>trur if file is useful, otherwise false.</returns>
    private static bool IsUsefulType(Type type)
    {
        // No practical need for Abstract of Interface kind of type.
        if (type.IsAbstract || type.IsInterface)
            return false;
        // Only types implementing these three interfaces are needed.
        //  IInstruction - for loading instructions.
        //  IDebugger    - for loading debuggers.
        //  IDebugFrame  - for loading debugger frame which IDebugger.Break() uses.
        if (type.Implements(typeof(IInstruction)) ||
            type.Implements(typeof(IDebugger)) ||
            type.Implements(typeof(IDebugFrame)))
            return true;
        return false;
    }

    /// <summary>
    /// Determines if .NET assembly located at fileInfo.FullName.
    /// </summary>
    /// <param name="fileInfo">File information, which we want to check.</param>
    /// <param name="assembly">Created assembly metadata using MetadataLoadContext, this is not the assembly for use.</param>
    /// <returns>true if .NET assembly is located there, otherwise false.</returns>
    private static bool IsUsefulAssembly(FileInfo fileInfo, out Assembly assembly)
    {
        assembly = null;
        try
        {
            var paths = new string[] { fileInfo.FullName, _mscorlibPath };
            var metadata = new MetadataLoadContext(
                new PathAssemblyResolver(paths), _mscorlibName);
            var asm = metadata.LoadFromAssemblyPath(fileInfo.FullName);
            assembly = asm;
            return asm.GetExportedTypes().Length != 0;
        }
        catch (FileLoadException) { }
        catch (FileNotFoundException) { }
        catch (BadImageFormatException) { }
        return false;
    }

    /// <summary>
    /// Determines if the assembly (using only it's metadata information) was already loaded.
    /// </summary>
    /// <param name="assembly">Assembly metadata, which we want to check.</param>
    /// <returns>true if assembly is already loaded, otherwise false.</returns>
    private static bool IsAssemblyLoaded(Assembly assembly)
    {
        if (_assemblyNameRepository == null)
            _assemblyNameRepository = new Dictionary<string, bool>();
        var asmName = assembly.GetName();
        var asmIsLoaded = false;
        if (!(asmIsLoaded = _assemblyNameRepository.ContainsKey(asmName.FullName)))
            _assemblyNameRepository[asmName.FullName] = true;
        return asmIsLoaded;
    }

    /// <summary>
    /// Loads assembly from it's metadata object for actual use.
    /// </summary>
    /// <param name="assembly">Assembly metadata object.</param>
    /// <returns>Loaded assembly for use, otherwise null.</returns>
    public static Assembly LoadAssemblyForUse(Assembly assembly)
    {
        try
        {
            return Assembly.LoadFile(assembly.Location);
        }
        catch (SecurityException) { }
        catch (FileLoadException) { }
        catch (BadImageFormatException) { }
        return null;
    }

    /// <summary>
    /// Validates the hash of assembly, with specified hashes in App.config.
    /// </summary>
    /// <param name="assembly">Assembly metadata information.</param>
    /// <returns>true if hash of specified assembly is valid, otherwise false.</returns>
    private static bool ValidateAssemblyHash(Assembly assembly)
    {
        string asmLocation = assembly.Location; 
        string asmManifestName = assembly.ManifestModule.Name;
        string hash = ConfigurationManager.AppSettings.Get(asmManifestName);
        if (hash != null)
            using (var sha256 = SHA256.Create())
                using (var stream = File.OpenRead(asmLocation))
                    return hash == BitConverter.ToString(sha256.ComputeHash(stream))
                        .Replace("-", "").ToLowerInvariant();
        return false;
    }

    /// <summary>
    /// Validates if assembly has signature or not.
    /// </summary>
    /// <param name="assembly">Instance of loaded assembly metadata.</param>
    /// <returns>true if specified assembly has signature, otherwise false.</returns>
    private static bool ValidateAssemblySignature(Assembly assembly)
    {
        try
        {
            return assembly.GetName().GetPublicKey().Length > 0;
        }
        catch (SecurityException) { }
        return false;
    }

    /// <summary>
    /// Updates (or initializes) types in _typeRepository.
    /// </summary>
    /// <param name="info">Directory which this assembly is belongs.</param>
    /// <param name="assembly">Assembly object from which types will be loaded.</param>
    public static void UpdateTypeRepository(DirectoryInfo info, Assembly assembly)
    {
        if (_typeRepository == null)
            _typeRepository = new Dictionary<Type, bool>();
        try
        {
            var isAsmUseful = false;
            foreach (var type in assembly.GetExportedTypes())
                if (!type.IsAbstract && !type.IsInterface)
                    if (IsUsefulType(type) && !_typeRepository.ContainsKey(type))
                        _typeRepository[type] = (isAsmUseful = true);
            // if no types are loaded from the assembly,
            // it means that it is not useful, we need to remove it.
            if (!isAsmUseful && info != null)
                _assemblyRepository[info].Remove(assembly);
            if (!isAsmUseful)
                _assemblyNameRepository.Remove(assembly.GetName().FullName);
        }
        catch (NotSupportedException) { }
        catch (FileNotFoundException) { }
    }

    /// <summary>
    /// Updates (or initializes) assemblies in _asmRepository.
    /// This method finds all .NET asseblies located in directory specified.
    /// </summary>
    /// <param name="root">Directory information from which this method will load all assemblies.</param>
    public static void UpdateAssemblyRepository(DirectoryInfo root)
    {
        if (_assemblyRepository == null)
        {
            _assemblyRepository = new Dictionary<DirectoryInfo, List<Assembly>>();
            UpdateTypeRepository(null, Assembly.GetExecutingAssembly());
        }
        Assembly loadedAssembly;
        var assemblies = new List<Assembly>();
        foreach (var fileInfo in root.EnumerateFiles())
            if (fileInfo.Exists && IsUsefulAssembly(fileInfo, out loadedAssembly))
                if (!IsAssemblyLoaded(loadedAssembly))
                    if (ValidateAssemblyHash(loadedAssembly))
                        if (ValidateAssemblySignature(loadedAssembly))
                            assemblies.Add(LoadAssemblyForUse(loadedAssembly));
        // clear the assemblies list from the possible null values
        // from LoadAssemblyForUse method.
        assemblies = assemblies.Where(a => a != null).ToList();
        if (!_assemblyRepository.ContainsKey(root))
            _assemblyRepository[root] = new List<Assembly>();
        foreach (var assembly in assemblies)
            _assemblyRepository[root].Add(assembly);
        // when we loaded all new assemblies, it is time to update the _typeRepository
        foreach (var assembly in assemblies)
            UpdateTypeRepository(root, assembly);
    }

    /// <summary>
    /// Tryes to get type from _typeRepository by its name (not case-sensitive).
    /// </summary>
    /// <param name="typeName">Name of type that we searching for.</param>
    /// <param name="type">Type object in here, if method found any.</param>
    /// <returns>true if type is found, otherwise false.</returns>
    public static bool TryToGetType(string typeName, out Type type) =>
        TryToGetType((t) => (t.Name.ToLower() == typeName.ToLower()), out type);

    /// <summary>
    /// Tryes to get type from _typeRepository by some boolean function.
    /// </summary>
    /// <param name="typeName">Name of type that we searching for.</param>
    /// <param name="type">Type object in here, if method found any.</param>
    /// <returns>true if type is found, otherwise false.</returns>
    public static bool TryToGetType(Func<Type, bool> action, out Type type)
    {
        if (_assemblyRepository == null)
            UpdateAssemblyRepository(new DirectoryInfo(
                Environment.CurrentDirectory));
        type = _typeRepository.Keys.FirstOrDefault(t => action(t));
        return type != null;
    }

    /// <summary>
    /// Finds any IDebugger type implementation, and instanciates it.
    /// </summary>
    /// <returns>Instance of first-found IDebugger type, otherwise null.</returns>
    public static IDebugger CreateDebugger()
    {
        Type type = null;
        if (TryToGetType((t) => t.Implements(typeof(IDebugger)), out type))
        {
            var instance = Instanciate(type);
            if (instance != null)
                return instance as IDebugger;
        }
        return null;
    }

    /// <summary>
    /// Finds any IDebugFrame type implementation, and instanciates it.
    /// </summary>
    /// <param name="args">Arguments with which new instance will be created.</param>
    /// <returns>Instance of first-found IDebugFrame type, otherwise null.</returns>
    public static IDebugFrame CreateDebugFrame(object[] args)
    {
        Type type = null;
        if (TryToGetType((t) => t.Implements(typeof(IDebugFrame)), out type))
        {
            var instance = Instanciate(type, args);
            if (instance != null)
                return instance as IDebugFrame;
        }
        return null;
    }

    /// <summary>
    /// Finds any IInstruction type implementation, and instanciates it.
    /// </summary>
    /// <param name="args">Arguments with which new instance will be created.</param>
    /// <returns>Instance of first-found IDebugFrame type, otherwise null.</returns>
    public static IInstruction CreateInstruction(string opCode, string[] operands = null)
    {
        Type type = null;
        if (TryToGetType(opCode, out type))
        {
            var instance = Instanciate(type);
            if (instance == null)
                throw new SvmCompilationException($"Cannot instanciate \'{opCode}\' instruction.");
            if (type.Implements(typeof(IInstructionWithOperand)))
                (instance as IInstructionWithOperand).Operands = operands;
            return instance as IInstruction;
        }
        throw new SvmCompilationException($"Cannot get type of \'{opCode}\' instruction.");
    }

    /// <summary>
    /// Instanciates type, with some arguments.
    /// </summary>
    /// <param name="type">Type, which we want instanciate.</param>
    /// <param name="args">arguments with which instance will be created.
    /// Can be null, in this case, empty object[] array will be created.</param>
    /// <returns>Object of type name="type", otherwise false.</returns>
    public static object Instanciate(Type type, object[] args = null) =>
        Activator.CreateInstance(type, args);

    /// <summary>
    /// Extension of Type class, determines if type implements some other type.
    /// </summary>
    /// <param name="type">Type that will be checked for implementation of some interface.</param>
    /// <param name="interfaceType">Interface type.</param>
    /// <returns>trur if name="type" implements name="interfaceType", otherwise false.</returns>
    public static bool Implements(this Type type, Type interfaceType)
    {
        foreach (var typeInterface in type.GetInterfaces())
            if (typeInterface == interfaceType)
                return true;
        return false;
    }
}
