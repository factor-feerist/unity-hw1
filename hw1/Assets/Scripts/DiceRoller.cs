using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceRoller : MonoBehaviour
{
    private const float Tolerance = 0.1f;
    private const float Radius = 5;
    private const int ForceMultiplier = 50;

    [SerializeField] private DiceGameModel diceGameModel;

    private Rigidbody _diePrefab;
    private Rigidbody[] _dice;
    private bool _isRolling;

    private readonly (Func<Rigidbody, Vector3> getDirectionVector, int score)[] _directionsWithScores =
    {
        (x => x.transform.forward, 1),
        (x => x.transform.up, 2),
        (x => -x.transform.right, 3),
        (x => x.transform.right, 4),
        (x => -x.transform.up, 5),
        (x => -x.transform.forward, 6)
    };

    private void Update()
    {
        if (_dice == null || _dice.Length == 0)
            return;

        var isRolling = _dice.Any(x
            => x.linearVelocity.sqrMagnitude > Tolerance
               || x.angularVelocity.sqrMagnitude > Tolerance);

        if (!isRolling && _isRolling)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            SetScore();
        }

        _isRolling = isRolling;
    }

    public void SetupDice(Rigidbody diePrefab, int dieCount)
    {
        _diePrefab = diePrefab;

        if (_dice != null)
        {
            foreach (var die in _dice)
                if (die != null)
                    Destroy(die.gameObject);
        }

        _dice = new Rigidbody[dieCount];

        var initialAngle = 2 * Mathf.PI / dieCount;
        for (var i = 0; i < dieCount; i++)
        {
            var die = Instantiate(_diePrefab, transform);
            var angle = initialAngle * i;
            die.transform.localPosition = new Vector3(Radius * Mathf.Cos(angle), 0, Radius * Mathf.Sin(angle));

            _dice[i] = die;
        }
    }

    public void OnRoll()
    {
        if (_isRolling)
            return;

        RollDice();
    }

    private void RollDice()
    {
        _isRolling = true;
        foreach (var die in _dice)
        {
            die.AddForce(Random.insideUnitSphere * ForceMultiplier, ForceMode.Impulse);
            die.AddTorque(Random.insideUnitSphere * ForceMultiplier, ForceMode.Impulse);
        }
    }

    private void SetScore()
    {
        var totalScore = 0;
        foreach (var die in _dice)
        {
            var maxDot = float.MinValue;
            var score = 0;
            foreach (var directionsWithScore in _directionsWithScores)
            {
                var dot = Vector3.Dot(directionsWithScore.getDirectionVector(die), Vector3.up);

                if (dot <= maxDot)
                    continue;

                maxDot = dot;
                score = directionsWithScore.score;
            }
            totalScore += score;
        }

        diceGameModel.SetCurrentScore(totalScore);
    }
}
