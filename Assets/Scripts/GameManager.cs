using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int currentScore = 0;
    public void AddScore(int amount) => currentScore += amount;
    public int GetScore() => currentScore;

    private Vector3 _lastCheckpointPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void SetCheckpoint(Vector3 position)
    {
        _lastCheckpointPosition = position;
    }

    public Vector3 GetCheckpointPosition()
    {
        return _lastCheckpointPosition;
    }
    
    public void FinishLevel()
    {
        Debug.Log("Nivel completado. Puntaje: " + currentScore);
        // Guardar datos, pasar al siguiente nivel, etc.
    }
}