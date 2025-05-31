using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 2f; // Tiempo antes de autodestruirse

    void Start()
    {
        Destroy(gameObject, lifeTime); // Destruir después de cierto tiempo
    }

    void Update()
    {
        transform.Translate(Vector2.right * (speed * Time.deltaTime));
    }
}