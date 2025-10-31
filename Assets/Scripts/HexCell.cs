using System.Collections.Generic;

public class HexCell
{
    public readonly int Q; 
    public readonly int R;    
    public List<HexCell> Neighbors { get; private set; }

    public HexCell(int q, int r)
    {
        Q = q;
        R = r;        
        Neighbors = new List<HexCell>();
    }

    public void AddNeighbor(HexCell cell)
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
