using System;
using UnityEngine;

public enum RotationDirection
{
    Clockwise = 1,
    CounterClockwise = -1
}

public class CubeRotator : MonoBehaviour
{
    private const float Tolerance = 0.01f;

    [SerializeField] [Range(3f,100f)] private float radius = 5f;
    [SerializeField] [Range(0f, 1000f)] private float rotationSpeed = 10f;
    [SerializeField] private RotationDirection rotationDirection =  RotationDirection.Clockwise;
    [SerializeField] [Min(1)] private int cubesCount = 8;
    
    [SerializeField] private GameObject cubePrefab;

    private float _initialAngle;
    private float _previousRadius;

    private void Awake()
    {
        for (var i = 0; i < cubesCount; i++)
        {
            _ = Instantiate(cubePrefab, transform);
        }

        ArrangeCubes();
    }

    private void Update()
    {
        if (Math.Abs(_previousRadius - radius) > Tolerance)
        {
            ArrangeCubes();
        }

        var deltaAngle = (int)rotationDirection * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, deltaAngle, 0);
    }

    private void ArrangeCubes()
    {
        _initialAngle = 2 * Mathf.PI / cubesCount;
        var i = 0;

        foreach (Transform cube in transform)
        {
            var angle = _initialAngle * i;
            cube.localPosition = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
            i++;
        }

        _previousRadius = radius;
    }
}
