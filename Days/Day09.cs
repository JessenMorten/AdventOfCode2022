using System.Drawing;

namespace AdventOfCode2022.Days;

internal class Day09 : IDay
{
    private readonly List<(char direction, int length)> _motions = new();

    public void Setup()
    {
        File
            .ReadAllLines("Input/09.txt")
            .Select(line => line.Split(' '))
            .ToList()
            .ForEach(e =>
            {
                _motions.Add((e[0][0], int.Parse(e[1])));
            });
    }

    public object SolveA()
    {
        var navigator = new Navigator(new(0, 0));

        foreach (var motion in _motions)
        {
            for (var i = 0; i < motion.length; i++)
            {
                navigator.Move(motion.direction);
            }
        }

        return navigator.TailPath
            .DistinctBy(p => $"{p.X}:{p.Y}")
            .Count();
    }

    public object SolveB()
    {
        return "?";
    }

    private sealed class Navigator
    {
        public List<Point> HeadPath { get; } = new();

        public List<Point> TailPath { get; } = new();

        public Point Head => HeadPath.Last();

        public Point Tail => TailPath.Last();

        public Navigator(Point start)
        {
            HeadPath.Add(start);
            TailPath.Add(start);
        }

        public void Move(char direction)
        {
            MoveHead(direction);

            if (ShouldTailMove())
            {
                MoveTail(direction);
            }
        }

        private void MoveHead(char direction)
        {
            var newHead = direction switch
            {
                'U' => new Point(Head.X, Head.Y + 1),
                'D' => new Point(Head.X, Head.Y - 1),
                'R' => new Point(Head.X + 1, Head.Y),
                'L' => new Point(Head.X - 1, Head.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
            HeadPath.Add(newHead);
        }

        private void MoveTail(char direction)
        {
            var newTail = direction switch
            {
                'U' => new Point(Head.X, Head.Y - 1),
                'D' => new Point(Head.X, Head.Y + 1),
                'R' => new Point(Head.X - 1, Head.Y),
                'L' => new Point(Head.X + 1, Head.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
            TailPath.Add(newTail);
        }

        private bool ShouldTailMove()
        {
            var xDiff = Math.Abs(Head.X - Tail.X);
            var yDiff = Math.Abs(Head.Y - Tail.Y);
            return xDiff > 1 || yDiff > 1;
        }
    }
}