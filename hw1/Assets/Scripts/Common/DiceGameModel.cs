using UnityEngine;
using UnityEngine.Events;

public class DiceGameModel : MonoBehaviour
{
    public int DieCount { get; private set; } = 3;
    public int CurrentScore { get; private set; }
    public int MinScoreForVictory { get; private set; }
    public int MinScoreForTie { get; private set; }

    [HideInInspector] public UnityEvent<int> OnDieCountChanged = new();
    [HideInInspector] public UnityEvent<int, int> OnTargetScoresChanged = new();
    [HideInInspector] public UnityEvent<int> OnScoreCalculated = new();

    public void InitializeRound(int dieCount)
    {
        DieCount = Mathf.Clamp(dieCount, 1, 6);
        OnDieCountChanged.Invoke(DieCount);

        GenerateRandomTargetScores();
    }

    public void GenerateRandomTargetScores()
    {
        var maxPossibleScore = DieCount * 6;
        var minPossibleScore = DieCount * 1;

        MinScoreForTie = Random.Range(minPossibleScore + 1, maxPossibleScore);
        MinScoreForVictory = Random.Range(MinScoreForTie + 1, maxPossibleScore + 1);

        OnTargetScoresChanged.Invoke(MinScoreForTie, MinScoreForVictory);
    }

    public void SetCurrentScore(int score)
    {
        CurrentScore = score;
        OnScoreCalculated.Invoke(CurrentScore);
    }
}
