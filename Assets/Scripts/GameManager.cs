using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")] [SerializeField]
    private TextMeshProUGUI textMessage;

    public static GameManager Instance;
    private int _currentScore = 0;
    private bool _isEndGame = false;
    public void AddScore(int amount) => _currentScore += amount;
    public int GetScore() => _currentScore;
    public bool GetEndgame() => _isEndGame;

    private Vector3 _lastCheckpointPosition;

    private void Awake()
    {
        Time.timeScale = 1f;
        if (Instance == null)
            Instance = this;

        textMessage.text = "";
    }

    public void SetCheckpoint(Vector3 position)
    {
        _lastCheckpointPosition = position;
        StartCoroutine(ShowMessage("Checkpoint alcanzado..."));
    }

    public Vector3 GetCheckpointPosition()
    {
        return _lastCheckpointPosition;
    }

    public void FinishLevel(bool isWin)
    {
        Debug.Log("Nivel completado. Puntaje: " + _currentScore);
        _isEndGame = true;
        if (isWin)
        {
            StartCoroutine(ShowMessage("Nivel completado!..."));
        }
        // Guardar datos, pasar al siguiente nivel, etc.
    }

    private IEnumerator ShowMessage(string message)
    {
        float duration = 2.5f; // tiempo total que estará visible
        float blinkInterval = 0.3f; // cada cuánto parpadea

        float timer = 0f;
        while (timer < duration)
        {
            textMessage.text = message;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;

            textMessage.text = "";
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }
    }
}