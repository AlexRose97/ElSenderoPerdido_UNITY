using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
}