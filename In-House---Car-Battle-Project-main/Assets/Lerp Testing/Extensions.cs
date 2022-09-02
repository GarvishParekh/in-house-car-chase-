using UnityEngine;

public static class Extensions
{

    public static void MoveObject(this Transform objToMove, Transform a, Transform b, float speed, float lerpValue)
    {
        lerpValue = Mathf.MoveTowards(lerpValue, 1, speed * Time.deltaTime);
        objToMove.position = Vector3.Lerp(a.position, b.position, lerpValue);
    }
}