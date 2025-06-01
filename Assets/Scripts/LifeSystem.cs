using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

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

    [Header("UI Elements")] [SerializeField]
    private TextMeshProUGUI textLife;

    [SerializeField] private TextMeshProUGUI textPoints;
    [SerializeField] private Slider sliderLife;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartContainer;
    private List<GameObject> _hearts = new List<GameObject>();

    private void Start()
    {
        _currentHealth = maxHealthPerLife;
        this.UpdateHud();
    }

    private void Update()
    {
        //Caerse del mapa
        if (transform.position.y < fallLimitY)
        {
            if (isPlayer)
            {
                this.SubtractLives();
                this.UpdateHud();
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
        GameObject instance = Instantiate(floatingTextPrefab, floatingTextPoint.position,
            Quaternion.identity);
        TextMeshPro floatingText = instance.GetComponent<TextMeshPro>();
        floatingText.color = isPlayer ? Color.red : Color.white;
        floatingText.text = (isPlayer ? "-" : "") + damage.ToString();
        Destroy(instance, 1.1f);
        if (_currentHealth <= 0)
        {
            SubtractLives();
        }

        this.UpdateHud();
    }

    private void SubtractLives()
    {
        totalLives--;
        if (totalLives > 0)
        {
            Debug.Log($"¡Perdiste una vida! Vidas restantes: {totalLives}");
            _currentHealth = maxHealthPerLife; // reinicia salud
            if (GameManager.Instance != null)
            {
                transform.position = GameManager.Instance.GetCheckpointPosition(); //regresa al checkpoint
            }
            //TODO: Agregar Animacion dead y tiempo invulnerable
        }
        else
        {
            Debug.Log("¡Game Over!");
            if (isPlayer)
            {
                GameManager.Instance.FinishLevel(false);
                PauseMenuManager.Instance.ShowGameOverMenu(); // muestra UI
            }
            else
                Destroy(gameObject);
        }
    }

    private void UpdateHud()
    {
        if (isPlayer)
        {
            sliderLife.value = _currentHealth / maxHealthPerLife;
            textLife.SetText($"{_currentHealth}/{maxHealthPerLife}");
            this.UpdateHearts();
        }
    }

    public void UpdateHearts()
    {
        // Elimina corazones anteriores
        foreach (var heart in _hearts)
            Destroy(heart);
        _hearts.Clear();

        // Crea nuevos corazones
        for (int i = 1; i < totalLives; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            _hearts.Add(heart);
        }
    }
}