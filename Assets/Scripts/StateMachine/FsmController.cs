using StateMachine.AttackType;
using UnityEngine;

namespace StateMachine
{
    public class FsmController : MonoBehaviour
    {
        [SerializeField] private bool followXOnly = true;
        [SerializeField] private bool isRangedEnemy = false;
        [SerializeField] private bool isMeleeEnemy = true;
        [SerializeField] private float meleeRange = 1.5f;
        [SerializeField] private float rangedRange = 5f;
        [SerializeField] private bool canChase = true;
        [SerializeField] private bool canAttackBehind = true;
        [SerializeField] private LayerMask damageLayer;
        private State _currentState;
        private MeleeAttackState _meleeAttackState;
        private RangedAttackState _rangedAttackState;
        private PatrolState _patrolState;
        private ChaseState _chaseState;
        private Transform _target;
        public void SetPatrolState() => ChangeState(_patrolState); //cambiar estado
        public Transform Target => _target;
        public LayerMask DamageLayer => damageLayer;
        public bool FollowXOnly => followXOnly;
        public void SetTarget(Transform t) => _target = t;
        public void ClearTarget() => _target = null;

        private void Awake()
        {
            _patrolState = GetComponent<PatrolState>();
            _chaseState = GetComponent<ChaseState>();
            _meleeAttackState = GetComponent<MeleeAttackState>();
            _rangedAttackState = GetComponent<RangedAttackState>();
            _currentState = _patrolState;
            _currentState.OnEnterState(this);
        }

        private void Update()
        {
            if (_currentState == null) return;
            _currentState.OnUpdateState();
            EvaluateCombatState();
        }


        public void ChangeState(State newState)
        {
            if (_currentState == newState) return; // Evita reentrar en el mismo estado

            _currentState?.OnExitState();
            _currentState = newState;
            _currentState.OnEnterState(this);
        }

        /// <summary>
        /// Evalúa la situación actual del jugador respecto al enemigo y decide 
        /// dinámicamente a qué estado debe cambiar la FSM del enemigo:
        /// - Melee si está muy cerca.
        /// - Ranged si está a media distancia.
        /// - Perseguir si está fuera de rango.
        /// - Patrullar si no hay target o no puede perseguir.
        /// Esta lógica admite enemigos melee, ranged o híbridos.
        /// </summary>
        public void EvaluateCombatState()
        {
            // Si no hay objetivo, el enemigo patrulla
            if (Target == null)
            {
                ChangeState(_patrolState);
                return;
            }

            bool isInFront = IsPlayerInFront();
            if (!canAttackBehind && !isInFront)
            {
                //return;
                //Si el enemigo esta de espaldas y no tiene permitido girar hacia el jugador
                ChangeState(_patrolState);
                return;
            }

            // Calculamos la distancia horizontal al jugador
            float distanceToPlayer = Mathf.Abs(Target.position.x - transform.position.x);

            // Flags para simplificar la lectura
            bool inMeleeRange = distanceToPlayer <= meleeRange;
            bool inRangedRange = distanceToPlayer <= rangedRange;

            // Enemigo híbrido: puede hacer melee y ataque a distancia
            if (isMeleeEnemy && isRangedEnemy)
            {
                if (inMeleeRange)
                {
                    ChangeState(_meleeAttackState); // Ataca cuerpo a cuerpo
                    return;
                }
                else if (inRangedRange)
                {
                    ChangeState(_rangedAttackState); // Ataca a distancia
                    return;
                }
            }
            // Solo puede atacar cuerpo a cuerpo
            else if (isMeleeEnemy && inMeleeRange)
            {
                ChangeState(_meleeAttackState);
                return;
            }
            // Solo puede atacar a distancia
            else if (isRangedEnemy && inRangedRange)
            {
                ChangeState(_rangedAttackState);
                return;
            }

            // Si no puede atacar pero puede perseguir, lo hace
            if (canChase)
            {
                ChangeState(_chaseState);
            }
            else
            {
                // No puede atacar ni perseguir: regresa a patrullar
                ChangeState(_patrolState);
            }
        }

        /// <summary>
        /// Determina si el jugador está frente al enemigo,
        /// según la rotación en Y (0 = derecha, 180 = izquierda).
        /// </summary>
        private bool IsPlayerInFront()
        {
            if (Target == null) return false;

            float deltaX = Target.position.x - transform.position.x;
            bool facingLeft = Mathf.Approximately(transform.eulerAngles.y, 180);

            // Si mira a la izquierda, el jugador debe estar a la izquierda
            return facingLeft ? deltaX < 0 : deltaX > 0;
        }
    }
}