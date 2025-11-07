using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class HexonStack
{
    private static int MaxStackAmount = 8;
    private Stack<Hexon> Stack;
    public Cell Cell { private set; get; }

    public Action<HexonStack, Cell> Placed;
    public Action<HexonStack> Depleted;

    public HexonStack()
    {
        Stack = new Stack<Hexon>();
    }

    public void Place(Cell cell)
    {
        Cell = cell;
        Placed?.Invoke(this, cell);
    }

    public Hexon[] PeekAll()
    {
        var hexons = Stack.ToArray();
        Array.Reverse(hexons);
        return hexons;
    }

    public void Push(Hexon hexon)
    {
        Stack.Push(hexon);
        hexon.ChangeStack(this);     
    }

    public int CheckAmount()
    {
        var count = 0;
        if (Stack.Count < MaxStackAmount)
        {
            return count;
        }
        
        var topColor = Stack.Peek().ColorType;        
        var buffer = new Stack<Hexon>();

        while (Stack.Count > 0 && Stack.Peek().ColorType == topColor)
        {
            buffer.Push(Stack.Pop());
        }

        if (buffer.Count >= MaxStackAmount)
        {
            count = buffer.Count;
            while (buffer.Count > 0)
            {
                var hexon = buffer.Pop();
                hexon.Sell(); 
            }

            UnlockNeibors();
        }
        else
        {            
            while (buffer.Count > 0)
            {
                Stack.Push(buffer.Pop());
            }
        }        

        IsEmpty();
        return count;
    }

    private void UnlockNeibors()
    {
        var neighbors = Cell.Neighbors;

        foreach (var neighbor in neighbors)
        {
            if (neighbor.Type == Cell.CellType.Blocked)
            {
                neighbor.Unlock();
            }
        }
    }

    public Hexon Pop() => Stack.Pop();
    public Hexon Peek() => Stack.Peek();

    public bool IsEmpty()
    {
        if(Stack == null)
        {
            return true;
        }

        if(Stack.Count == 0)
        {
            Delete();
            return true;
        }
        return false;
    }

    public void Delete()
    {
        while(Stack.Count > 0)
        {
            var hexon = Stack.Pop();
            hexon.Sell();
        }

        if(Cell != null)
        {
            Cell.CleanUp();
            Cell = null;
        }        
        Depleted?.Invoke(this);
        Stack = null;
    }
}
