using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Walk : StateMachineBehaviour
{
    //Components
    public GameObject baal;
    public GameObject player;
    public NavMeshAgent agent;
    public Rigidbody rb;

    //Boss Stats
    public float speed; //Normal move Speed
    public float runSpeed; //Run Speed
    public float stepBackSpeed; //Speed boss walks back
    public float rotSpeed = 10f; //Spped Boss rotate towards player
    public float attackRange; //Boss attack Range
    public float walkRange; //Range from playing till walking
    public float jumpAttackRangeMin; //Min range from player to jump
    public float jumpAttackRangeMax; //Max range from player to jump

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get and Set Baal in Scene
        baal = GameObject.Find("Baal");
        
        //Get and set player
        player = GameObject.FindGameObjectWithTag("Player");
        
        //Get and set Components 
        agent = animator.GetComponent<NavMeshAgent>();
        rb = animator.GetComponent<Rigidbody>();
        
        //Set boss NavMesh Pro active
        agent.enabled = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if the boss Further away from player than 5 and is not roaring
        if (Vector3.Distance(player.transform.position, agent.transform.position) >= 5 && !animator.GetBool("Roar"))
        {
            //boss navmesh set active
            agent.enabled = true;
            //boss moves towards player
            agent.SetDestination(player.transform.position);
            //pick a random int
            int randomAnimation = Random.Range(0, 3);

            //if the player is out of walk range
            if (Vector3.Distance(player.transform.position, agent.transform.position) >= walkRange)
            {
                //Run
                animator.SetBool("Running", true);
                agent.speed = runSpeed;
            }
            else
            {
                //Walk
                animator.SetBool("Running", false);
                agent.speed = speed;
            }

            //if player is in attack range 
            if (Vector3.Distance(player.transform.position, agent.transform.position) <= attackRange)
            {
                //Stop moving
                agent.isStopped = true;

                //Rotation
                Vector3 direction = player.transform.position - agent.transform.position; //assign direction as player pos
                Quaternion rotation = Quaternion.LookRotation(direction); //assign rotation to look at that direction
                rotation.x = 0; //lock x rot
                rotation.z = 0; //lock z rot
                agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, rotation, rotSpeed * Time.deltaTime); //rotate the boss towards player smoothly

                //Make a random attack happen
                animator.SetInteger("Attack", randomAnimation);
            }
            else
            {
                //walk
                animator.SetInteger("Attack", 0);
                agent.isStopped = false;
            }

            //if polayer is within min max jumping range
            if (Vector3.Distance(player.transform.position, agent.transform.position) >= jumpAttackRangeMin && Vector3.Distance(player.transform.position, agent.transform.position) <= jumpAttackRangeMax)
            {
                //set animation jump
                animator.SetInteger("Attack", 3);

            }
        }
       
        //if the boss is within 5 of the player
        if(Vector3.Distance(player.transform.position, agent.transform.position) <= 5)
        { 
            //Turn off navmesh agent
            agent.enabled = false;
            
            //move backwards
            Vector3 direction = -baal.transform.forward.normalized; //set direction to - boss direction 
            rb.MovePosition(baal.transform.position + direction * stepBackSpeed* Time.deltaTime); //Move Boss away from player

            //roate boss to player same as before
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
        //when this script stops playing no attacks
        animator.SetInteger("Attack", 0);

    }



}
    



