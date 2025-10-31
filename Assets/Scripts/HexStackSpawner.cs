using UnityEngine;
using System.Collections.Generic;

public class HexStackSpawner : MonoBehaviour
{    
    [SerializeField, Range(1, 10)] 
    private int stacksCount = 3;
    [SerializeField, Range(1, 10)] 
    private int minStackHeight = 1;
    [SerializeField, Range(1, 10)] 
    private int maxStackHeight = 5;
    [SerializeField, Range(0f, 1f)] 
    private float twoColorChance = 0.5f;

    [SerializeField]
    private HexColorDatabase colorDatabase;
    [SerializeField]
    private GameObject hexPrefab;
    [SerializeField]
    private float hexHeight = 1f;

    [SerializeField]
    private List<Transform> stackPositions = new();

    private List<GameObject> spawnedObjects = new();

    private void Start()
    {
        GenerateAndSpawn();
    }
        
    public void GenerateAndSpawn()
    {
        ClearOld();

        var generator = new HexStackGenerator(stacksCount, minStackHeight, maxStackHeight, twoColorChance);
        var stacks = generator.GenerateStacks();

        for (int i = 0; i < stacks.Count; i++)
        {
            Vector3 basePos;
                       
            basePos = stackPositions[i].position;
            
            var stack = stacks[i];
            var hexons = stack.ToArray();
            System.Array.Reverse(hexons); 

            for (int j = 0; j < hexons.Length; j++)
            {
                var hexon = hexons[j];
                var obj = Instantiate(hexPrefab, basePos + Vector3.up * (j * hexHeight), Quaternion.identity, stackPositions[i]);

                
                var renderer = obj.GetComponentInChildren<Renderer>();
                if (renderer != null)
                    renderer.material.color = colorDatabase.GetColor(hexon.ColorType);

                spawnedObjects.Add(obj);
            }
        }
    }

    private void ClearOld()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }                
        }
        spawnedObjects.Clear();
    }
}
