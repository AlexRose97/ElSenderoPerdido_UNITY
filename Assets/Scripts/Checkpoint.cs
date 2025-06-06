using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerDetection"))
        {
            GameManager.Instance.SetCheckpoint(other.transform.position);
        }
    }
}
