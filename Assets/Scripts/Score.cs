using System;
using System.Text;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ScoreText; 

    private Model Model;
    private int Count;
    private int Goal;
    private readonly StringBuilder SB = new StringBuilder();

    public Action OnGoalReached;

    public void Initialize(Model model)
    {
        Model = model;        
        Model.StackedHexons += OnIncrease;
    }

    public void StartLevel(Level level)
    {
        Count = 0;
        Goal = level.RequaredScore;
        UpdateUI();
    }

    private void OnIncrease(int count)
    {
        Count += count;

        UpdateUI();

        if(Count >= Goal)
        {
            OnGoalReached?.Invoke();
        }
    }

    private void UpdateUI()
    {
        SB.Clear();
        SB.Append(Count).Append(" / ").Append(Goal);
        ScoreText.text = SB.ToString();
    }

    private void OnDestroy()
    {
        Model.StackedHexons -= OnIncrease;
    }
}
