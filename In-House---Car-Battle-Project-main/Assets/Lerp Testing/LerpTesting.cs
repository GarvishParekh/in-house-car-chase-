using UnityEngine;

public class LerpTesting : MonoBehaviour
{
    float lerpValue;
    [SerializeField] Transform player;
    [SerializeField] Transform pointA, pointB;

    [Range(0f, 1f)]
    [SerializeField] float speed;

    int a = -4;
    uint b;

    private void Update()
    {
        player.MoveObject(pointA, pointB, speed, lerpValue);
    }

    private void Start()
    {
        b = (uint)a;
        print($"Vlaue: {b}");
    }
}
