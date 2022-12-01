using System.Reflection;
using AdventOfCode2022;

var type = typeof(IDay);

Assembly
    .GetExecutingAssembly()
    .GetTypes()
    .Where(t => type.IsAssignableFrom(t) && t.IsClass)
    .Select(t => Activator.CreateInstance(t) ?? throw new NotSupportedException())
    .Select(day => (DayOfMonth: int.Parse(day.GetType().Name[3..]), Solution: (IDay)day))
    .OrderBy(day => day.DayOfMonth)
    .ToList()
    .ForEach(day =>
    {
        Console.WriteLine($"December {day.DayOfMonth}");

        day.Solution.Setup();

        Console.WriteLine($"    A = {day.Solution.SolveA()}");
        Console.WriteLine($"    B = {day.Solution.SolveB()}");
    });
