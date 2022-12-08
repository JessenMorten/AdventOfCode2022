namespace AdventOfCode2022.Days;

internal class Day03 : IDay
{
    private List<Rucksack> _rucksacks = new();

    public void Setup()
    {
        File
            .ReadAllLines("Input/03.txt")
            .Select(line => new Rucksack(line.Select(c => new RucksackItem(c))))
            .ToList()
            .ForEach(_rucksacks.Add);
    }

    public object SolveA()
    {
        return _rucksacks
            .Select(rucksack => FindSharedRucksackItem(rucksack.LeftCompartment, rucksack.RightCompartment))
            .Sum(rucksackItem => rucksackItem.Priority);
    }

    public object SolveB()
    {
        return _rucksacks
            .Chunk(3)
            .Select(elfGroup => FindSharedRucksackItem(elfGroup))
            .Sum(sharedRucksackItem => sharedRucksackItem.Priority);
    }

    private static RucksackItem FindSharedRucksackItem(IEnumerable<Rucksack> rucksacks)
    {
        return FindSharedRucksackItem(rucksacks.Select(r => r.AllItems).ToArray());
    }

    private static RucksackItem FindSharedRucksackItem(params IEnumerable<RucksackItem>[] rucksackItems)
    {
        return rucksackItems
            .Aggregate((previous, current) => previous.Intersect(current))
            .Single();
    }

    private struct Rucksack
    {
        private readonly Lazy<RucksackItem[]> _left;

        private readonly Lazy<RucksackItem[]> _right;

        public RucksackItem[] LeftCompartment => _left.Value;

        public RucksackItem[] RightCompartment => _right.Value;

        public RucksackItem[] AllItems { get; }

        public Rucksack(IEnumerable<RucksackItem> items)
        {
            var allItems = items.ToArray();
            AllItems = allItems;
            _left = new(() => allItems.Take(allItems.Length / 2).ToArray());
            _right = new(() => allItems.Skip(allItems.Length / 2).ToArray());
        }
    }

    private struct RucksackItem
    {
        public int Priority { get; }

        public RucksackItem(char value)
        {
            Priority = ((int)value) - (char.IsUpper(value) ? 38 : 96);
        }
    }
}
