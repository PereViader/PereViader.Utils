using System;
using System.Diagnostics;
using System.IO;
using PereViader.Utils.Common.Results;
using UnityEditor;
using UnityEngine;

public static class UpdateUtilsDll
{
    private static string RepositoryRootPath => Path.Combine(Application.dataPath, "../..");
    private static string UpdateDllScript => Path.Combine(Application.dataPath, "Tools/UpdateUtilsDll.sh");
    
    [MenuItem("Tools/Sync PereViader.Utils.Common")]
    public static void BuildAndImportDll()
    {
        var processStartInfoResult = GetProcessStartInfo()
            .GetResultOrThrow(x => new InvalidOperationException(x));

        using var process = Process.Start(processStartInfoResult);
        if (process is null)
        {
            throw new InvalidOperationException("For some reson, could not start process to update dll");
        }
        
        process.WaitForExit();

        AssetDatabase.Refresh();

        var output = process.StandardOutput.ReadToEnd();
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException("Build failed\n" + output);
        }
        UnityEngine.Debug.Log("Build succeded\n " + output);
    }
    
    static Result<ProcessStartInfo, string> GetProcessStartInfo()
    {
#if UNITY_EDITOR_WIN
        if (!File.Exists("C:/Program Files/Git/git-bash.exe"))
        {
            return Result<ProcessStartInfo, string>.Failure("Could not find git bash at C:/Program Files/Git/git-bash.exe");
        }
        
        return Result<ProcessStartInfo, string>.Success(new ProcessStartInfo
        {
            FileName = "C:/Program Files/Git/git-bash.exe",
            Arguments = UpdateDllScript,
            WorkingDirectory = RepositoryRootPath,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        });
#else
        return Result<ProcessStartInfo, string>.Failure("Current platform has not yet been implemented");
#endif
    }
}