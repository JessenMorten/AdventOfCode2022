namespace AdventOfCode2022.Days;

internal class Day07 : IDay
{
    private readonly Directory _root = new("/");

    public void Setup()
    {
        var lines = System.IO.File.ReadAllLines("Input/07.txt");
        var navigator = new Navigator();

        Directory directory = _root;
        foreach (var command in lines)
        {
            var currentWorkingDirectory = command.StartsWith("$ cd ")
                ? navigator.ChangeDirectory(command[5..])
                : navigator.CurrectWorkingDirectory;

            var existingDir = _root
                .GetSubDirectoriesRecursively()
                .FirstOrDefault(d => d.AbsolutePath == currentWorkingDirectory);

            if (existingDir is null)
            {
                existingDir = new Directory(currentWorkingDirectory);
                directory.TryAddSubDirectory(existingDir);
            }

            directory = existingDir;

            if (directory is null)
            {
                directory = new Directory(currentWorkingDirectory);
                _root.TryAddSubDirectory(directory);
            }
            else
            {
                directory.TryAddSubDirectory(new(currentWorkingDirectory));
            }

            if (command == "dir ")
            {
                var dirName = command[4..];
                directory.TryAddSubDirectory(new Directory($"{currentWorkingDirectory}/{dirName}"));
            }

            var commandParts = command.Split(' ');
            if (commandParts.Length == 2 && int.TryParse(commandParts[0], out var size))
            {
                var fileName = commandParts[1];
                directory.TryAddFile(new File(fileName, size));
            }
        }
    }

    public object SolveA()
    {
        var maxDirectorySize = 100_000;

        return _root
            .GetSubDirectoriesRecursively()
            .Prepend(_root)
            .Where(d => d.GetFilesRecursively().Sum(f => f.Size) < maxDirectorySize)
            .SelectMany(d => d.GetFilesRecursively())
            .Sum(f => f.Size);
    }

    public object SolveB()
    {
        var totalDiskSpace = 70_000_000;
        var spaceRequiredForUpdate = 30_000_000;

        var allDirectories = _root
            .GetSubDirectoriesRecursively()
            .Prepend(_root)
            .Select(directory => (
                Directory: directory,
                Files: directory.GetFilesRecursively(),
                Size: directory.GetFilesRecursively().Sum(f => f.Size)))
            .ToList();

        var spaceUsed = allDirectories
            .SelectMany(d => d.Files)
            .Distinct()
            .Sum(d => d.Size);

        spaceRequiredForUpdate -= (totalDiskSpace - spaceUsed);

        return allDirectories
            .Where(d => d.Size >= spaceRequiredForUpdate)
            .MinBy(d => d.Size)
            .Size;
    }

    private sealed record File(string Name, int Size);

    private sealed record Directory(string AbsolutePath)
    {
        private readonly Dictionary<string, File> _files = new();

        private readonly Dictionary<string, Directory> _subDirectories = new();

        public IEnumerable<File> Files => _files.Values;

        public IEnumerable<Directory> SubDirectories => _subDirectories.Values;

        public void TryAddSubDirectory(Directory directory)
        {
            _subDirectories.TryAdd(directory.AbsolutePath, directory);
        }

        public void TryAddFile(File file)
        {
            _files.TryAdd(file.Name, file);
        }

        public IEnumerable<Directory> GetSubDirectoriesRecursively()
        {
            var result = new List<Directory>();

            foreach (var subDir in SubDirectories)
            {
                result.Add(subDir);
                result.AddRange(subDir.GetSubDirectoriesRecursively());
            }

            return result;
        }

        public IEnumerable<File> GetFilesRecursively()
        {
            var result = new List<File>();

            foreach (var subDir in SubDirectories)
                result.AddRange(subDir.GetFilesRecursively());

            result.AddRange(Files);
            return result;
        }
    }

    private class Navigator
    {
        private readonly Stack<string> _history = new();

        public string CurrectWorkingDirectory => $"/{string.Join('/', _history.Reverse())}";

        public string ChangeDirectory(string directory)
        {
            if (directory == "..")
                _ = _history.Pop();
            else if (directory == "/")
                _history.Clear();
            else
                _history.Push(directory);

            return CurrectWorkingDirectory;
        }
    }
}
