using System;
using System.Collections;
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
                var hexonView = Instantiate(HexonView,stackHolder.transform.position + Vector3.up * (j * 0.3f), Quaternion.identity, stackHolder.transform);                
                var renderer = hexonView.GetComponentInChildren<Renderer>();                
                renderer.material.color = colorDatabase.GetColor(hexon.ColorType);
                hexonView.Initialize(hexon, Controller);
            }
        }        
    }

    public HexonStackView GetViewByStack(HexonStack stack) => Stacks[stack];    

    public void OnStackDepleted(HexonStack hexonStack)
    {
        var view = Stacks[hexonStack];
        if(view == null)
        {
            return;
        }
        Stacks.Remove(hexonStack);
        StartCoroutine(DelayDestroy(view, 0.3f));
    }

    private IEnumerator DelayDestroy(HexonStackView view, float duration)
    {       
        yield return new WaitForSeconds(duration);       
        Destroy(view.gameObject);
    }

    public void CleanUpStacks()
    {        
        var stacksCopy = Stacks.Keys.ToList();
        
        foreach (var stack in stacksCopy)
        {
            if(stack.Cell == null)
            {
                stack.Delete();
            }            
        }

        Stacks.Clear();
    }


    private void OnDestroy()
    {
        Generator.AllStacksWereUsed -= Spawn;
    }
}
