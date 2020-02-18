using UnityEngine;

public class MovingEnviroment : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;

    private void Start()
    {
        MoveSystem.instance.subscribe(transform);
    }
}