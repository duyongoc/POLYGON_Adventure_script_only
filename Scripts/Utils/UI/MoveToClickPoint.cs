using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour
{


    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;



    public void SetMove(Vector3 position)
    {
        agent.SetDestination(position);
    }

    private void Update()
    {
        animator.SetFloat("Blend", agent.velocity.magnitude);
    }


}