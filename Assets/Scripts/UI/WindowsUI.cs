using UnityEngine;

public class WindowsUI : MonoBehaviour
{
    public Pool WindowPool {  get; private set; }

    private void Awake()
    {
        WindowPool = GetComponent<Pool>();
    }
}
