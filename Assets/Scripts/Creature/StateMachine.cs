using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> : MonoBehaviour where T : StateMachine<T>
{
    private State<T> currentState;
	private State<T> nextState;

	public void ChangeState(State<T> _nextState)
	{
		nextState = _nextState;
	}

	private void FixedUpdate()
	{
		if (currentState != null)
		{
			currentState.OnUpdate();
		}

	    if(nextState != null)
		{
			if (currentState != null)
			{
				currentState.OnExit();
			}

			currentState = nextState;
			currentState.OnEnter();
            nextState = null;
		}
	}
}

public class State<T> where T : StateMachine<T>
{
    protected T m;

	public State(T _m) 
    { 
        m = _m; 
    }

    public virtual void OnEnter(){}
    public virtual void OnUpdate(){}
    public virtual void OnExit(){}
}
