using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionBehaviour : StateMachineBehaviour
{
   
    public GameObject player;
    public NavMeshAgent agent;
    public Rigidbody rb;
    
    public int noAttack = 1;
    public float attackRange = 2;
    public float rotRange = 5;
    public float walkRange = 10;

    public float rotSpeed = 10f;
    public float speed;
    public float runSpeed;

   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player");
       agent = animator.GetComponent<NavMeshAgent>();
       rb = animator.GetComponent<Rigidbody>();

       agent.enabled = true;

       
    }

     //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //move to player
        agent.SetDestination(player.transform.position);

        
       //If out of walk range, RUN
        if (Vector3.Distance(player.transform.position, agent.transform.position) >= walkRange)
        {
            
            agent.speed = runSpeed;
        }
        else
        {
            
            agent.speed = speed;
        }
        

        //if in attack range
        if (Vector3.Distance(player.transform.position, agent.transform.position) <= attackRange)
        {
            //dont move
            agent.isStopped = true;

            //roate to player
            Vector3 direction = player.transform.position - agent.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, rotation, rotSpeed * Time.deltaTime);

            //Attack
            animator.SetTrigger("Attack");

        }
        else
        {
           //move
           agent.isStopped = false;
            
        }
        

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //leaving this animation resets everything
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Enemy_Hit");
        agent.enabled = false;
    }


}
