using UnityEngine;

namespace StateMachine
{
    public class FsmController : MonoBehaviour
    {
        [SerializeField] private bool followXOnly = true;

        [SerializeField] private bool canPatrol = true;

        //[SerializeField] private bool canAttack = true;
        [SerializeField] private bool canChase = true;
        [SerializeField] private bool isRangedEnemy = false;
        [SerializeField] private bool isMeleeEnemy = true;
        [SerializeField] private float meleeRange = 1.5f;
        [SerializeField] private float rangedRange = 5f;
        private State _currentState;
        private AttackState _meleeAttackState;
        private RangedAttackState _rangedAttackState;
        private PatrolState _patrolState;
        private ChaseState _chaseState;
        private Transform _target;
        public void SetPatrolState() => ChangeState(_patrolState); //cambiar estado
        public Transform Target => _target;
        public bool FollowXOnly => followXOnly;
        public void SetTarget(Transform t) => _target = t;
        public void ClearTarget() => _target = null;

        private void Awake()
        {
            _patrolState = GetComponent<PatrolState>();
            _chaseState = GetComponent<ChaseState>();
            _meleeAttackState = GetComponent<AttackState>();
            _rangedAttackState = GetComponent<RangedAttackState>();
            _currentState = _patrolState;
            _currentState.OnEnterState(this);
        }

        private void Update()
        {
            if (_currentState == null) return;

            _currentState.OnUpdateState();

            if (Target == null || _currentState is PatrolState) return;

            float distanceToPlayer = Mathf.Abs(Target.position.x - transform.position.x);
            bool inMeleeRange = isMeleeEnemy && distanceToPlayer <= meleeRange;
            bool inRangedRange = isRangedEnemy && distanceToPlayer <= rangedRange;

            Debug.LogWarning($"[FSM] inMeleeRange: {inMeleeRange}, inRangedRange: {inRangedRange}, State: {_currentState.GetType().Name}");

            // 1. Si estoy en persecución y entro en rango → cambiar a ataque
            if (_currentState is ChaseState && (inMeleeRange || inRangedRange))
            {
                EvaluateCombatState();
            }

            // 2. Si estoy en ataque y me alejé → volver a perseguir
            else if (
                (_currentState is AttackState && !inMeleeRange) ||
                (_currentState is RangedAttackState && !inRangedRange))
            {
                EvaluateCombatState();
            }
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
                    Debug.Log("meleAttackHIBRID"+ " inMeleeRange:"+inMeleeRange+" inRangedRange:"+inRangedRange);
                    ChangeState(_meleeAttackState); // Ataca cuerpo a cuerpo
                    return;
                }
                else if (inRangedRange)
                {
                    Debug.Log("rangedAttackHIBRID"+ " inMeleeRange:"+inMeleeRange+" inRangedRange:"+inRangedRange);
                    ChangeState(_rangedAttackState); // Ataca a distancia
                    return;
                }
            }
            // Solo puede atacar cuerpo a cuerpo
            else if (isMeleeEnemy && inMeleeRange)
            {
                Debug.Log("meleAttack"+ " inMeleeRange:"+inMeleeRange+" inRangedRange:"+inRangedRange);
                ChangeState(_meleeAttackState);
                return;
            }
            // Solo puede atacar a distancia
            else if (isRangedEnemy && inRangedRange)
            {
                Debug.Log("rangedAttack"+ " inMeleeRange:"+inMeleeRange+" inRangedRange:"+inRangedRange);
                ChangeState(_rangedAttackState);
                return;
            }

            // Si no puede atacar pero puede perseguir, lo hace
            if (canChase)
            {
                Debug.Log("Chase"+ " inMeleeRange:"+inMeleeRange+" inRangedRange:"+inRangedRange);
                ChangeState(_chaseState);
            }
            else
            {
                Debug.Log("patrol"+ " inMeleeRange:"+inMeleeRange+" inRangedRange:"+inRangedRange);
                // No puede atacar ni perseguir: regresa a patrullar
                ChangeState(_patrolState);
            }
        }
    }
}