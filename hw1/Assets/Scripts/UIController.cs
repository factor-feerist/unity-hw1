using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private DiceGameModel diceGameModel;
    [SerializeField] private DiceRoller diceRoller;
    [SerializeField] private Rigidbody diePrefab;

    [SerializeField] private TMP_Text dieCountText;
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text minScoreForVictoryText;
    [SerializeField] private TMP_Text minScoreForTieText;

    [SerializeField] private TMP_InputField dieCountInput;

    [SerializeField] private Button rollButton;

    private void Awake()
    {
        diceGameModel.OnDieCountChanged.AddListener(UpdateDieCountText);
        diceGameModel.OnScoreCalculated.AddListener(UpdateCurrentScoreText);
        diceGameModel.OnTargetScoresChanged.AddListener(UpdateTargetScoresText);

        dieCountInput.onValueChanged.AddListener(HandleDieCountInputFieldChanged);
        rollButton.onClick.AddListener(HandleRollButtonClick);

        StartNewRound(3);
    }

    private void HandleDieCountInputFieldChanged(string text)
    {
        if (!string.IsNullOrEmpty(text) && int.TryParse(text, out var userDieCount))
        {
            diceGameModel.InitializeRound(userDieCount);
            diceRoller.SetupDice(diePrefab, diceGameModel.DieCount);
        }
    }

    private void HandleRollButtonClick()
    {
        if (!string.IsNullOrEmpty(dieCountInput.text) && int.TryParse(dieCountInput.text, out var userDieCount))
        {
            if (userDieCount != diceGameModel.DieCount)
            {
                StartNewRound(userDieCount);
            }
            else
            {
                currentScoreText.text = "Выпавшее значение: -";
            }
        }

        diceRoller.OnRoll();
    }

    private void StartNewRound(int count)
    {
        diceGameModel.InitializeRound(count);
        diceRoller.SetupDice(diePrefab, diceGameModel.DieCount);
        currentScoreText.text = "Выпавшее значение: -";
    }

    private void UpdateDieCountText(int count)
    {
        dieCountText.text = $"Число кубиков: {count}";
    } 

    private void UpdateCurrentScoreText(int score)
    {
        currentScoreText.text = $"Выпавшее значение: {score}";
    }

    private void UpdateTargetScoresText(int minScoreForTie, int minScoreForVictory)
    {
        minScoreForTieText.text = $"Ничья от: {minScoreForTie} очков";
        minScoreForVictoryText.text = $"Победа от: {minScoreForVictory} очков";
    }
}
