using UnityEngine;

namespace StateMachine.AttackType
{
    public class MeleeAttackState : AttackBase
    {
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRadio;
        public override void OnUpdateState()
        {   
            if ((Time.time - LastAttackTime >= attackInterval) && this.TryAttackPlayerDirection())
            {
                _animator.SetTrigger("attack1");
                LastAttackTime = Time.time;
            }
        }
        
        private void MeleeAttackHandler()
        {
            Collider2D[] otherActors = Physics2D.OverlapCircleAll(attackPoint.position, attackRadio, _controller.DamageLayer);
            foreach (Collider2D other in otherActors)
            {
                DamageSystem newDamage = gameObject.GetComponent<DamageSystem>();
                bool result = newDamage.DamagePlayer(other);
                if(result) break;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(attackPoint.position, attackRadio);
        }
    }
}
