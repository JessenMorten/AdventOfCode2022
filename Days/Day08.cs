namespace AdventOfCode2022.Days;

internal class Day08 : IDay
{
    private static Tree[,] _treeMap = null!;

    private List<Tree> _allTrees = new List<Tree>();

    public void Setup()
    {
        var lines = System.IO.File.ReadAllLines("Input/08.txt");

        _treeMap = new Tree[lines.Length, lines.First().Length];

        for (var i = 0; i < lines.Length; i++)
            for (var j = 0; j < lines[i].Length; j++)
            {
                var tree = new Tree(int.Parse(lines[i][j].ToString()), i, j);
                _allTrees.Add(tree);
                _treeMap[i, j] = tree;
            }
    }

    public object SolveA()
    {
        return _allTrees.Count(tree =>
            tree.Up.All(t => t.Height < tree.Height)
            || tree.Down.All(t => t.Height < tree.Height)
            || tree.Left.All(t => t.Height < tree.Height)
            || tree.Right.All(t => t.Height < tree.Height));
    }

    public object SolveB()
    {
        return _allTrees.Max(tree =>
        {
            var up = CountVisibleTrees(tree, tree.Up);
            var down = CountVisibleTrees(tree, tree.Down);
            var left = CountVisibleTrees(tree, tree.Left);
            var right = CountVisibleTrees(tree, tree.Right);

            return up * down * left * right;
        });
    }

    private static int CountVisibleTrees(Tree tree, Tree[] neighbors)
    {
        var visibleTrees = new List<Tree>();

        foreach (var neighbor in neighbors)
        {
            visibleTrees.Add(neighbor);
            if (neighbor.Height >= tree.Height)
                break;
        }

        return visibleTrees.Count();
    }

    private sealed class Tree
    {
        public int Height { get; }

        public int Row { get; }

        public int Column { get; }

        public Tree[] Up => GetNeighbors(true, false, false, false);

        public Tree[] Down => GetNeighbors(false, true, false, false);

        public Tree[] Left => GetNeighbors(false, false, true, false);

        public Tree[] Right => GetNeighbors(false, false, false, true);

        public Tree(int height, int row, int column)
        {
            Height = height;
            Row = row;
            Column = column;
        }

        private Tree[] GetNeighbors(bool up, bool down, bool left, bool right)
        {
            var n = new List<Tree>();

            Tree? neighbor = GetNeighbor(this, up, down, left, right);

            while (neighbor is not null)
            {
                n.Add(neighbor);
                neighbor = GetNeighbor(neighbor, up, down, left, right);
            }

            return n.ToArray();
        }

        private static Tree? GetNeighbor(Tree tree, bool up, bool down, bool left, bool right)
        {
            var row = tree.Row;
            var column = tree.Column;

            if (up)
                row--;
            else if (down)
                row++;
            else if (left)
                column--;
            else if (right)
                column++;

            if (row < 0 || column < 0)
                return null;
            if (row >= _treeMap.GetLength(0) || column >= _treeMap.GetLength(1))
                return null;

            return _treeMap[row, column];
        }
    }
}