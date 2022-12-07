namespace AdventOfCode2022.Days;

internal class Day02 : IDay
{
    private readonly List<Tuple<char, char>> _rounds = new();

    public void Setup()
    {
        File
            .ReadAllLines("Input/02.txt")
            .Select(line => Tuple.Create(line[0], line[2]))
            .ToList()
            .ForEach(_rounds.Add);
    }

    public object SolveA()
    {
        var totalScore = 0;

        foreach (var round in _rounds)
        {
            var opponent = ToAssumedColumnValue(round.Item1);
            var me = ToAssumedColumnValue(round.Item2);
            var result = Evaluate(opponent, me);
            totalScore += CalculateScore(me) + CalculateScore(result);
        }

        return totalScore;
    }

    public object SolveB()
    {
        var totalScore = 0;

        foreach (var round in _rounds)
        {
            var opponent = ToKnownColumnValue(round.Item1);
            var result = ToKnownColumnValue(round.Item2);
            var me = result switch
            {
                ColumnValue.Won => ForceWin(opponent),
                ColumnValue.Lost => ForceLose(opponent),
                ColumnValue.Tie => ForceTie(opponent),
                _ => throw new NotSupportedException($"Unknown result: {result}")
            };
            totalScore += CalculateScore(me) + CalculateScore(result);
        }
        return totalScore;
    }

    private static ColumnValue ToAssumedColumnValue(char c) => c switch
    {
        'A' => ColumnValue.Rock,
        'B' => ColumnValue.Paper,
        'C' => ColumnValue.Scissor,
        'X' => ColumnValue.Rock,
        'Y' => ColumnValue.Paper,
        'Z' => ColumnValue.Scissor,
        _ => throw new NotSupportedException($"Unknown char: {c}")
    };

    private static ColumnValue ToKnownColumnValue(char c) => c switch
    {
        'A' => ColumnValue.Rock,
        'B' => ColumnValue.Paper,
        'C' => ColumnValue.Scissor,
        'X' => ColumnValue.Lost,
        'Y' => ColumnValue.Tie,
        'Z' => ColumnValue.Won,
        _ => throw new NotSupportedException($"Unknown char: {c}")
    };

    private static int CalculateScore(ColumnValue columnValue) => columnValue switch
    {
        ColumnValue.Rock => 1,
        ColumnValue.Paper => 2,
        ColumnValue.Scissor => 3,
        ColumnValue.Lost => 0,
        ColumnValue.Tie => 3,
        ColumnValue.Won => 6,
        _ => throw new NotSupportedException($"Unknown column value: {columnValue}")
    };

    private static ColumnValue Evaluate(ColumnValue opponent, ColumnValue me)
    {
        if (opponent == me)
            return ColumnValue.Tie;

        var lost =
            (opponent == ColumnValue.Rock && me == ColumnValue.Scissor)
            || (opponent == ColumnValue.Paper && me == ColumnValue.Rock)
            || (opponent == ColumnValue.Scissor && me == ColumnValue.Paper);

        return lost ? ColumnValue.Lost : ColumnValue.Won;
    }

    private static ColumnValue ForceWin(ColumnValue opponent) => opponent switch
    {
        ColumnValue.Rock => ColumnValue.Paper,
        ColumnValue.Paper => ColumnValue.Scissor,
        ColumnValue.Scissor => ColumnValue.Rock,
        _ => throw new NotSupportedException($"Unknown opponent: {opponent}")
    };

    private static ColumnValue ForceLose(ColumnValue opponent) => opponent switch
    {
        ColumnValue.Rock => ColumnValue.Scissor,
        ColumnValue.Paper => ColumnValue.Rock,
        ColumnValue.Scissor => ColumnValue.Paper,
        _ => throw new NotSupportedException($"Unknown opponent: {opponent}")
    };

    private static ColumnValue ForceTie(ColumnValue opponent) => opponent;

    private enum ColumnValue
    {
        Rock,
        Paper,
        Scissor,
        Tie,
        Won,
        Lost
    }
}
