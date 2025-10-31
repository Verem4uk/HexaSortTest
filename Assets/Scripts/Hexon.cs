public class Hexon
{
    public HexColorType ColorType { private set; get; }

    public Hexon (HexColorType colorType)
    {
        ColorType = colorType;
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

