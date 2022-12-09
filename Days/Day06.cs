namespace AdventOfCode2022.Days;

internal class Day06 : IDay
{
    private readonly List<char> _buffer = new();

    public void Setup() => _buffer.AddRange(File.ReadAllText("Input/06.txt").ToArray());

    public object SolveA() => GetPacketStart(_buffer);

    public object SolveB() => GetMessageStart(_buffer);

    private static int GetPacketStart(IEnumerable<char> buffer) => GetStart(buffer, 4);

    private static int GetMessageStart(IEnumerable<char> buffer) => GetStart(buffer, 14);

    private static int GetStart(IEnumerable<char> buffer, int disinct)
    {
        var start = 0;
        while (buffer.Skip(start).Take(disinct).Distinct().Count() != disinct)
            start++;
        return start + disinct;
    }
}
