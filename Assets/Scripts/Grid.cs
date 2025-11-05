using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int Radius;
    private Dictionary<(int, int), Cell> Cells;

    private static readonly (int dq, int dr)[] _directions = new (int, int)[]
    {
        (1, 0), (1, -1), (0, -1),
        (-1, 0), (-1, 1), (0, 1)
    };

    public Grid(int radius)
    {
        Radius = radius;
        Cells = new Dictionary<(int, int), Cell>();
        GenerateHexonShape();
        ConnectNeighbors();
    }

    private void GenerateHexonShape()
    {
        for (int q = -Radius; q <= Radius; q++)
        {
            int r1 = Mathf.Max(-Radius, -q - Radius);
            int r2 = Mathf.Min(Radius, -q + Radius);
            for (int r = r1; r <= r2; r++)
            {
                Cells[(q, r)] = new Cell(q, r);
            }
        }
    }

    private void ConnectNeighbors()
    {
        foreach (var cell in Cells.Values)
        {
            foreach (var (dq, dr) in _directions)
            {
                var neighborKey = (cell.Q + dq, cell.R + dr);
                if (Cells.TryGetValue(neighborKey, out var neighbor))
                {
                    cell.AddNeighbor(neighbor);
                }
            }
        }
    }

    public IEnumerable<Cell> GetAllCells() => Cells.Values;
}
