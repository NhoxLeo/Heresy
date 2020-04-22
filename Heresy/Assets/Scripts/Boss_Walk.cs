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

        
        
            int randomAnimation = Random.Range(0, 4);
          
           
                if (Vector3.Distance(player.transform.position, agent.transform.position) <= attackRange)
                {
                    animator.SetInteger("Attack", randomAnimation);
                }


        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)



