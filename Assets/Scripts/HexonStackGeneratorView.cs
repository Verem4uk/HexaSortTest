using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private HexonView HexonView;
    [SerializeField]
    private float hexHeight = 1f;

    [SerializeField]
    private HexonStackView StackHolder;

    [SerializeField]
    private List<Transform> StackPositions;

    private HexonStackGenerator Generator;
    private Controller Controller;

    private Dictionary<HexonStack, HexonStackView> Stacks = new();

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
            var stack = stacks[i];
            stackHolder.Initialize(stack, Controller);
            Stacks.Add(stack, stackHolder);
            stack.Depleted += OnStackDepleted;
            var hexons = stack.PeekAll();

            for (int j = 0; j < hexons.Length; j++)
            {
                var hexon = hexons[j];
                var hexonView = Instantiate(HexonView,stackHolder.transform.position + Vector3.up * (j * hexHeight), Quaternion.identity, stackHolder.transform);                
                var renderer = hexonView.GetComponentInChildren<Renderer>();                
                renderer.material.color = colorDatabase.GetColor(hexon.ColorType);
                hexonView.Initialize(hexon, Controller);
            }
        }        
    }

    public Vector3 GetPositionByStack(HexonStack stack)
    {
        var view = Stacks[stack];
        var hexonsCount = stack.PeekAll().Length;
        var cellPosition = Controller.GetPositionForMove(stack.Cell);
        return cellPosition + Vector3.up * (hexonsCount * hexHeight);
    }

    public void OnStackDepleted(HexonStack hexonStack)
    {  
        Stacks.Remove(hexonStack);
        //var view = Stacks[hexonStack];
        //Destroy(view.gameObject);
    }

    public void CleanUpStacks()
    {        
        var stacksCopy = Stacks.Keys.ToList();
        
        foreach (var stack in stacksCopy)
        {
            stack.Delete();
        }

        Stacks.Clear();
    }


    private void OnDestroy()
    {
        Generator.AllStacksWereUsed -= Spawn;
    }
}
