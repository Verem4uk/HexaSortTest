using System;
using System.Collections.Generic;

public class HexStackGenerator
{
    private int StacksCount;
    private int MinStackHeight;
    private int MaxStackHeight;
    private float TwoColorChance;
    private Random Random = new Random();

    public HexStackGenerator(int stacksCount, int minStackHeight, int maxStackHeight, float twoColorChance)
    {
        StacksCount = stacksCount;
        MinStackHeight = minStackHeight;
        MaxStackHeight = maxStackHeight;
        TwoColorChance = twoColorChance;
    }

    public List<Stack<Hexon>> GenerateStacks()
    {
        var allTypes = Enum.GetValues(typeof(HexColorType));
        var stacks = new List<Stack<Hexon>>();

        for (int i = 0; i < StacksCount; i++)
        {
            var stack = new Stack<Hexon>();
            int stackHeight = Random.Next(MinStackHeight, MaxStackHeight + 1);
            bool useTwoColors = Random.NextDouble() < TwoColorChance && stackHeight > 1;

            if (useTwoColors)
            {
                var first = (HexColorType)allTypes.GetValue(Random.Next(allTypes.Length));
                HexColorType second;
                do
                {
                    second = (HexColorType)allTypes.GetValue(Random.Next(allTypes.Length));
                } 
                while (second == first);

                int firstCount = Random.Next(1, stackHeight);
                int secondCount = stackHeight - firstCount;

                for (int j = 0; j < firstCount; j++)
                {
                    stack.Push(new Hexon(first));
                }
                    
                for (int j = 0; j < secondCount; j++)
                {
                    stack.Push(new Hexon(second));
                }                    
            }
            else
            {
                var color = (HexColorType)allTypes.GetValue(Random.Next(allTypes.Length));
                for (int j = 0; j < stackHeight; j++)
                {
                    stack.Push(new Hexon(color));
                }                    
            }

            stacks.Add(stack);
        }

        return stacks;
    }
}
