using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Walk : StateMachineBehaviour
{

    public float speed = 2.5f;
    public float attackRange = 3f;

    public GameObject player;
    public NavMeshAgent agent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.transform.position);

        for (int i = 0; i < 1; i++)

        {
            int randomAnimation = Random.Range(0, 4);
            if (randomAnimation == 1)
            {
                if (Vector3.Distance(player.transform.position, agent.transform.position) <= attackRange)
                {
                    animator.SetTrigger("Attack_Quick_Punch");
                }

            }

            else if (randomAnimation == 2)
            {
                if (Vector3.Distance(player.transform.position, agent.transform.position) <= attackRange)
                {
                    animator.SetTrigger("Attack_Big_Swipe");
                }


             
            }

            else if (randomAnimation == 3)
                {
                    if (Vector3.Distance(player.transform.position, agent.transform.position) <= attackRange)
                    {
                        animator.SetTrigger("Attack_Jump");
                    }


                }
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack_Quick_Punch");
        animator.ResetTrigger("Attack_Big_Swipe");
        animator.ResetTrigger("Attack_Jump");
    }


}
