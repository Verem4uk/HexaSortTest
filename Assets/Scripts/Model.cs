
using System.Collections.Generic;

public class Model
{
    private Grid Grid;

    public Model (Grid grid)
    {
        Grid = grid;
    }

    public void Place(HexonStack stack, Cell cell)
    {
        cell.PlaceStack(stack);
        stack.Place(cell);
    }
}
