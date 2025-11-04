using System;
using System.Collections;
using System.Collections.Generic;

public class HexonStack
{
    private Stack<Hexon> Stack;
    private HexCell Cell;

    public Action<HexonStack> Placed;

    public HexonStack()
    {
        Stack = new Stack<Hexon>();
    }

    public void Place(HexCell cell)
    {
        Cell = cell;
        Placed?.Invoke(this);
    }

    public Hexon[] PeekAll()
    {
        var hexons = Stack.ToArray();
        Array.Reverse(hexons);
        return hexons;
    }

    public void Push(Hexon hexon) => Stack.Push(hexon);

    public Hexon Pop() => Stack.Pop();

    public Hexon Peek() => Stack.Peek();
}
