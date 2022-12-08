namespace AdventOfCode2022.Days;

internal class Day04 : IDay
{
    private List<(int[] Left, int[] Right)> _pairs = new();

    public void Setup()
    {
        File
            .ReadAllLines("Input/04.txt")
            .ToList()
            .ForEach(line =>
            {
                var elves = line.Split(",");

                var leftStart = int.Parse(elves.First().Split("-").First());
                var leftEnd = int.Parse(elves.First().Split("-").Last());

                var rightStart = int.Parse(elves.Last().Split("-").First());
                var rightEnd = int.Parse(elves.Last().Split("-").Last());

                _pairs.Add((
                    Left: Enumerable.Range(leftStart, leftEnd - leftStart + 1).ToArray(),
                    Right: Enumerable.Range(rightStart, rightEnd - rightStart + 1).ToArray()
                ));
            });
    }

    public object SolveA()
    {
        return _pairs.Count(FullOverlap);
    }

    public object SolveB()
    {
        return _pairs.Count(pair => pair.Left.Intersect(pair.Right).Any());
    }

    private static bool FullOverlap((int[] Left, int[] Right) pair)
    {
        return
            (pair.Left.Min() <= pair.Right.Min() && pair.Left.Max() >= pair.Right.Max())
            || (pair.Right.Min() <= pair.Left.Min() && pair.Right.Max() >= pair.Left.Max());
    }
}
