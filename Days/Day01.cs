namespace AdventOfCode2022.Days;

internal class Day01 : IDay
{
    private readonly List<List<int>> _elves = new();

    public void Setup()
    {
        File
            .ReadAllLines("Input/01.txt")
            .Select<string, int?>(line => int.TryParse(line, out var food) ? food : null)
            .ToList()
            .ForEach(food =>
            {
                if (food is null || !_elves.Any())
                    _elves.Add(new());

                if (food.HasValue)
                    _elves.Last().Add(food.Value);
            });
    }

    public object SolveA()
    {
        return _elves
            .Select(elf => elf.Sum())
            .Max();
    }

    public object SolveB()
    {
        return _elves
             .Select(elf => elf.Sum())
             .OrderByDescending(kcal => kcal)
             .Take(3)
             .Sum();
    }
}
