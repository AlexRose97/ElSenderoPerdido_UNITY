using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    [SerializeField] private float lifes;

    public void GetDamage(float damage)
    {
        Debug.Log("Daño de: " + damage);
        lifes -= damage;
        if (lifes <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}