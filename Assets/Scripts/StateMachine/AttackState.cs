using UnityEngine;

namespace StateMachine
{
    public class AttackState : State
    {
        protected float LastAttackTime;
        [SerializeField] protected float attackInterval = 3f; // Tiempo entre ataques
        [SerializeField] protected bool canAttackBehind = true;

        public override void OnEnterState(FsmController controller)
        {
            base.OnEnterState(controller);
            LastAttackTime = Time.time - attackInterval;
        }

        public override void OnUpdateState()
        {
            if ((Time.time - LastAttackTime >= attackInterval) && this.TryAttackPlayerDirection())
            {
                _animator.SetTrigger("attack1");
                LastAttackTime = Time.time;
            }
        }

        public override void OnExitState()
        {
        }

        /// <summary>
        /// Valida si el jugador está en la dirección correcta para atacar.
        /// Si está detrás y no se permite atacar desde atrás, cambia a patrullaje.
        /// También rota al enemigo hacia el jugador si es necesario.
        /// </summary>
        /// <returns>True si puede atacar, false si no.</returns>
        protected bool TryAttackPlayerDirection()
        {
            if (_controller.Target == null)
            {
                _controller.EvaluateCombatState();
                return false;
            }

            bool isInFront = IsPlayerInFront();
            bool canAttack = canAttackBehind || isInFront;

            if (!canAttack)
            {
                _controller.EvaluateCombatState();
                return false;
            }

            RotateTowardsTarget(_controller.Target);
            return true;
        }

        /// <summary>
        /// Determina si el jugador está frente al enemigo,
        /// según la rotación en Y (0 = derecha, 180 = izquierda).
        /// </summary>
        private bool IsPlayerInFront()
        {
            if (_controller.Target == null) return false;

            float deltaX = _controller.Target.position.x - transform.position.x;
            bool facingLeft = Mathf.Approximately(transform.eulerAngles.y, 180);

            // Si mira a la izquierda, el jugador debe estar a la izquierda
            return facingLeft ? deltaX < 0 : deltaX > 0;
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