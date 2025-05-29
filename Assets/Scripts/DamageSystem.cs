using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerCollision"))
        {
            LifeSystem lifeSystem = other.gameObject.GetComponent<LifeSystem>();
            lifeSystem.GetDamage(damage);
        }
        else if (other.gameObject.CompareTag("PlayerDetection"))
        {
            Debug.Log("TODO: Player en area");
        }
    }
}