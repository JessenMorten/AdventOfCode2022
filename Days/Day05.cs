namespace AdventOfCode2022.Days;

internal class Day05 : IDay
{
    private List<Stack<char>> _stacks = new();

    private List<(int MoveCount, int Source, int Destination)> _operations = new();

    public void Setup()
    {
        var allLines = File.ReadAllLines("Input/05.txt").ToList();

        allLines
            .Where(line => line.Contains("["))
            .Reverse()
            .ToList()
            .ForEach(line =>
            {
                var stacks = line.Chunk(4).ToArray();

                while (_stacks.Count < stacks.Length)
                    _stacks.Add(new());

                for (var i = 0; i < stacks.Length; i++)
                    if (stacks[i].Contains('['))
                        _stacks.ElementAt(i).Push(stacks[i][1]);
            });

        allLines
            .Where(line => line.StartsWith("move"))
            .ToList()
            .ForEach(line =>
            {
                var operationData = line
                    .Replace("move ", string.Empty)
                    .Replace(" from ", ",")
                    .Replace(" to ", ",")
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();

                _operations.Add((operationData[0], operationData[1] - 1, operationData[2] - 1));
            });
    }

    public object SolveA()
    {
        var stacks = _stacks.Select(s => new Stack<char>(s.Reverse())).ToList();

        foreach (var operation in _operations)
        {
            for (var i = 0; i < operation.MoveCount; i++)
            {
                var value = stacks[operation.Source].Pop();
                stacks[operation.Destination].Push(value);
            }
        }

        return string.Join(string.Empty, stacks.Select(stack => stack.Peek()));
    }

    public object SolveB()
    {
        foreach (var operation in _operations)
        {
            var items = Enumerable
                .Range(0, operation.MoveCount)
                .Select(_ => _stacks[operation.Source].Pop())
                .Reverse();

            foreach (var aaa in items)
            {
                _stacks[operation.Destination].Push(aaa);
            }
        }

        return string.Join(string.Empty, _stacks.Select(stack => stack.Peek()));
    }
}
