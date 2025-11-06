using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private GridGenerator GridGenerator;

    [SerializeField]
    private HexonStackGeneratorView StackGenerator;

    private Model Model;
    private bool InputIsLocked;

    private void Start()
    {
        //Entry point

        GridGenerator.Initialize(out var grid);
        Model = new Model(grid, this);
        StackGenerator.Initialize(this);        
    }

    public bool Place(HexonStack stack, Cell cell)
    {        
        if (InputIsLocked || cell.IsOccupied)
        {
            return false;
        }

        Model.Place(stack, cell);
        NextMove();
        return true;
    }

    public void NextMove()
    {
        InputIsLocked = true;

        while (Model.HasMoves)
        {
            Model.NextMove();
        }

        InputIsLocked = false;
    }

    public void CheckMark(Cell cell) => GridGenerator.CheckMark(cell);
    public Vector3 GetPositionForMove(Cell cell) => GridGenerator.GetPositionByCell(cell);
    public Vector3 GetPositionForMove(HexonStack stack) => StackGenerator.GetPositionByStack(stack);
}
