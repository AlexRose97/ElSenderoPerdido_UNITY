using UnityEngine;

namespace StateMachine
{
    public class ChaseState : State
    {
        [SerializeField] private float moveSpeed = 2f;
        private Transform _target;

        public override void OnEnterState(FsmController controller)
        {
            base.OnEnterState(controller);
            _target = _controller.Target; // Guardamos referencia al jugador
        }

        public override void OnUpdateState()
        {
            if (_target == null)
            {
                _controller.EvaluateCombatState(); // vuelve a evaluar
                return;
            }
            
            Vector3 targetPosition;

            if (_controller.FollowXOnly)
            {
                // Mantiene la posición Y actual del enemigo
                targetPosition = new Vector3(_target.position.x, transform.position.y, transform.position.z);
            }
            else
            {
                // Va directamente al punto (X, Y)
                targetPosition = new Vector3(_target.position.x, _target.position.y,
                    transform.position.z);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            //Animation Direction
            transform.eulerAngles = transform.position.x >= _target.position.x
                ? new Vector3(0, 180, 0)
                : Vector3.zero;
        }

        public override void OnExitState()
        {
        }
    }
}