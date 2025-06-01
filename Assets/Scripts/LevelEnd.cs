using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private int scoreToGive = 100;
    [SerializeField] private GameObject levelEndUI;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerDetection"))
        {
            GameManager.Instance.AddScore(scoreToGive);
            GameManager.Instance.FinishLevel(true);
            PauseMenuManager.Instance.ShowGameOverMenu(); // muestra UI
        }
    }
}