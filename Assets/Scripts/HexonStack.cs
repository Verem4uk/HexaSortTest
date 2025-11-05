using System;
using System.Collections;
using System.Collections.Generic;

public class HexonStack
{
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

    public Hexon Pop() => Stack.Pop();
    public Hexon Peek() => Stack.Peek();

    public bool CheckForEmpty()
    {
        if(Stack.Count == 0)
        {
            Cell.CleanUp();
            Cell = null;
            Depleted?.Invoke(this);
            Stack = null;
            return true;
        }
        return false;
    }
}
