using UnityEngine;

public class BlockerWall : MonoBehaviour
{
    // Call this On Step Complete event to remove this blocker wall
    public void Open()
    {
        gameObject.SetActive(false);
    }
}
