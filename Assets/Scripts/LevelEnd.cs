using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private int scoreToGive = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerDetection"))
        {
            GameManager.Instance.AddScore(scoreToGive);
            GameManager.Instance.FinishLevel();
        }
    }
}