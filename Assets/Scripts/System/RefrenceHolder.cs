using UnityEngine;

public class RefrenceHolder:MonoBehaviour
{
    public static RefrenceHolder _instance;
    public GameObject[] Anchors;
    public GameObject eventSystem;
    private void Start()
    {
        _instance = this;
    }
}
