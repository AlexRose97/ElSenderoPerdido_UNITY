using UnityEngine;

namespace StateMachine.AttackType
{
    public class RangedAttackState : AttackBase
    {
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private Transform firePoint;
        public override void OnUpdateState()
        {
            if ((Time.time - LastAttackTime >= attackInterval) && this.TryAttackPlayerDirection())
            {
                _animator.SetTrigger("ranged_attack");
                LastAttackTime = Time.time;
            }
        }

        private void RangedAttackHandler()
        {
            Instantiate(fireballPrefab, firePoint.position, transform.rotation);
        }
    }
}
