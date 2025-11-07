using System;
using System.Collections.Generic;

public class Model
{
    private Cell NextCellOperation;      
    private List<HexonStack> UsedStacks = new();
    private Queue<Cell> CellsForCheck = new();

    public bool HasMoves => NextCellOperation != null;

    public Action<int> StackedHexons;

    public void Place(HexonStack stack, Cell cell)
    {        
        cell.PlaceStack(stack);
        stack.Place(cell);
        NextCellOperation = cell;
    }

    public void NextMove()
    {       
        if (NextCellOperation.Stack == null || NextCellOperation.Stack.IsEmpty())
        {
            SellStacks();
            return;
        }

        var counter = 0;
        var targetStack = NextCellOperation.Stack;        
        var color = targetStack.Peek().ColorType;
        var neighbors = NextCellOperation.Neighbors;

        foreach (var neighbor in neighbors)
        {            
            if (!neighbor.IsOccupied)
            {
                continue;
            }

            var neighborStack = neighbor.Stack;
            var buffer = new Stack<Hexon>();

            while (!neighborStack.IsEmpty() && neighborStack.Peek().ColorType == color)
            {
                var hexon = neighborStack.Pop();                
                buffer.Push(hexon);
                if (neighborStack.IsEmpty())
                {
                    break;
                }
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

            UsedStacks.Add(targetStack);
        }

        NextCellOperation = CellsForCheck.Count > 0 ? CellsForCheck.Dequeue() : null;             

        if(NextCellOperation == null)
        {
            SellStacks();
        }
    }

    private void SellStacks()
    {
        foreach (var stack in UsedStacks)
        {
            if(stack == null || stack.IsEmpty())
            {
                continue;
            }
            var count = stack.CheckAmount();
            if(count > 0)
            {
                StackedHexons?.Invoke(count);
            }
            if (!stack.IsEmpty())
            {
                CellsForCheck.Enqueue(stack.Cell);
            }
        }

        UsedStacks.Clear();
        NextCellOperation = CellsForCheck.Count > 0 ? CellsForCheck.Dequeue() : null;
    }
}

