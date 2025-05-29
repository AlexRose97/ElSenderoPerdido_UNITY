using System;
using StateMachine;
using UnityEngine;

public class FSM_Controller : MonoBehaviour
{
    private State _currentState;
    private PatrolState _patrolState;
    private ChaseSate _chaseState;
    private AttackState _attackState;

    private void Awake()
    {
        _patrolState = GetComponent<PatrolState>();
        _chaseState = GetComponent<ChaseSate>();
        _attackState = GetComponent<AttackState>();
        _currentState = _patrolState;
        _currentState.OnEnterState(this);
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.OnUpdateState();
        }
    }

    public void ChangeState(State newState)
    {
        if (_currentState != null)
        {
            _currentState.OnExitState();
        }

        _currentState = newState;
        _currentState.OnEnterState(this);
    }
}