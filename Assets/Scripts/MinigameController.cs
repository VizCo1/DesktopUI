using UnityEngine;

public class MinigameController : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("PLAY!");
        }
    }
}
