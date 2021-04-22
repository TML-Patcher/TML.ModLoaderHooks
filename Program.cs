using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using MonoMod;
using MonoMod.RuntimeDetour.HookGen;

static void WriteAndExit(string toWrite, ConsoleColor color = ConsoleColor.Red)
{
    Console.ForegroundColor = color;
    Console.WriteLine(toWrite);

    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("Press any key to exit...");

    Console.ReadKey();
}

Console.ForegroundColor = ConsoleColor.Gray;
Console.WriteLine($" Running TML.ModLoaderHooks [{Assembly.GetAssembly(Type.GetType("<Program>$")!)?.GetName().Version}]");

try
{
    if (args.Length == 0)
    {
        WriteAndExit("Error: no valid assembly file path was specified");
        return;
    }

    Directory.CreateDirectory("Generated");
    string outputPath = Path.Combine("Generated", "ModLoaderHooks.dll");

    if (!File.Exists(args[0]))
    {
        WriteAndExit($"Error: could not find file: {args[0]}");
        return;
    }

    if (File.Exists(outputPath))
    {
        Console.WriteLine($" Deleting {outputPath}...");
        File.Delete(outputPath);
    }

    Console.WriteLine();

    using MonoModder mModder = new()
    {
        InputPath = args[0],
        OutputPath = outputPath,
        ReadingMode = ReadingMode.Deferred,

        MissingDependencyThrow = false
    };

    mModder.Read();
    mModder.MapDependencies();

    HookGenerator hookGenerator = new(mModder, "ModLoaderHooks") { HookPrivate = true };

    using (ModuleDefinition module = hookGenerator.OutputModule)
    {
        hookGenerator.Generate();

        for (int i = hookGenerator.OutputModule.Types.Count - 1; i >= 0; i--)
            if (!hookGenerator.OutputModule.Types[i].FullName.Contains("Terraria.ModLoader"))
                hookGenerator.OutputModule.Types.RemoveAt(i);

        module.Write(outputPath);
    }

    Console.WriteLine();
    WriteAndExit($" Generated out-put DLL at {outputPath}!", ConsoleColor.Green);
}
catch (Exception e)
{
    Console.ForegroundColor = ConsoleColor.Red;
    WriteAndExit($"Error: encountered exception: {e}");
}