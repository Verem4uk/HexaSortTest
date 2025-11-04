using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

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
    private HexStackView StackHolder;

    [SerializeField]
    private List<Transform> stackPositions;

    private HexStackGenerator Generator;


    private void Start()
    {
        Generator = new HexStackGenerator(stacksCount, minStackHeight, maxStackHeight, twoColorChance);
        Generator.AllStacksWereUsed += Spawn;
        Spawn();
    }
        
    public void Spawn()    
    {   
        var stacks = Generator.GenerateStacks();

        for (int i = 0; i < stacks.Count; i++)
        {                     
            var basePos = stackPositions[i].position;            
            var stackHolder = Instantiate(StackHolder, basePos, Quaternion.identity, stackPositions[i]);
            stackHolder.Initialize(stacks[i]);
                                                
            var stack = stacks[i];
            var hexons = stack.PeekAll();

            for (int j = 0; j < hexons.Length; j++)
            {
                var hexon = hexons[j];
                var stackView = Instantiate(hexPrefab,stackHolder.transform.position + Vector3.up * (j * hexHeight), Quaternion.identity, stackHolder.transform);
                                
                var renderer = stackView.GetComponentInChildren<Renderer>();                
                renderer.material.color = colorDatabase.GetColor(hexon.ColorType);
            }
        }        
    }

    private void OnDestroy()
    {
        Generator.AllStacksWereUsed -= Spawn;
    }
}
