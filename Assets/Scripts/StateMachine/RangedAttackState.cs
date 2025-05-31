using UnityEngine;

namespace StateMachine
{
    public class RangedAttackState : AttackState
    {
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private Transform firePoint;
        public override void OnUpdateState()
        {
            if ((Time.time - LastAttackTime >= attackInterval) && this.TryAttackPlayerDirection())
            {
                _animator.SetTrigger("attack1");
                LastAttackTime = Time.time;
            }
        }

        private void SpawnAmmo()
        {
            Instantiate(fireballPrefab, firePoint.position, transform.rotation);
        }
    }
}
