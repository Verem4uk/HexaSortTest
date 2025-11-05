using System.Collections.Generic;

public class Cell
{
    public int Q { private set; get; } 
    public int R { private set; get; }  
    public List<Cell> Neighbors { get; private set; }
    public bool IsOccupied => Stack != null;

    private HexonStack Stack;

    public Cell(int q, int r)
    {
        Q = q;
        R = r;        
        Neighbors = new List<Cell>();
    }

    public void AddNeighbor(Cell cell)
    {
        if (cell != null && !Neighbors.Contains(cell))
        {
            Neighbors.Add(cell);
        }            
    }

    public override string ToString()
    {
        return $"HexCell ({Q},{R}) Neigh={Neighbors.Count}";
    }
}
