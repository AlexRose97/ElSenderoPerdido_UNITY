using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected FSM_Controller _controller;

    public virtual void OnEnterState(FSM_Controller controller)
    {
        this._controller = controller;
    }

    public abstract void OnUpdateState();

    public abstract void OnExitState();
}
