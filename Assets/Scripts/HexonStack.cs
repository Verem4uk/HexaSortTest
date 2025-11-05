using System;
using System.Collections;
using System.Collections.Generic;

public class HexonStack
{
    private Stack<Hexon> Stack;
    private Cell Cell;

    public Action<HexonStack,Cell> Placed;

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

    public void Push(Hexon hexon) => Stack.Push(hexon);

    public Hexon Pop()
    {
        return Stack.Pop();
    }

    public Hexon Peek() => Stack.Peek();
}
