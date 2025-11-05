using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private GridGenerator GridGenerator;

    [SerializeField]
    private HexonStackGeneratorView StackGenerator;

    private Model Model;

    private void Start()
    {
        //Entry point

        GridGenerator.Initialize(out var grid);
        Model = new Model(grid);
        StackGenerator.Initialize(this);        
    }

    public bool Place(HexonStack stack, Cell cell)
    {
        Debug.Log("Try place");
        if (cell.IsOccupied)
        {
            return false;
        }
        Model.Place(stack, cell);
        return true;
    }

    public Vector3 GetPositionForMove(Cell cell) => GridGenerator.GetPositionByCell(cell);    
}
