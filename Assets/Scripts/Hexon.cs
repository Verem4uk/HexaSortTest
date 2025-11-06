using System;

public class Hexon
{
    public HexColorType ColorType { private set; get; }

    public Action<HexonStack> StackChanged;
    public Action Sold;

    public Hexon (HexColorType colorType)
    {
        ColorType = colorType;
    }

    public void ChangeStack(HexonStack stack)
    {
        StackChanged?.Invoke(stack);
    }
    public void Sell()
    {
        Sold?.Invoke();
    }
}

public enum HexColorType
{
    Red,
    Green,
    Blue,
    Yellow,
    Purple,
    Turquoise,
    Orange,
    White
}

