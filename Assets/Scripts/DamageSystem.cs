using System;
using StateMachine;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private float damage;
    private Transform _playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerCollision"))
        {
            LifeSystem lifeSystem = other.gameObject.GetComponent<LifeSystem>();
            lifeSystem.GetDamage(damage);
        }
        else if (other.gameObject.CompareTag("PlayerDetection"))
        {
            _playerInRange = other.transform;
            FsmController fsm = GetComponentInParent<FsmController>();
            if (fsm != null)
            {
                fsm.SetTarget(_playerInRange); // Guardar al jugador como "target"
                fsm.EvaluateCombatState(); // FSM decide que accion realizar
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerDetection"))
        {
            FsmController fsm = GetComponentInParent<FsmController>();
            if (fsm != null)
            {
                fsm.ClearTarget(); // Elimina referencia del jugador
                fsm.EvaluateCombatState(); // FSM decide que accion realizar
            }
        }
    }
}