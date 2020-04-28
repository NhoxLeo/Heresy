using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Walk : StateMachineBehaviour
{

    public float speed = 3f;
    public float runSpeed = 6f;
    public float attackRange = 3f;
    public float stepBackSpeed = 0.1f;
    public float jumpAttackRangeMin = 7f;
    public float jumpAttackRangeMax = 8f;
    
    public float walkRange = 12;

    public float rotSpeed = 10f;
    public GameObject baal;
    public GameObject player;
    public NavMeshAgent agent;
    public Rigidbody rb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        baal = GameObject.Find("Baal");
        player = GameObject.FindGameObjectWithTag("Player");
        agent = animator.GetComponent<NavMeshAgent>();
        rb = animator.GetComponent<Rigidbody>();
        agent.enabled = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        if (Vector3.Distance(player.transform.position, agent.transform.position) >= 5 && !animator.GetBool("Roar"))
        {
            agent.enabled = true;
            agent.SetDestination(player.transform.position);

            int randomAnimation = Random.Range(0, 4);


            if (Vector3.Distance(player.transform.position, agent.transform.position) >= walkRange)
            {
                animator.SetBool("Running", true);
                agent.speed = runSpeed;
            }
            else
            {
                animator.SetBool("Running", false);
                agent.speed = speed;
            }

            if (Vector3.Distance(player.transform.position, agent.transform.position) <= attackRange)
            {
                agent.isStopped = true;


                Vector3 direction = player.transform.position - agent.transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                rotation.x = 0;
                rotation.z = 0;
                agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, rotation, rotSpeed * Time.deltaTime);


                animator.SetInteger("Attack", randomAnimation);
            }
            else
            {
                animator.SetInteger("Attack", 0);
                agent.isStopped = false;
            }

            if (Vector3.Distance(player.transform.position, agent.transform.position) >= jumpAttackRangeMin && Vector3.Distance(player.transform.position, agent.transform.position) <= jumpAttackRangeMax)
            {
                animator.SetInteger("Attack", 3);

            }
        }
       
        if(Vector3.Distance(player.transform.position, agent.transform.position) <= 5)
        { 
            agent.enabled = false;
            
            Vector3 direction = -baal.transform.forward.normalized;
            rb.MovePosition(baal.transform.position + direction * stepBackSpeed* Time.deltaTime);

            Vector3 rotDirection = player.transform.position - baal.transform.position;
            Quaternion rotation = Quaternion.LookRotation(rotDirection);
            rotation.x = 0;
            rotation.z = 0;
            baal.transform.rotation = Quaternion.Lerp(baal.transform.rotation, rotation, rotSpeed * Time.deltaTime);
        }


    }

 
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("Attack", 0);

    }

}
    



