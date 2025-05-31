using StateMachine;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected FsmController _controller;
    protected Animator _animator;

    public virtual void OnEnterState(FsmController controller)
    {
        this._controller = controller;
        _animator = gameObject.GetComponent<Animator>();
    }

    public abstract void OnUpdateState();

    public abstract void OnExitState();
}
