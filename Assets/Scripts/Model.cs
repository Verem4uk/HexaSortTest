
using System.Collections.Generic;
using UnityEngine;

public class Model
{
    private Grid Grid;
    private Cell LastCellOperation;
    private Queue<Cell> CellsForCheck = new();

    public Model (Grid grid)
    {
        Grid = grid;
    }

    public void Place(HexonStack stack, Cell cell)
    {        
        cell.PlaceStack(stack);
        stack.Place(cell);
        LastCellOperation = cell;
    }

    public int CheckMoves()
    {
        if(LastCellOperation == null)
        {
            Debug.Log("No moves");
            return 0;
        }
        var neighbors = LastCellOperation.Neighbors;
        if (neighbors.Count == 0)
        {
            Debug.Log("No moves");
            return 0;
        }

        var counter = 0;
        var targetStack = LastCellOperation.Stack;
        var color = targetStack.Peek().ColorType;

        foreach (var neighbor in neighbors)
        {           
            if (!neighbor.IsOccupied)
            {
                continue;
            }

            var neighborStack = neighbor.Stack;
            var buffer = new Stack<Hexon>();

            while (!neighborStack.CheckForEmpty() &&
                neighborStack.Peek().ColorType == color)
            {
                var hexon = neighborStack.Pop();                
                buffer.Push(hexon);
            }

            if(buffer.Count == 0)
            {
                continue;
            }

            counter = buffer.Count;
                        
            while (buffer.Count > 0)
            {
                var hexon = buffer.Pop();
                targetStack.Push(hexon);                
            }

            if(neighbor.IsOccupied)
            {
                CellsForCheck.Enqueue(neighbor);
            }

            return counter;
        }

        LastCellOperation = CellsForCheck.Count > 0 ? CellsForCheck.Dequeue() : null;
        return 0;
    }

}
