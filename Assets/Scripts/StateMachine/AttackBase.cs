using UnityEngine;

namespace StateMachine
{
    public class AttackBase : State
    {
        protected float LastAttackTime;
        [SerializeField] protected float attackInterval = 3f; // Tiempo entre ataques
        public override void OnEnterState(FsmController controller)
        {
            base.OnEnterState(controller);
            LastAttackTime = Time.time - attackInterval;
        }

        public override void OnUpdateState()
        {
        }
        public override void OnExitState()
        {
        }

        /// <summary>
        /// Valida si el jugador está en la dirección correcta para atacar.
        /// Rota al enemigo hacia el jugador si es necesario.
        /// </summary>
        /// <returns>True si puede atacar, false si no.</returns>
        protected bool TryAttackPlayerDirection()
        {
            if (_controller.Target == null)
            {
                _controller.EvaluateCombatState();
                return false;
            }
            
            RotateTowardsTarget(_controller.Target);
            return true;
        }
        
        /// <summary>
        /// Gira al enemigo para mirar hacia el objetivo.
        /// </summary>
        protected void RotateTowardsTarget(Transform target)
        {
            if (target == null) return;

            transform.eulerAngles = transform.position.x >= target.position.x
                ? new Vector3(0, 180, 0)
                : Vector3.zero;
        }
    }
}