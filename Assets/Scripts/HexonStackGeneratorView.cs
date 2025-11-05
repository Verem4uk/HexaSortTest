using UnityEngine;
using System.Collections.Generic;
using System;

public class HexonStackGeneratorView : MonoBehaviour
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
    private ColorsDatabase colorDatabase;
    [SerializeField]
    private GameObject hexPrefab;
    [SerializeField]
    private float hexHeight = 1f;

    [SerializeField]
    private HexonStackView StackHolder;

    [SerializeField]
    private List<Transform> StackPositions;

    private HexonStackGenerator Generator;
    private Controller Controller;

    public void Initialize(Controller controller)
    {
        Controller = controller;
        Generator = new HexonStackGenerator(stacksCount, minStackHeight, maxStackHeight, twoColorChance);
        Generator.AllStacksWereUsed += Spawn;
        Spawn();
    }
        
    public void Spawn()    
    {   
        var stacks = Generator.GenerateStacks();

        for (int i = 0; i < stacks.Count; i++)
        {                     
            var basePos = StackPositions[i].position;            
            var stackHolder = Instantiate(StackHolder, basePos, Quaternion.identity, StackPositions[i]);
            stackHolder.Initialize(stacks[i], Controller);
                                                
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
