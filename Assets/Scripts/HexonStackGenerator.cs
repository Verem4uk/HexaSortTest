using System;
using System.Collections.Generic;
using UnityEngine;
public class HexonStackGenerator
{
    private int StacksCount;
    private int MinStackHeight;
    private int MaxStackHeight;
    private float TwoColorChance;
    private System.Random Random = new System.Random();

    private List<HexonStack> SpawnedStacks;
    public Action AllStacksWereUsed;
    
    public HexonStackGenerator(int stacksCount, int minStackHeight, int maxStackHeight, float twoColorChance)
    {
        StacksCount = stacksCount;
        MinStackHeight = minStackHeight;
        MaxStackHeight = maxStackHeight;
        TwoColorChance = twoColorChance;
    }

    public List<HexonStack> GenerateStacks()
    {
        var allTypes = Enum.GetValues(typeof(HexColorType));
        SpawnedStacks = new List<HexonStack>();

        for (int i = 0; i < StacksCount; i++)
        {
            var stack = new HexonStack();
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

            SpawnedStacks.Add(stack);
            stack.Placed += OnStackUsed;
        }

        return SpawnedStacks;
    }

    private void OnStackUsed(HexonStack stack, Cell cell)
    {
        stack.Placed -= OnStackUsed;
        SpawnedStacks.Remove(stack);
        UnityEngine.Debug.Log("On Stack Used");
        if (SpawnedStacks.Count == 0)
        {
            UnityEngine.Debug.Log("AllStacksWereUsed");
            AllStacksWereUsed?.Invoke();
        }
    }
}
