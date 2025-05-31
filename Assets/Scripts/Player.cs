using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("MovementSystem")] [SerializeField]
    private float velocidadMovimiento;

    [SerializeField] private float fuerzaSalto;
    [SerializeField] private LayerMask jumpingLayer;
    [SerializeField] private Transform jumpingPoint;
    [SerializeField] private float jumpingRadio;

    [Header("CombatSystem")] [SerializeField]
    private Transform attackPoint;

    [SerializeField] private float attackRadio;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask damageLayer;

    private Animator _animator;
    private Rigidbody2D _rb;
    private float _inputHorizontal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per framea
    void Update()
    {
        Motion();
        Jump();
        Attack();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("attack1");
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && JumpHandler())
        {
            _rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            _animator.SetTrigger("jump");
        }
    }

    private void Motion()
    {
        _inputHorizontal = Input.GetAxis("Horizontal");
        _rb.linearVelocity = new Vector2(_inputHorizontal * velocidadMovimiento, _rb.linearVelocity.y);
        if (_inputHorizontal != 0)
        {
            _animator.SetBool("running", true);
            if (_inputHorizontal > 0)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            _animator.SetBool("running", false);
        }
    }

    private void AttackHandler()
    {
        Collider2D[] otherActors = Physics2D.OverlapCircleAll(attackPoint.position, attackRadio, damageLayer);
        foreach (Collider2D other in otherActors)
        {
            LifeSystem lifeSystem = other.gameObject.GetComponent<LifeSystem>();
            lifeSystem.GetDamage(damage);
        }
    }

    private bool JumpHandler()
    {
        return Physics2D.Raycast(jumpingPoint.position, Vector3.down, jumpingRadio, jumpingLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(attackPoint.position, attackRadio);
        Gizmos.DrawRay(jumpingPoint.position,Vector3.down);
    }
}