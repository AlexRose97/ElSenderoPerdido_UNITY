using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LifeSystem : MonoBehaviour
{
    [Header("Vida")] [SerializeField] private float maxHealthPerLife = 100f;
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Transform floatingTextPoint;
    private float _currentHealth;

    [Header("Intentos")] [SerializeField] private int totalLives = 3;

    [Header("Jugador o Enemigo")] [SerializeField]
    private bool isPlayer = false;


    [Header("Altura límite de caída")] [SerializeField]
    private float fallLimitY = -20f;

    private void Start()
    {
        _currentHealth = maxHealthPerLife;
    }

    private void Update()
    {
        //Caerse del mapa
        if (transform.position.y < fallLimitY)
        {
            if (isPlayer)
            {
                // checkpoints
                if (GameManager.Instance != null)
                    transform.position = GameManager.Instance.GetCheckpointPosition();
                else
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reinicia
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void GetDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log(floatingTextPoint.position);
        Debug.Log($"Daño recibido: {damage}. Salud restante: {_currentHealth}");
        GameObject instance = Instantiate(floatingTextPrefab,floatingTextPoint.position,
            Quaternion.identity);
        TextMeshPro floatingText = instance.GetComponent<TextMeshPro>();
        floatingText.color = isPlayer ? Color.red : Color.white;
        floatingText.text = (isPlayer ? "-" : "") + damage.ToString();
        Destroy(instance, 1.1f);
        if (_currentHealth <= 0)
        {
            totalLives--;
            if (totalLives > 0)
            {
                Debug.Log($"¡Perdiste una vida! Vidas restantes: {totalLives}");
                _currentHealth = maxHealthPerLife; // reinicia salud
                transform.position = GameManager.Instance.GetCheckpointPosition();//regresa al checkpoint
                //TODO: Agregar Animacion dead y tiempo invulnerable
            }
            else
            {
                Debug.Log("¡Game Over!");
                if (isPlayer)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reinicia nivel
                else
                    Destroy(gameObject);
            }
        }
    }
}