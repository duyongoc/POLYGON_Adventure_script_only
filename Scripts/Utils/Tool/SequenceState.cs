using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceState : MonoBehaviour
{

    public List<IState> states;
    public IState currentState;
    public int currentStateIndex;




    IEnumerator StateSequence()
    {
        while (currentStateIndex < states.Count)
        {
            ChangeState(states[currentStateIndex]);
            yield return new WaitForSeconds(2);
            currentStateIndex++;
        }
    }


    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }


    // void Update()
    // {
    //     stateMachine.Update();
    // }
}


public class AState : IState
{
    public void Enter() { Debug.Log("Entering AState State"); }
    public void Execute() { Debug.Log("Executing AState State"); }
    public void Exit() { Debug.Log("Exiting AState State"); }
}

public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}