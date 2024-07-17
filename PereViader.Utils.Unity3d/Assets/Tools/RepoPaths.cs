using System.IO;
using UnityEngine;

public static class RepoPaths
{
    public static string RepositoryRootPath => Path.Combine(Application.dataPath, "../..");
}