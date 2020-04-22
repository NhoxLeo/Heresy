using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionBehaviour : StateMachineBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;
    public Rigidbody rb;
   
    public float attackRange = 3;
    public float walkRange = 10;
    public float rotSpeed = 5f;

    public float speed;
    public float runSpeed;

   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player");

       agent = animator.GetComponent<NavMeshAgent>();

       agent.enabled = true;

       rb = animator.GetComponent<Rigidbody>();
    }

     //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        agent.SetDestination(player.transform.position);

        if (Vector3.Distance(player.transform.position, agent.transform.position) >= walkRange)
        {
            agent.speed = runSpeed;
        }
        else
        {
            agent.speed = speed;
        }
            

        if (Vector3.Distance(player.transform.position, agent.transform.position) <= attackRange)
        {
            
            agent.isStopped = true;
            Vector3 direction = player.transform.position - agent.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, rotation, rotSpeed * Time.deltaTime);
            
            animator.SetTrigger("Attack");
            
        }
        else
        {

            agent.isStopped = false;
        } 
        

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Enemy_Hit");
        agent.enabled = false;
    }


}
