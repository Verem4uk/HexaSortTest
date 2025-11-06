
using System.Collections.Generic;
using UnityEngine;

public class Model
{
    private Grid Grid;
    private Cell NextCellOperation;    
    private Controller Controller;

    private List<HexonStack> UsedStacks = new();
    private Queue<Cell> CellsForCheck = new();

    public Model (Grid grid, Controller controller)
    {
        Grid = grid;
        Controller = controller;
    }

    public bool HasMoves => NextCellOperation != null;

    public void Place(HexonStack stack, Cell cell)
    {        
        cell.PlaceStack(stack);
        stack.Place(cell);
        NextCellOperation = cell;
    }

    public void NextMove()
    {        
        var neighbors = NextCellOperation.Neighbors;
        if (neighbors.Count == 0)
        {
            NextCellOperation = CellsForCheck.Count > 0 ? CellsForCheck.Dequeue() : null;

            if (NextCellOperation == null)
            {
                SellStacks();
            }
        }

        var counter = 0;
        var targetStack = NextCellOperation.Stack;        
        var color = targetStack.Peek().ColorType;

        foreach (var neighbor in neighbors)
        {            
            if (!neighbor.IsOccupied)
            {
                continue;
            }

            var neighborStack = neighbor.Stack;
            var buffer = new Stack<Hexon>();

            while (neighborStack.Peek().ColorType == color)
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

    public void SellStacks()
    {
        foreach (var stack in UsedStacks)
        {
            if(stack == null || stack.IsEmpty())
            {
                continue;
            }
            stack.CheckAmount();
            if (!stack.IsEmpty())
            {
                CellsForCheck.Enqueue(stack.Cell);
            }
        }

        UsedStacks.Clear();
        NextCellOperation = CellsForCheck.Count > 0 ? CellsForCheck.Dequeue() : null;
    }
}

