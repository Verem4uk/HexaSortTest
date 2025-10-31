using System.Collections.Generic;
using UnityEngine;

public class HexGrid
{
    public readonly int Radius;
    private readonly Dictionary<(int, int), HexCell> _cells;

    private static readonly (int dq, int dr)[] _directions = new (int, int)[]
    {
        (1, 0), (1, -1), (0, -1),
        (-1, 0), (-1, 1), (0, 1)
    };

    public HexGrid(int radius)
    {
        Radius = radius;
        _cells = new Dictionary<(int, int), HexCell>();
        GenerateHexShape();
        ConnectNeighbors();
    }

    private void GenerateHexShape()
    {
        for (int q = -Radius; q <= Radius; q++)
        {
            int r1 = Mathf.Max(-Radius, -q - Radius);
            int r2 = Mathf.Min(Radius, -q + Radius);
            for (int r = r1; r <= r2; r++)
            {
                _cells[(q, r)] = new HexCell(q, r);
            }
        }
    }

    private void ConnectNeighbors()
    {
        foreach (var cell in _cells.Values)
        {
            foreach (var (dq, dr) in _directions)
            {
                var neighborKey = (cell.Q + dq, cell.R + dr);
                if (_cells.TryGetValue(neighborKey, out var neighbor))
                {
                    cell.AddNeighbor(neighbor);
                }
            }
        }
    }

    public IEnumerable<HexCell> GetAllCells() => _cells.Values;
}
