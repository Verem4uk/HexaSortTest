using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HexColorDatabase", menuName = "HexGame/HexColorDatabase")]
public class HexColorDatabase : ScriptableObject
{
    [Serializable]
    public class HexColorEntry
    {
        public HexColorType type;
        public Color color;
    }
        
    public List<HexColorEntry> colors = new List<HexColorEntry>();

    public Color GetColor(HexColorType type)
    {
        foreach (var entry in colors)
        {
            if (entry.type == type)
            {
                return entry.color;
            }                
        }
        
        return Color.white;
    }
}
