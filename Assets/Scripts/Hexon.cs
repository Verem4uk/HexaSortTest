using System;

public class Hexon
{
    public HexColorType ColorType { private set; get; }

    public Action<HexonStack> StackChanged;

    public Hexon (HexColorType colorType)
    {
        ColorType = colorType;
    }

    public void ChangeStack(HexonStack stack)
    {
        StackChanged?.Invoke(stack);
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
    Black,
    White
}

