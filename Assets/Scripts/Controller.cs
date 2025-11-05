using System.Collections;
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
        Model = new Model(grid);
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
        StartCoroutine(DelayCheck());
    }

    private IEnumerator DelayCheck()
    {
        float delay;

        while ((delay = Model.CheckMoves()) > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        InputIsLocked = false;
    }

    public Vector3 GetPositionForMove(Cell cell) => GridGenerator.GetPositionByCell(cell);
    public Vector3 GetPositionForMove(HexonStack stack) => StackGenerator.GetPositionByStack(stack);
}
