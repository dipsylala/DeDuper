using System.Security.Cryptography;

try
{
    CheckArguments(Environment.GetCommandLineArgs());
}
catch (ArgumentException ex)
{
    System.Console.WriteLine(ex.Message);
    return 1;
}

var inputDir = new DirectoryInfo(Environment.GetCommandLineArgs()[1]);
var outputDir = new DirectoryInfo(Environment.GetCommandLineArgs()[2]);

var recurseFileStructure = new RecurseDirectoryStructure();
recurseFileStructure.TraverseDirectory(inputDir, outputDir);

return 0;


static void CheckArguments(string[] args)
{
    if (args.Length != 3)
    {
        throw new ArgumentException("Arguments: <input directory> <output directory>");
    }

    if (Directory.Exists(Environment.GetCommandLineArgs()[1]) == false)
    {
        throw new ArgumentException($"Output directory {Environment.GetCommandLineArgs()[1]} does not exist");
    }

    if (Directory.Exists(Environment.GetCommandLineArgs()[2]) == false)
    {
        throw new ArgumentException($"Output directory {Environment.GetCommandLineArgs()[2]} does not exist");
    }
}

public class RecurseDirectoryStructure
{
    public void TraverseDirectory(DirectoryInfo inputDir, DirectoryInfo outputDir )
    {
        var subDirectories = inputDir.EnumerateDirectories();

        foreach (var subDirectory in subDirectories)
        {
            TraverseDirectory(subDirectory, outputDir);
        }

        var files = inputDir.EnumerateFiles();

        foreach (var file in files)
        {
            HandleFile(file, outputDir);
        }
    }

    private static void HandleFile(FileSystemInfo file, FileSystemInfo outputDir)
    {
        using var md5 = MD5.Create();
        var name = file.Name;

        using var stream = File.OpenRead(file.FullName);
        var hash = md5.ComputeHash(stream);
        var hashString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

        // check for directory in output called <name>_<checksum>
        var path = Path.Combine(outputDir.FullName, hashString[..2], name + " - " + hashString);
        if (Directory.Exists(path))
        {
            return;
        }

        Directory.CreateDirectory(path);
        File.Copy(file.FullName, Path.Combine(path, file.Name));
    }
}