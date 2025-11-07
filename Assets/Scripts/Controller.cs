using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private GridGenerator GridGenerator;

    [SerializeField]
    private HexonStackGeneratorView StackGenerator;

    [SerializeField]
    private Score Score;

    [SerializeField]
    private Messanger Messanger;

    [SerializeField]
    private Level[] Levels;

    private Model Model;
    private bool InputIsLocked;
    private int CurrentLevelIndex;

    private void Start()
    {
        //Entry point

        GridGenerator.Initialize();
        Model = new Model();
        StackGenerator.Initialize(this);

        Score.Initialize(Model);
        Score.OnGoalReached += OnNextLevel;
        Score.StartLevel(Levels[CurrentLevelIndex]);
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

        if(GridGenerator.GridIsFull())
        {
            Messanger.ShowMessage("YOU LOSE!");
        }

        InputIsLocked = false;
    }

    public void OnNextLevel()
    {        
        StackGenerator.CleanUpStacks();
        GridGenerator.CleanUp();        

        if(++CurrentLevelIndex < Levels.Length)
        {            
            Messanger.ShowMessage("Level "+ (CurrentLevelIndex + 1).ToString());
            Score.StartLevel(Levels[CurrentLevelIndex]);
            StackGenerator.Spawn();
        }
        else
        {
            Messanger.ShowMessage("YOU WIN!");
        }        
    }
        
    public Vector3 GetPositionForMove(Cell cell) => GridGenerator.GetPositionByCell(cell);
    public HexonStackView GetStackViewForMove(HexonStack stack) => StackGenerator.GetViewByStack(stack);
}
