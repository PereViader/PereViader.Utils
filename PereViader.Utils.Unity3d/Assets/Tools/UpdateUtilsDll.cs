using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class UpdateUtilsDll
{
    private const string SolutionRelativePath = "../../PereViader.Utils.Common/PereViader.Utils.Common.sln";
    private const string CommonDllPath = "../../PereViader.Utils.Common/PereViader.Utils.Common/bin/Release/netstandard2.1/PereViader.Utils.Common.dll";
    private const string GeneratorsDllPath = "../../PereViader.Utils.Common/PereViader.Utils.Common.Generators/bin/Release/netstandard2.0/PereViader.Utils.Common.Generators.dll";
    private const string CommonOutputDllPath = "Plugins/PereViader.Utils.Common/PereViader.Utils.Common.dll";
    private const string GeneratorsOutputDllPath = "Plugins/PereViader.Utils.Common/PereViader.Utils.Common.Generators.dll";

    [MenuItem("Tools/Sync PereViader.Utils.Common")]
    public static void BuildAndImportDll()
    {
        var solutionPath = Path.GetFullPath(Path.Combine(Application.dataPath, SolutionRelativePath));

        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
            FileName = "dotnet",
            Arguments = $"build \"{solutionPath}\" -c Release",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        process!.WaitForExit();

        var output = process.StandardOutput.ReadToEnd();
        
        if (process.ExitCode != 0)
        {
            UnityEngine.Debug.LogError("Build failed\n" +output);
            return;
        }

        UnityEngine.Debug.Log("Build succeded\n "+ output);


        // Copy the built DLLs to the Unity project
        var commonSourceDllPath = Path.GetFullPath(Path.Combine(Application.dataPath, CommonDllPath));
        var commonDestinationDllPath = Path.GetFullPath(Path.Combine(Application.dataPath, CommonOutputDllPath));
        File.Copy(commonSourceDllPath, commonDestinationDllPath, true);
        
        var generatorsSourceDllPath = Path.GetFullPath(Path.Combine(Application.dataPath, GeneratorsDllPath));
        var generatorsDestinationDllPath = Path.GetFullPath(Path.Combine(Application.dataPath, GeneratorsOutputDllPath));
        File.Copy(generatorsSourceDllPath, generatorsDestinationDllPath, true);
        
        AssetDatabase.Refresh();

        var generatorAssetImporter = (PluginImporter)AssetImporter.GetAtPath(Path.Combine("Assets", GeneratorsOutputDllPath));
        AssetDatabase.SetLabels(generatorAssetImporter, new string[] { "RoslynAnalyzer" });
        generatorAssetImporter.SetCompatibleWithAnyPlatform(false);
        generatorAssetImporter.SaveAndReimport();
        AssetDatabase.Refresh();
    }
}
